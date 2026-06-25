using Colossal.UI.Binding;
using MeshStatsViewer.Variables;

namespace MeshStatsViewer.Extensions
{
    public static class MeshStatsWriter
    {
        public static void Write(this IJsonWriter writer, MeshStats[] array)
        {
            writer.ArrayBegin(array.Length);
            foreach (var item in array)
                Write(writer, item);
            writer.ArrayEnd();
        }

        public static void Write(this IJsonWriter writer, MeshStats value)
        {
            writer.TypeBegin(value.GetType().FullName);

            writer.PropertyName(nameof(value.LODCount));
            writer.Write(value.LODCount);

            writer.PropertyName(nameof(value.MeshStatData));
            writer.Write(value.MeshStatData);

            writer.TypeEnd();
        }

        public static void Write(this IJsonWriter writer, MeshStatData[] array)
        {
            writer.ArrayBegin(array.Length);
            foreach (var item in array)
                Write(writer, item);
            writer.ArrayEnd();
        }

        public static void Write(this IJsonWriter writer, MeshStatData value)
        {
            writer.TypeBegin(value.GetType().FullName);

            writer.PropertyName(nameof(value.Name));
            writer.Write(value.Name);

            writer.PropertyName(nameof(value.SurfaceArea));
            writer.Write(value.SurfaceArea);

            writer.PropertyName(nameof(value.IndexCount));
            writer.Write(value.IndexCount);

            writer.PropertyName(nameof(value.VertexCount));
            writer.Write(value.VertexCount);

            writer.PropertyName(nameof(value.TrisCount));
            writer.Write(value.TrisCount);

            writer.PropertyName(nameof(value.TrisDensity));
            writer.Write(value.TrisDensity);

            writer.PropertyName(nameof(value.VertexDensity));
            writer.Write(value.VertexDensity);

            writer.PropertyName(nameof(value.Width));
            writer.Write(value.Width);

            writer.PropertyName(nameof(value.Height));
            writer.Write(value.Height);

            writer.PropertyName(nameof(value.Depth));
            writer.Write(value.Depth);

            writer.PropertyName(nameof(value.Volume));
            writer.Write(value.Volume);

            writer.TypeEnd();
        }
    }
}
