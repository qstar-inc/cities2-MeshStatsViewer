using System;
using Colossal.Entities;
using Colossal.UI.Binding;
using Game;
using Game.Prefabs;
using MeshStatsViewer.Extensions;
using MeshStatsViewer.Types;
using StarQ.Shared.Extensions;
using StarQ.Shared.Extensions.UI;
using Unity.Entities;

namespace MeshStatsViewer.Systems
{
    public partial class SIPSystem : ExtendedInfoSectionBase
    {
        public override GameMode gameMode => GameMode.Game;
        protected override string group
        {
            get { return nameof(MeshStatsViewer); }
        }
        protected override bool displayForUpgrades
        {
            get { return true; }
        }

#nullable disable
        public static string prefabName = "";
        public static bool hasMesh = false;

        private PrefabSystem prefabSystem;

        public static MeshStats[] meshStatsInfo = Array.Empty<MeshStats>();

#nullable enable

        protected override void OnCreate()
        {
            base.OnCreate();
            m_InfoUISystem.AddMiddleSection(this);

            prefabSystem = WorldHelper.PrefabSystem;
            Enabled = false;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            visible = Visible();
        }

        public override void OnWriteProperties(IJsonWriter writer)
        {
            writer.PropertyName("prefabName");
            writer.Write(prefabName);

            writer.PropertyName("hasMesh");
            writer.Write(hasMesh);

            writer.PropertyName("meshStatsInfo");
            MeshStatsWriter.Write(writer, meshStatsInfo);

            writer.PropertyName("meshViewerOptions");
            MeshViewerOptionsWriter.Write(writer, OptionsDefaults.GetCurrent());
        }

        protected override void Reset()
        {
            prefabName = string.Empty;
            hasMesh = false;
            meshStatsInfo = Array.Empty<MeshStats>();
        }

        private bool Visible()
        {
            Reset();
            if (!EntityManager.TryGetComponent(selectedEntity, out PrefabRef _))
                return false;
            if (!prefabSystem.TryGetPrefab(selectedPrefab, out PrefabBase pb) || pb == null)
                return false;
            if (pb.isBuiltin && !Mod.m_Setting.EnableVanilla)
                return false;

            if (
                EntityManager.IsComponentEnabled<PrefabData>(selectedPrefab)
                && (
                    EntityManager.TryGetBuffer(
                        selectedPrefab,
                        true,
                        out DynamicBuffer<SubMesh> subMesh
                    )
                    && subMesh.Length > 0
                )
            )
            {
                hasMesh = true;
                return true;
            }

            return false;
        }

        protected override void OnProcess()
        {
            if (!hasMesh)
                return;
            CheckMeshSystem.CheckMesh(
                EntityManager,
                prefabSystem,
                selectedPrefab,
                out prefabName,
                out meshStatsInfo
            );
        }
    }
}
