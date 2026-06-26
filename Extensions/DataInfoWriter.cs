using Colossal.UI.Binding;
using MeshStatsViewer.Types;

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

            writer.PropertyName(nameof(value.TrisCount));
            writer.Write(value.TrisCount);

            writer.PropertyName(nameof(value.VertexCount));
            writer.Write(value.VertexCount);

            writer.PropertyName(nameof(value.TrisReduction));
            writer.Write(value.TrisReduction);

            writer.PropertyName(nameof(value.TrisDensity));
            writer.Write(value.TrisDensity);

            writer.PropertyName(nameof(value.SurfaceArea));
            writer.Write(value.SurfaceArea);

            writer.PropertyName(nameof(value.Volume));
            writer.Write(value.Volume);

            writer.TypeEnd();
        }
    }

    public static class OptionsWriter
    {
        public static void Write(this IJsonWriter writer, Options value)
        {
            writer.TypeBegin(value.GetType().FullName);

            writer.PropertyName(nameof(value.ViewChooser));
            writer.Write((int)value.ViewChooser);

            writer.PropertyName(nameof(value.LOD1Threshold));
            writer.Write(value.LOD1Threshold);

            writer.PropertyName(nameof(value.LOD2Threshold));
            writer.Write(value.LOD2Threshold);

            writer.PropertyName(nameof(value.TrisThreshold));
            writer.Write(value.TrisThreshold);

            writer.PropertyName(nameof(value.VolumeThreshold));
            writer.Write(value.VolumeThreshold);

            writer.PropertyName(nameof(value.CheekyMode));
            writer.Write(value.CheekyMode);

            writer.TypeEnd();
        }
    }
}
