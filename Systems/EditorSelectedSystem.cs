using System;
using Colossal.Serialization.Entities;
using Game;
using Game.Prefabs;
using Game.Tools;
using Game.UI.Editor;
using MeshStatsViewer.Types;
using StarQ.Shared.Extensions;
using StarQ.Shared.Extensions.UI;
using Unity.Entities;

namespace MeshStatsViewer.Systems
{
    public partial class EditorSelectedSystem : ExtendedUISystemBase
    {
        public override GameMode gameMode => GameMode.Editor;

        private InspectorPanelSystem inspector;
        private ToolSystem toolSystem;
        private PrefabSystem prefabSystem;

        private PrefabBase lastPrefab;

        private ValueBindingHelper<string> prefabNameBinding;
        private ValueBindingHelper<bool> hasMeshBinding;
        private ValueBindingHelper<MeshStats[]> meshStatsBinding;
        private ValueBindingHelper<MeshViewerOptions> meshViewerOptionsBinding;

        protected override void OnCreate()
        {
            base.OnCreate();
            inspector = WorldHelper.InspectorPanelSystem;
            toolSystem = WorldHelper.ToolSystem;
            prefabSystem = WorldHelper.PrefabSystem;
            prefabNameBinding = CreateBinding("prefabNameBinding", string.Empty);
            hasMeshBinding = CreateBinding("hasMeshBinding", false);
            meshStatsBinding = CreateBinding("meshStatsBinding", Array.Empty<MeshStats>());
            meshViewerOptionsBinding = CreateBinding(
                "meshViewerOptionsBinding",
                new MeshViewerOptions()
            );
            Enabled = false;
        }

        protected override void OnGamePreload(Purpose purpose, GameMode mode)
        {
            base.OnGamePreload(purpose, mode);
            lastPrefab = null;
            Enabled = false;
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            Enabled = WorldHelper.IsEditor;
            if (Enabled)
                Reset();
        }

        protected override void OnUpdate()
        {
            if (WorldHelper.IsEditor)
                UpdateSelection();
            base.OnUpdate();
        }

        private void UpdateSelection()
        {
            PrefabBase prefabBase =
                toolSystem.activePrefab ?? inspector.selectedObject as PrefabBase;

            if (ReferenceEquals(prefabBase, lastPrefab))
                return;
            lastPrefab = prefabBase;

            if (
                prefabBase == null
                || !prefabSystem.TryGetEntity(prefabBase, out Entity prefabEntity)
            )
            {
                Reset();
                return;
            }

            if (
                CheckMeshSystem.CheckMesh(
                    EntityManager,
                    prefabSystem,
                    prefabEntity,
                    out string prefabName,
                    out MeshStats[] meshStatsInfo
                )
            )
            {
                hasMeshBinding.Value = true;
                prefabNameBinding.Value = prefabName;
                meshStatsBinding.Value = meshStatsInfo;
                meshViewerOptionsBinding.Value = OptionsDefaults.GetCurrent();
            }
            else
                Reset();
        }

        private void Reset()
        {
            hasMeshBinding.Value = false;
            prefabNameBinding.Value = string.Empty;
            meshStatsBinding.Value = Array.Empty<MeshStats>();
            meshViewerOptionsBinding.Value = new();
        }
    }
}
