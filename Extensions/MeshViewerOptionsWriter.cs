using Colossal.UI.Binding;
using MeshStatsViewer.Types;

namespace MeshStatsViewer.Extensions
{
    public static class MeshViewerOptionsWriter
    {
        public static void Write(this IJsonWriter writer, MeshViewerOptions value)
        {
            writer.TypeBegin(value.GetType().FullName);

            writer.PropertyName(nameof(value.EnableVanilla));
            writer.Write(value.EnableVanilla);

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

            writer.PropertyName(nameof(value.ShowWHD));
            writer.Write(value.ShowWHD);

            writer.PropertyName(nameof(value.CheekyMode));
            writer.Write(value.CheekyMode);

            writer.TypeEnd();
        }
    }
}
