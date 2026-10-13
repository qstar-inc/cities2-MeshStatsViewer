using System;
using System.Collections.Generic;
using Colossal.Entities;
using Game.Prefabs;
using MeshStatsViewer.Types;
using Unity.Entities;
using Unity.Mathematics;

namespace MeshStatsViewer.Systems
{
    internal static class CheckMeshSystem
    {
        public static Dictionary<string, List<MeshStats>> meshStatDict = new();

        public static bool CheckMesh(
            EntityManager em,
            PrefabSystem ps,
            Entity sp,
            out string prefabName,
            out MeshStats[] meshStatsInfo
        ) => CheckMesh(em, ps, sp, false, out prefabName, out meshStatsInfo);

        public static bool CheckMesh(
            EntityManager em,
            PrefabSystem ps,
            Entity sp,
            bool skipVanilla,
            out string prefabName,
            out MeshStats[] meshStatsInfo
        )
        {
            prefabName = null;
            meshStatsInfo = Array.Empty<MeshStats>();

            if (!ps.TryGetPrefab(sp, out PrefabBase pb) || pb == null)
                return false;

            if (skipVanilla && pb.isBuiltin)
                return false;

            prefabName = pb.name ?? string.Empty;
            List<MeshStats> setGlobal = new();

            if (pb is RenderPrefab)
            {
                if (meshStatDict.TryGetValue(prefabName, out List<MeshStats> meshStatsFromDict))
                {
                    meshStatsInfo = meshStatsFromDict.ToArray();
                    return true;
                }

                if (!ProcessMesh(em, ps, sp, skipVanilla, ref setGlobal))
                    return false;
            }
            else
            {
                if (
                    !em.TryGetBuffer(sp, true, out DynamicBuffer<SubMesh> subMesh)
                    || subMesh.Length <= 0
                )
                    return false;

                if (meshStatDict.TryGetValue(prefabName, out List<MeshStats> meshStatsFromDict))
                {
                    meshStatsInfo = meshStatsFromDict.ToArray();
                    return true;
                }

                for (int i = 0; i < subMesh.Length; i++)
                {
                    if (!ProcessMesh(em, ps, subMesh[i].m_SubMesh, skipVanilla, ref setGlobal))
                        continue;
                }
            }
            meshStatDict[prefabName] = setGlobal;
            meshStatsInfo = setGlobal.ToArray();
            return true;
        }

        public static bool ProcessMesh(
            EntityManager em,
            PrefabSystem ps,
            Entity currentMesh,
            bool skipVanilla,
            ref List<MeshStats> setGlobal
        )
        {
            MeshStats setLocal = new() { };
            List<MeshStatData> b1 = new();
            int currentTris = 0;

            if (
                ps.TryGetPrefab(currentMesh, out PrefabBase meshPrefab)
                && meshPrefab is RenderPrefab meshPrefab_rp
            )
            {
                RenderPrefab rp = meshPrefab_rp;

                if (skipVanilla && rp.isBuiltin)
                    return false;

                b1.Add(GetMeshStatData(rp, -1, out currentTris));
                // meshtest goes here
            }

            if (
                em.TryGetBuffer(currentMesh, true, out DynamicBuffer<LodMesh> lodMesh)
                && lodMesh.Length > 0
            )
            {
                setLocal.LODCount = lodMesh.Length;
                for (int j = 0; j < lodMesh.Length; j++)
                {
                    Entity currentLod = lodMesh[j].m_LodMesh;

                    if (
                        ps.TryGetPrefab(currentLod, out PrefabBase lodPrefab)
                        && lodPrefab is RenderPrefab lodPrefab_rp
                    )
                    {
                        RenderPrefab rp = lodPrefab_rp;
                        if (skipVanilla && rp.isBuiltin)
                            continue;

                        b1.Add(GetMeshStatData(rp, currentTris, out currentTris));
                    }
                }
            }
            setLocal.MeshStatData = b1.ToArray();
            setGlobal.Add(setLocal);
            return true;
        }

        public static MeshStatData GetMeshStatData(
            RenderPrefab rp,
            float prevTris,
            out int currentTris
        )
        {
            int trisCount = rp.indexCount / 3;
            float reduction = prevTris == -1 ? -1 : 100 * (prevTris - trisCount) / prevTris;
            currentTris = trisCount;
            float width = math.abs(rp.bounds.min.x - rp.bounds.max.x);
            float height = math.abs(rp.bounds.min.y - rp.bounds.max.y);
            float depth = math.abs(rp.bounds.min.z - rp.bounds.max.z);
            return new MeshStatData()
            {
                Name = rp.name,
                SurfaceArea = rp.surfaceArea,
                VertexCount = rp.vertexCount,
                TrisCount = trisCount,
                TrisDensity = trisCount / rp.surfaceArea,
                TrisReduction = reduction,
                Width = width,
                Height = height,
                Depth = depth,
                Volume = width * height * depth,
            };
        }

        public static void ClearDictionary()
        {
            meshStatDict.Clear();
        }
    }
}
