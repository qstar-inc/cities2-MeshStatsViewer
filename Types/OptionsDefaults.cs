namespace MeshStatsViewer.Types
{
    public static class OptionsDefaults
    {
        public static bool DefaultEnableVanilla = false;
        public static View DefaultViewChooser = View.Tabular;
        public static int DefaultLOD1Threshold = 50;
        public static int DefaultLOD2Threshold = 80;
        public static int DefaultTrisThreshold = 5000;
        public static int DefaultVolumeThreshold = 1;
        public static bool DefaultShowWHD = false;
        public static bool DefaultCheekyMode = true;

        public static MeshViewerOptions GetCurrent() =>
            new()
            {
                EnableVanilla = Mod.m_Setting.EnableVanilla,
                ViewChooser = Mod.m_Setting.ViewChooser,
                LOD1Threshold = Mod.m_Setting.LOD1Threshold,
                LOD2Threshold = Mod.m_Setting.LOD2Threshold,
                TrisThreshold = Mod.m_Setting.TrisThreshold,
                ShowWHD = Mod.m_Setting.ShowWHD,
                VolumeThreshold = Mod.m_Setting.VolumeThreshold,
                CheekyMode = Mod.m_Setting.CheekyMode,
            };
    }
}
