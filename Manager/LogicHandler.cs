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
            
            // Godseeker default waypoints
            lmb.DeserializeFile(LogicFileType.Waypoints, new JsonLogicFormat(), typeof(FlowerRandomizer).Assembly.GetManifestResourceStream($"FlowerRandomizer.Resources.Logic.waypoints.json"));

            // Vanilla NPCs
            lmb.AddLogicDef(new ("Flower_Quest-Elderbug", "Town + LISTEN?TRUE + NOFLOWER=FALSE"));
            lmb.AddLogicDef(new ("Flower_Quest-Oro", "Room_nailmaster_03[left1] + Can_Visit_Lemm + LISTEN?TRUE + NOFLOWER=FALSE"));
            lmb.AddLogicDef(new ("Flower_Quest-Godseeker", "GG_Waterways + GODTUNERUNLOCK + LISTEN?TRUE + Defeated_Pantheon_1 + Defeated_Pantheon_2 + Defeated_Pantheon_3 + NOFLOWER=FALSE"));
            lmb.AddLogicDef(new ("Flower_Quest-Emilitia", "(Ruins_House_03[left1] | (ANYCLAW + Ruins_House_03[left2])) + LISTEN?TRUE + NOFLOWER=FALSE"));
            lmb.AddLogicDef(new ("Flower_Quest-White_Lady", "Room_Queen[left1] + LISTEN?TRUE + NOFLOWER=FALSE"));

            // Custom NPCs
            lmb.AddLogicDef(new ("Flower_Quest-Mato", "Room_nailmaster[left1] + LISTEN?TRUE + NOFLOWER=FALSE"));
            lmb.AddLogicDef(new ("Flower_Quest-Sheo", "Room_nailmaster_02[left1] + LISTEN?TRUE + NOFLOWER=FALSE"));
            lmb.AddLogicDef(new ("Flower_Quest-Marissa", "(Ruins_Bathhouse[door1] | Ruins_Bathhouse[right1] + Opened_Pleasure_House_Wall) + DREAMNAIL + NOFLOWER=FALSE"));
            lmb.AddLogicDef(new ("Flower_Quest-Midwife", "Deepnest_41[left2] + LISTEN?TRUE + Wall-Midwife?TRUE + (LANTERN | NOLANTERN?FALSE | DARKROOMS) + NOFLOWER=FALSE"));
            lmb.AddLogicDef(new ("Flower_Quest-Isma", "Waterways_13 + DREAMNAIL + NOFLOWER=FALSE"));
            lmb.AddLogicDef(new ("Flower_Quest-Radiance", "*Pale_Ore-Crystal_Peak + DREAMNAIL + NOFLOWER=FALSE"));
            lmb.AddLogicDef(new ("Flower_Quest-Pale_King", "*King_Fragment + NOFLOWER=FALSE"));
            lmb.AddLogicDef(new ("Flower_Quest-Pain", "White_Palace_20[bot1] + Completed_Path_of_Pain + NOFLOWER=FALSE"));
        }
    }
}