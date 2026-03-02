using RandomizerCore.Json;
using RandomizerCore.Logic;
using RandomizerMod.RC;
using RandomizerMod.Settings;


namespace FlowerRandomizer.Manager
{
    public class LogicHandler
    {
        public static void Hook()
        {
            RCData.RuntimeLogicOverride.Subscribe(0f, ApplyLogic);
        }

        private static void ApplyLogic(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!FlowerManager.Settings.Enabled)
                return;
            
            lmb.DeserializeFile(LogicFileType.Locations, new JsonLogicFormat(), typeof(FlowerRandomizer).Assembly.GetManifestResourceStream($"FlowerRandomizer.Resources.Logic.locations.json"));
            lmb.DeserializeFile(LogicFileType.Waypoints, new JsonLogicFormat(), typeof(FlowerRandomizer).Assembly.GetManifestResourceStream($"FlowerRandomizer.Resources.Logic.waypoints.json"));
        }
    }
}