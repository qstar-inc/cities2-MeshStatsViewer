namespace MeshStatsViewer.Types
{
    public class MeshViewerOptions
    {
        public bool EnableVanilla { get; set; } = OptionsDefaults.DefaultEnableVanilla;
        public View ViewChooser { get; set; } = OptionsDefaults.DefaultViewChooser;
        public int LOD1Threshold { get; set; } = OptionsDefaults.DefaultLOD1Threshold;
        public int LOD2Threshold { get; set; } = OptionsDefaults.DefaultLOD2Threshold;
        public int TrisThreshold { get; set; } = OptionsDefaults.DefaultTrisThreshold;
        public int VolumeThreshold { get; set; } = OptionsDefaults.DefaultVolumeThreshold;
        public bool ShowWHD { get; set; } = OptionsDefaults.DefaultShowWHD;
        public bool CheekyMode { get; set; } = OptionsDefaults.DefaultCheekyMode;
    }
}
