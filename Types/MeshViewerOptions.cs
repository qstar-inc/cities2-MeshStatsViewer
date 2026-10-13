using Colossal.UI.Binding;

namespace MeshStatsViewer.Types
{
    public class MeshViewerOptions : IJsonWritable
    {
        public bool EnableVanilla { get; set; } = OptionsDefaults.DefaultEnableVanilla;
        public View ViewChooser { get; set; } = OptionsDefaults.DefaultViewChooser;
        public int LOD1Threshold { get; set; } = OptionsDefaults.DefaultLOD1Threshold;
        public int LOD2Threshold { get; set; } = OptionsDefaults.DefaultLOD2Threshold;
        public int TrisThreshold { get; set; } = OptionsDefaults.DefaultTrisThreshold;
        public int VolumeThreshold { get; set; } = OptionsDefaults.DefaultVolumeThreshold;
        public bool ShowWHD { get; set; } = OptionsDefaults.DefaultShowWHD;
        public bool CheekyMode { get; set; } = OptionsDefaults.DefaultCheekyMode;

        public void Write(IJsonWriter writer)
        {
            writer.TypeBegin(GetType().FullName);

            writer.PropertyName(nameof(EnableVanilla));
            writer.Write(EnableVanilla);

            writer.PropertyName(nameof(ViewChooser));
            writer.Write((int)ViewChooser);

            writer.PropertyName(nameof(LOD1Threshold));
            writer.Write(LOD1Threshold);

            writer.PropertyName(nameof(LOD2Threshold));
            writer.Write(LOD2Threshold);

            writer.PropertyName(nameof(TrisThreshold));
            writer.Write(TrisThreshold);

            writer.PropertyName(nameof(VolumeThreshold));
            writer.Write(VolumeThreshold);

            writer.PropertyName(nameof(ShowWHD));
            writer.Write(ShowWHD);

            writer.PropertyName(nameof(CheekyMode));
            writer.Write(CheekyMode);

            writer.TypeEnd();
        }
    }
}
