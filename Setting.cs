using Game.Modding;
using Game.Settings;
using Game.UI;
using MeshStatsViewer.Types;
using StarQ.Shared.Generators;

namespace MeshStatsViewer
{
    [GenerateSettingCommonAttribute]
    public partial class Setting : ModSetting
    {
        public override void SetDefaults()
        {
            EnableVanilla = OptionsDefaults.DefaultEnableVanilla;
            ViewChooser = OptionsDefaults.DefaultViewChooser;
            LOD1Threshold = OptionsDefaults.DefaultLOD1Threshold;
            LOD2Threshold = OptionsDefaults.DefaultLOD2Threshold;
            TrisThreshold = OptionsDefaults.DefaultTrisThreshold;
            VolumeThreshold = OptionsDefaults.DefaultVolumeThreshold;
            CheekyMode = OptionsDefaults.DefaultCheekyMode;
        }

        [SettingsUISection(GeneralTab, GeneralGroup)]
        public bool EnableVanilla { get; set; } = false;

        [SettingsUISection(GeneralTab, GeneralGroup)]
        public View ViewChooser { get; set; } = View.Tabular;

        [SettingsUISlider(max = 60, min = 30, step = 10, unit = Unit.kPercentage)]
        [SettingsUISection(GeneralTab, GeneralGroup)]
        public int LOD1Threshold { get; set; } = 50;

        [SettingsUISlider(max = 100, min = 50, step = 10, unit = Unit.kPercentage)]
        [SettingsUISection(GeneralTab, GeneralGroup)]
        public int LOD2Threshold { get; set; } = 80;

        [SettingsUISlider(max = 10000, min = 1000, step = 100, unit = Unit.kInteger)]
        [SettingsUISection(GeneralTab, GeneralGroup)]
        public int TrisThreshold { get; set; } = 1000;

        [SettingsUISlider(max = 100, min = 0, step = 1, unit = Unit.kVolume)]
        [SettingsUISection(GeneralTab, GeneralGroup)]
        public int VolumeThreshold { get; set; } = 1;

        [SettingsUISection(GeneralTab, GeneralGroup)]
        public bool CheekyMode { get; set; } = false;
    }
}
