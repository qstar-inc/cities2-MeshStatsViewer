using System.Collections.Generic;
using Colossal.IO.AssetDatabase;
using Game;
using Game.Modding;
using MeshStatsViewer.Systems;
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
            LocaleHelper.Init(Id, Name, GetReplacements);

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            AssetDatabase.global.LoadSettings(Id, m_Setting, new Setting(this));

            updateSystem.UpdateAfter<SIP_MSV>(SystemUpdatePhase.UIUpdate);
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
    }
}
