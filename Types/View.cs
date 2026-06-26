using Colossal.Json;
using Game.Settings;

namespace MeshStatsViewer.Types
{
    public enum View
    {
        Tabular,
        Detailed,
        Simplified,
    }

    public static class OptionsDefaults
    {
        public static View DefaultViewChooser = View.Tabular;
        public static int DefaultLOD1Threshold = 50;
        public static int DefaultLOD2Threshold = 80;
        public static int DefaultTrisThreshold = 5000;
        public static int DefaultVolumeThreshold = 1;
        public static bool DefaultCheekyMode = false;
    }

    public class Options
    {
        public View ViewChooser { get; set; } = OptionsDefaults.DefaultViewChooser;
        public int LOD1Threshold { get; set; } = OptionsDefaults.DefaultLOD1Threshold;
        public int LOD2Threshold { get; set; } = OptionsDefaults.DefaultLOD2Threshold;
        public int TrisThreshold { get; set; } = OptionsDefaults.DefaultTrisThreshold;
        public int VolumeThreshold { get; set; } = OptionsDefaults.DefaultVolumeThreshold;
        public bool CheekyMode { get; set; } = OptionsDefaults.DefaultCheekyMode;
    }
}
