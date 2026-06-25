using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Game.Modding;
using Game.Settings;
using Game.UI;
using StarQ.Shared.Extensions;

namespace MeshStatsViewer
{
    [FileLocation("ModsSettings\\StarQ\\" + nameof(MeshStatsViewer))]
    [SettingsUITabOrder(GeneralTab, AboutTab, LogTab)]
    public class Setting : ModSetting
    {
        public Setting(IMod mod)
            : base(mod) => SetDefaults();

        public const string GeneralTab = "GeneralTab";
        public const string GeneralGroup = "GeneralGroup";

        public const string AboutTab = "AboutTab";
        public const string InfoGroup = "InfoGroup";

        public const string LogTab = "LogTab";

        public override void SetDefaults()
        {
            DetailedView = true;
            LOD1Threshold = 50;
            LOD2Threshold = 80;
            TrisThreshold = 5000;
            VolumeThreshold = 1;
        }

        [SettingsUISection(GeneralTab, GeneralGroup)]
        public bool DetailedView { get; set; } = true;

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

        [SettingsUISection(AboutTab, InfoGroup)]
        public string NameText => Mod.Name;

        [SettingsUISection(AboutTab, InfoGroup)]
        public string VersionText => VariableHelper.AddDevSuffix(Mod.Version);

        [SettingsUISection(AboutTab, InfoGroup)]
        public string AuthorText => VariableHelper.StarQ;

        [SettingsUIButton]
        [SettingsUIButtonGroup("Social")]
        [SettingsUISection(AboutTab, InfoGroup)]
        public bool BMaCLink
        {
            set => VariableHelper.OpenBMAC();
        }

        //[SettingsUIButton]
        //[SettingsUIButtonGroup("Social")]
        //[SettingsUISection(AboutTab, InfoGroup)]
        //public bool Discord
        //{
        //    set => VariableHelper.OpenDiscord(XXXX);
        //}

        [SettingsUIMultilineText]
        [SettingsUIDisplayName(typeof(LogHelper), nameof(LogHelper.LogText))]
        [SettingsUISection(LogTab, "")]
        public string LogText => string.Empty;

        [Exclude]
        [SettingsUIHidden]
        public bool IsLogMissing
        {
            get => VariableHelper.CheckLog(Mod.Id);
        }

        [SettingsUIButton]
        [SettingsUIDisableByCondition(typeof(Setting), nameof(IsLogMissing))]
        [SettingsUISection(LogTab, "")]
        public bool OpenLog
        {
            set => VariableHelper.OpenLog(Mod.Id);
        }
    }
}
