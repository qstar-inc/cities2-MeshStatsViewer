using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Colossal.IO.AssetDatabase;
using Colossal.PSI.Environment;
using Game;
using Game.Prefabs;
using Game.UI.Localization;
using MeshStatsViewer.Types;
using Newtonsoft.Json;
using StarQ.Shared.Extensions;
using Unity.Collections;
using Unity.Entities;

namespace MeshStatsViewer.Systems
{
    public partial class ReportSystem : GameSystemBase
    {
        private PrefabSystem prefabSystem;

        private readonly SortedDictionary<
            string,
            SortedDictionary<string, MeshStatExport>
        > meshStatsJson = new();

        private readonly SortedDictionary<
            string,
            SortedDictionary<string, int[]>
        > meshStatsSimplifiedText = new();

        private readonly StringBuilder meshStatsCsv = new();

        protected override void OnCreate()
        {
            base.OnCreate();
            prefabSystem = WorldHelper.PrefabSystem;
            Enabled = false;
        }

        protected override void OnUpdate() { }

        private static string MeshDataExport(string fileText, string extension)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                fileText = fileText.Replace(c, '_');

            return Path.Combine(Mod.DataDir, $"MSVExport-{fileText}.{extension}");
        }

        private static string TrimMeshSuffix(string name) =>
            name.EndsWith(" Mesh", StringComparison.Ordinal) ? name[..^" Mesh".Length] : name;

