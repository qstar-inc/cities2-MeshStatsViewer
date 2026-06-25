//using System.Collections.Generic;
//using Colossal.Entities;
//using Colossal.UI.Binding;
//using Game;
//using Game.Prefabs;
//using Game.UI;
//using MeshStatsViewer.Extensions;
//using MeshStatsViewer.Variables;
//using StarQ.Shared.Extensions;
//using Unity.Entities;

//namespace MeshStatsViewer.Systems
//{
//    public partial class UI_MSV : ExtendedUISystemBase
//    {
//        public override GameMode gameMode => GameMode.Game;

//#nullable disable
//        public static bool hasMesh = false;

//        private NameSystem nameSystem;
//        private PrefabSystem prefabSystem;

//        //public static MeshStats[] meshStatsInfo = new MeshStats[0];
//        public static Dictionary<string, List<MeshStats>> meshStatDict = new();

//        private ValueBindingHelper<MeshStats[]> MeshStats;

//#nullable enable

//        protected override void OnCreate()
//        {
//            base.OnCreate();
//            //m_InfoUISystem.AddMiddleSection(this);

//            nameSystem = WorldHelper.NameSystem;
//            prefabSystem = WorldHelper.PrefabSystem;

//            MeshStats = CreateBinding("GetMeshStats", new MeshStats[0]);

//            Enabled = false;
//        }

//        protected override void OnUpdate()
//        {
//            base.OnUpdate();
//            if (!WorldHelper.IsGame)
//                return;
//            CheckMesh();
//        }

//        public override void OnWriteProperties(IJsonWriter writer)
//        {
//            writer.PropertyName("hasMesh");
//            writer.Write(hasMesh);

//            writer.PropertyName("meshStatsInfo");
//            MeshStatsWriter.Write(writer, meshStatsInfo);
//        }

//        protected override void Reset()
//        {
//            //bldgBrandInfo = new();

//            hasMesh = false;
//        }

//        private bool Visible()
//        {
//            return false;
//            //if (!EntityManager.TryGetComponent(selectedEntity, out PrefabRef _))
//            //    return false;
//            //if (!prefabSystem.TryGetPrefab(selectedPrefab, out PrefabBase _))
//            //    return false;

//            //if (
//            //    EntityManager.HasComponent<BuildingData>(selectedPrefab)
//            //    || EntityManager.HasComponent<BuildingExtensionData>(selectedPrefab)
//            //)
//            //    return true;

//            //return false;
//        }

//        protected override void OnProcess()
//        {
//            Reset();
//            CheckMesh();
//            List<string> text = new();
//            foreach (var item in meshStatsInfo)
//            {
//                text.Add($"{item.LODCount} lods found on the RenderPrefab");
//                foreach (var item2 in item.MeshStatData)
//                {
//                    text.Add(
//                        $"{item2.SurfaceArea} SurfaceArea, {item2.VertexCount} VertexCount, {item2.IndexCount} IndexCount"
//                    );
//                }
//            }
//            if (text.Count > 0)
//                LogHelper.SendLog(string.Join("\n", text));
//        }

//        public void CheckMesh()
//        {
//            if (
//                EntityManager.TryGetBuffer(selectedPrefab, true, out DynamicBuffer<SubMesh> subMesh)
//                && subMesh.Length > 0
//            )
//                hasMesh = true;

//            if (!hasMesh)
//                return;

//            string prefabName = PrefabHelper.GetPrefabName(selectedPrefab) ?? string.Empty;

//            if (meshStatDict.TryGetValue(prefabName, out List<MeshStats> meshStatsFromDict))
//            {
//                meshStatsInfo = meshStatsFromDict.ToArray();
//                return;
//            }

//            List<MeshStats> a = new();

//            for (int i = 0; i < subMesh.Length; i++)
//            {
//                Entity currentMesh = subMesh[i].m_SubMesh;
//                MeshStats b = new();
//                List<MeshStatData> b1 = new();

//                if (
//                    prefabSystem.TryGetPrefab(currentMesh, out PrefabBase meshPrefab)
//                    && meshPrefab is RenderPrefab meshPrefab_rp
//                )
//                {
//                    b1.Add(
//                        new MeshStatData()
//                        {
//                            VertexCount = meshPrefab_rp.vertexCount,
//                            IndexCount = meshPrefab_rp.indexCount,
//                            SurfaceArea = meshPrefab_rp.surfaceArea,
//                        }
//                    );
//                }

//                if (
//                    EntityManager.TryGetBuffer(
//                        currentMesh,
//                        true,
//                        out DynamicBuffer<LodMesh> lodMesh
//                    )
//                    && lodMesh.Length > 0
//                )
//                {
//                    b.LODCount = lodMesh.Length + 1;
//                    for (int j = 0; j < lodMesh.Length; j++)
//                    {
//                        Entity currentLod = lodMesh[j].m_LodMesh;

//                        if (
//                            prefabSystem.TryGetPrefab(currentLod, out PrefabBase lodPrefab)
//                            && lodPrefab is RenderPrefab lodPrefab_rp
//                        )
//                        {
//                            b1.Add(
//                                new MeshStatData()
//                                {
//                                    SurfaceArea = lodPrefab_rp.surfaceArea,
//                                    IndexCount = lodPrefab_rp.indexCount,
//                                    VertexCount = lodPrefab_rp.vertexCount,
//                                }
//                            );
//                        }
//                    }
//                }
//                b.MeshStatData = b1.ToArray();
//                a.Add(b);
//            }
//            meshStatDict[prefabName] = a;

//            meshStatsInfo = a.ToArray();
//        }
//    }
//}
