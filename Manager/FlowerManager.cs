using Newtonsoft.Json;
using RandomizerMod.Logging;
using FlowerRandomizer.Settings;
using FlowerRandomizer.Modules;
using ItemChanger;

namespace FlowerRandomizer.Manager
{
    internal static class FlowerManager
    {
        public static FlowerSettings Settings => FlowerRandomizer.Instance.GS;
        public static void Hook()
        {
            ConnectionMenu.Hook();
            LogicHandler.Hook();
            ItemHandler.Hook();
            LanguageManager.Hook();
            SettingsLog.AfterLogSettings += AddFileSettings;
        }

        private static void AddFileSettings(LogArguments args, System.IO.TextWriter tw)
        {
            // Log settings into the settings file
            tw.WriteLine("Flower Randomizer Settings:");
            using JsonTextWriter jtw = new(tw) { CloseOutput = false };
            RandomizerMod.RandomizerData.JsonUtil._js.Serialize(jtw, Settings);
            tw.WriteLine();

            // Add modules
            if (args.gs.LongLocationSettings.FlowerQuestPreview)
                ItemChangerMod.Modules.Add<FlowerPreviewModule>();
        }        
    }
}