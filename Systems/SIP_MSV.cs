using System.Collections.Generic;
using Colossal.Entities;
using Colossal.PSI.Environment;
using Colossal.UI.Binding;
using Game;
using Game.Prefabs;
using MeshStatsViewer.Extensions;
using MeshStatsViewer.Types;
using StarQ.Shared.Extensions;
using StarQ.Shared.Extensions.UI;
using Unity.Entities;
using Unity.Mathematics;

namespace MeshStatsViewer.Systems
{
    public partial class SIP_MSV : ExtendedInfoSectionBase
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

        public static MeshStats[] meshStatsInfo = new MeshStats[0];
        public static Dictionary<string, List<MeshStats>> meshStatDict = new();

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

            writer.PropertyName("options");
            OptionsWriter.Write(
                writer,
                new Options()
                {
                    ViewChooser = Mod.m_Setting.ViewChooser,
                    LOD1Threshold = Mod.m_Setting.LOD1Threshold,
                    LOD2Threshold = Mod.m_Setting.LOD2Threshold,
                    TrisThreshold = Mod.m_Setting.TrisThreshold,
                    VolumeThreshold = Mod.m_Setting.VolumeThreshold,
                    CheekyMode = Mod.m_Setting.CheekyMode,
                }
            );
        }

        protected override void Reset()
        {
            prefabName = string.Empty;
            hasMesh = false;
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
            CheckMesh();
        }

        public void CheckMesh()
        {
            if (!hasMesh)
                return;
            if (
                !EntityManager.TryGetBuffer(
                    selectedPrefab,
                    true,
                    out DynamicBuffer<SubMesh> subMesh
                )
                || subMesh.Length <= 0
            )
                return;

            prefabName = PrefabHelper.GetPrefabName(selectedPrefab) ?? string.Empty;

            if (meshStatDict.TryGetValue(prefabName, out List<MeshStats> meshStatsFromDict))
            {
                meshStatsInfo = meshStatsFromDict.ToArray();
                return;
            }

            List<MeshStats> a = new();

            for (int i = 0; i < subMesh.Length; i++)
            {
                Entity currentMesh = subMesh[i].m_SubMesh;
                MeshStats b = new() { };
                List<MeshStatData> b1 = new();
                int prevTris = 0;

                if (
                    prefabSystem.TryGetPrefab(currentMesh, out PrefabBase meshPrefab)
                    && meshPrefab is RenderPrefab meshPrefab_rp
                )
                {
                    RenderPrefab rp = meshPrefab_rp;

                    int trisCount = rp.indexCount / 3;
                    prevTris = trisCount;
                    float width = math.abs(rp.bounds.min.x - rp.bounds.max.x);
                    float height = math.abs(rp.bounds.min.y - rp.bounds.max.y);
                    float depth = math.abs(rp.bounds.min.z - rp.bounds.max.z);
                    b1.Add(
                        new MeshStatData()
                        {
                            Name = rp.name,
                            SurfaceArea = rp.surfaceArea,
                            //IndexCount = rp.indexCount,
                            VertexCount = rp.vertexCount,
                            TrisCount = trisCount,
                            TrisDensity = trisCount / rp.surfaceArea,
                            TrisReduction = -1,
                            //VertexDensity = rp.vertexCount / rp.surfaceArea,
                            Volume = width * height * depth,
                        }
                    );
                }

                if (
                    EntityManager.TryGetBuffer(
                        currentMesh,
                        true,
                        out DynamicBuffer<LodMesh> lodMesh
                    )
                    && lodMesh.Length > 0
                )
                {
                    b.LODCount = lodMesh.Length;
                    for (int j = 0; j < lodMesh.Length; j++)
                    {
                        Entity currentLod = lodMesh[j].m_LodMesh;

                        if (
                            prefabSystem.TryGetPrefab(currentLod, out PrefabBase lodPrefab)
                            && lodPrefab is RenderPrefab lodPrefab_rp
                        )
                        {
                            RenderPrefab rp = lodPrefab_rp;

                            int trisCount = rp.indexCount / 3;
                            float reduction = 100 * (prevTris - trisCount) / prevTris;
                            prevTris = trisCount;
                            float width = math.abs(rp.bounds.min.x - rp.bounds.max.x);
                            float height = math.abs(rp.bounds.min.y - rp.bounds.max.y);
                            float depth = math.abs(rp.bounds.min.z - rp.bounds.max.z);
                            b1.Add(
                                new MeshStatData()
                                {
                                    Name = rp.name,
                                    SurfaceArea = rp.surfaceArea,
                                    //IndexCount = rp.indexCount,
                                    VertexCount = rp.vertexCount,
                                    TrisCount = trisCount,
                                    TrisReduction = reduction,
                                    TrisDensity = trisCount / rp.surfaceArea,
                                    //VertexDensity = rp.vertexCount / rp.surfaceArea,
                                    Volume = width * height * depth,
                                }
                            );
                        }
                    }
                }
                b.MeshStatData = b1.ToArray();
                a.Add(b);
            }
            meshStatDict[prefabName] = a;

            meshStatsInfo = a.ToArray();
        }
    }
}
