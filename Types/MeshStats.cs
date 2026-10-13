using Colossal.UI.Binding;

namespace MeshStatsViewer.Types
{
    public class MeshStats : IJsonWritable
    {
        public int LODCount { get; set; } = 0;
        public MeshStatData[] MeshStatData { get; set; } = new MeshStatData[0];

        public void Write(IJsonWriter writer)
        {
            writer.TypeBegin(GetType().FullName);

            writer.PropertyName(nameof(LODCount));
            writer.Write(LODCount);

            writer.PropertyName(nameof(MeshStatData));
            writer.ArrayBegin(MeshStatData.Length);
            foreach (var item in MeshStatData)
                item.Write(writer);
            writer.ArrayEnd();

            writer.TypeEnd();
        }
    }

    public class MeshStatData : IJsonWritable
    {
        public string Name { get; set; } = string.Empty;
        public int TrisCount { get; set; } = 0;
        public int VertexCount { get; set; } = 0;
        public float TrisReduction { get; set; } = 0;
        public float TrisDensity { get; set; } = 0;
        public float SurfaceArea { get; set; } = 0;
        public float Width { get; set; } = 0;
        public float Height { get; set; } = 0;
        public float Depth { get; set; } = 0;
        public float Volume { get; set; } = 0;

        public void Write(IJsonWriter writer)
        {
            writer.TypeBegin(GetType().FullName);

            writer.PropertyName(nameof(Name));
            writer.Write(Name);
            writer.PropertyName(nameof(TrisCount));
            writer.Write(TrisCount);
            writer.PropertyName(nameof(VertexCount));
            writer.Write(VertexCount);
            writer.PropertyName(nameof(TrisReduction));
            writer.Write(TrisReduction);
            writer.PropertyName(nameof(TrisDensity));
            writer.Write(TrisDensity);
            writer.PropertyName(nameof(SurfaceArea));
            writer.Write(SurfaceArea);
            writer.PropertyName(nameof(Width));
            writer.Write(Width);
            writer.PropertyName(nameof(Height));
            writer.Write(Height);
            writer.PropertyName(nameof(Depth));
            writer.Write(Depth);
            writer.PropertyName(nameof(Volume));
            writer.Write(Volume);

            writer.TypeEnd();
        }
    }

    public sealed class MeshStatExport
    {
        public float SurfaceArea;
        public int VertexCount;
        public int TrisCount;
        public float TrisReduction;
        public float TrisDensity;
        public float Volume;
    }
}
