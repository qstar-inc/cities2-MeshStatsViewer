using System.Collections.Generic;
using System.IO;
using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using MeshStatsViewer.Systems;
using MeshStatsViewer.Types;
using StarQ.Shared.Extensions;
using StarQ.Shared.Generators;

namespace MeshStatsViewer
{
    [GenerateModInfo]
    public partial class Mod : IMod
    {
        public void OnLoad(UpdateSystem updateSystem)
        {
            LogHelper.Init(Id, log);
            LocaleHelper.Init(Id, Name, GetReplacements, AddLocale);

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            AssetDatabase.global.LoadSettings(Id, m_Setting, new Setting(this));

            Directory.CreateDirectory(DataDir);

            WorldHelper.GetSystem<ReportSystem>();

            updateSystem.UpdateAfter<SIPSystem>(SystemUpdatePhase.UIUpdate);
            updateSystem.UpdateAfter<EditorSelectedSystem>(SystemUpdatePhase.UIUpdate);
        }

        public void OnDispose()
        {
            LocaleHelper.Dispose();
            m_Setting?.UnregisterInOptionsUI();
            m_Setting = null;
        }

        public static Dictionary<string, string> GetReplacements()
        {
            return new() { };
        }

        public static void AddLocale()
        {
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsEnumLocaleId(nameof(AssetSource), nameof(AssetSource.Local)),
                LocaleHelper.Translate("Menu.CLOUD_TARGET[Local]")
            );
            LocaleHelper.AddLocalization(
                LocaleHelper.GetOptionsEnumLocaleId(
                    nameof(AssetSource),
                    nameof(AssetSource.Subscribed)
                ),
                LocaleHelper.Translate("Editor.ASSET_CATEGORY_TITLE[Subscibed]")
            );
        }
    }
}