        public void GetAllRenderPrefabData()
        {
            AssetSource assetType = Mod.m_Setting.AssetSourceSelection;
            string exportFormatText = Mod.m_Setting.ExportFormatSelection;
            if (!Enum.TryParse(exportFormatText, out ExportFormat exportFormat))
            {
                LogHelper.SendLog($"Failed to parse {exportFormatText}", LogLevel.Error);
                return;
            }

            meshStatsJson.Clear();
            meshStatsSimplifiedText.Clear();
            meshStatsCsv.Clear();

            if (exportFormat == ExportFormat.CSV && assetType != AssetSource.Subscribed)
            {
                DialogHelper.ShowMessageDialog(
                    Mod.Name,
                    LocalizedString.Value("CSV export is only available for Subscribed assets"),
                    LocalizedString.Id("Common.OK"),
                    null
                );
                return;
            }

            string combinedName = $"Combined_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}";

            var entities = SystemAPI
                .QueryBuilder()
                .WithAll<SubMesh>()
                .Build()
                .ToEntityArray(Allocator.Temp);

            int count = 0;
            foreach (Entity item in entities)
            {
                prefabSystem.TryGetPrefab(item, out PrefabBase prefabBase);

                if (prefabBase == null)
                    continue;

                if (prefabBase.asset == null)
                    continue;

                if (
                    assetType == AssetSource.Subscribed
                    && prefabBase.asset.database is not AssetDatabase<ParadoxMods>
                )
                    continue;

                if (
                    assetType == AssetSource.Local
                    && prefabBase.asset.database is not AssetDatabase<User>
                )
                    continue;

                if (prefabBase.asset.subPath == null)
                    continue;

                string fileText = "";
                switch (assetType)
                {
                    case AssetSource.Local:
                        fileText = prefabBase
                            .asset.subPath.Replace("ImportedData/", "")
                            .Replace("/", "_");
                        break;
                    case AssetSource.Subscribed:
                        fileText = prefabBase.asset.subPath.Split("/")[1];
                        break;
                }

                if (
                    CheckMeshSystem.CheckMesh(
                        EntityManager,
                        WorldHelper.PrefabSystem,
                        item,
                        true,
                        out string prefabName,
                        out MeshStats[] meshDataInfo
                    )
                )
                {
                    switch (exportFormat)
                    {
                        case ExportFormat.JSON:
                            if (!meshStatsJson.TryGetValue(fileText, out var meshes))
                            {
                                meshes = new SortedDictionary<string, MeshStatExport>();
                                meshStatsJson[fileText] = meshes;
                            }

                            foreach (MeshStats stats in meshDataInfo)
                            {
                                foreach (var data in stats.MeshStatData)
                                {
                                    meshes.TryAdd(
                                        data.Name,
                                        new MeshStatExport
                                        {
                                            SurfaceArea = data.SurfaceArea,
                                            VertexCount = data.VertexCount,
                                            TrisCount = data.TrisCount,
                                            TrisReduction = data.TrisReduction,
                                            TrisDensity = data.TrisDensity,
                                            Volume = data.Volume,
                                        }
                                    );
                                }
                            }
                            break;

                        case ExportFormat.TXT:
                            if (!meshStatsSimplifiedText.TryGetValue(fileText, out var assets))
                            {
                                assets = new SortedDictionary<string, int[]>();
                                meshStatsSimplifiedText[fileText] = assets;
                            }

                            foreach (MeshStats stats in meshDataInfo)
                            {
                                if (stats.MeshStatData.Length == 0)
                                    continue;

                                string assetName = TrimMeshSuffix(stats.MeshStatData[0].Name);

                                assets.TryAdd(
                                    assetName,
                                    stats.MeshStatData.Select(m => m.TrisCount).ToArray()
                                );
                            }
                            break;

                        case ExportFormat.CSV:
                            foreach (MeshStats stats in meshDataInfo)
                            {
                                for (int lod = 0; lod < stats.MeshStatData.Length; lod++)
                                {
                                    MeshStatData d = stats.MeshStatData[lod];

                                    meshStatsCsv.AppendLine(
                                        string.Join(
                                            ",",
                                            CsvEscape(fileText),
                                            CsvEscape(prefabName),
                                            lod,
                                            CsvEscape(d.Name),
                                            Num(d.SurfaceArea),
                                            Num(d.VertexCount),
                                            Num(d.TrisCount),
                                            Num(d.TrisReduction),
                                            Num(d.TrisDensity),
                                            Num(d.Volume)
                                        )
                                    );
                                }
                            }
                            break;
                    }
                }
            }

            switch (exportFormat)
            {
                case ExportFormat.JSON:
                    foreach (var (fileText, meshes) in meshStatsJson)
                    {
                        count++;
                        File.WriteAllText(
                            MeshDataExport(fileText, "json"),
                            JsonConvert.SerializeObject(meshes, Formatting.Indented)
                        );
                    }
                    break;

                case ExportFormat.TXT:
                    foreach (var (fileText, assets) in meshStatsSimplifiedText)
                    {
                        count++;
                        StringBuilder sb = new();

                        foreach (var (assetName, trisPerLod) in assets)
                        {
                            var lods = trisPerLod.Select(
                                (tris, lod) =>
                                {
                                    string lodText = $"LOD{lod}";
                                    if (lod == 0)
                                        lodText = "Main";
                                    return $"**{lodText}**: {tris.ToString("N0", CultureInfo.InvariantCulture)} triangles";
                                }
                            );

                            sb.AppendLine($"* *{assetName}*: {string.Join(", ", lods)}");
                        }

                        File.WriteAllText(MeshDataExport(fileText, "txt"), sb.ToString());
                    }
                    break;

                case ExportFormat.CSV:
                    count = 1;
                    File.WriteAllText(
                        MeshDataExport(combinedName, "csv"),
                        "mod_id,PrefabName,LODIndex,RenderPrefabName,SurfaceArea,VertexCount,TrisCount,TrisReduction,TrisDensity,Volume"
                            + Environment.NewLine
                            + meshStatsCsv
                    );
                    break;
            }

            string endText =
                $"{count} mesh data export files saved in {Mod.DataDir.Replace("\\", "/").Replace(EnvPath.kUserDataPath.Replace("\\", "/"), "")}";
            LogHelper.SendLog(endText);
            DialogHelper.ShowMessageDialog(
                Mod.Name,
                LocalizedString.Value(endText),
                LocalizedString.Id("Common.OK"),
                null
            );
            CheckMeshSystem.ClearDictionary();
        }

        private static string Num(IFormattable value) =>
            value.ToString(null, CultureInfo.InvariantCulture);

        private static string CsvEscape(string value) =>
            value.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0
                ? $"\"{value.Replace("\"", "\"\"")}\""
                : value;
    }
}
