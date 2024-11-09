using System;
using System.Collections.Generic;
using FlowerRandomizer.IC;
using FlowerRandomizer.Settings;
using ItemChanger;
using Modding;
using RandomizerMod.RC;

namespace FlowerRandomizer.Manager {
    internal static class ItemHandler
    {
        internal static void Hook()
        {
            DefineObjects();
            RequestBuilder.OnUpdate.Subscribe(0f, AddObjects);
            if (ModHooks.GetMod("LoreRandomizer") is Mod)
                RequestBuilder.OnUpdate.Subscribe(50f, RemoveLore);
        }

        public static void DefineObjects()
        {
            // Vanilla NPCs
            Finder.DefineCustomLocation(new VanillaFlowerLocation("Elderbug", "Town", "Elderbug", "Flower Accept", 0f, 0.2f));
            Finder.DefineCustomLocation(new VanillaFlowerLocation("Oro", "Room_nailmaster_03", "NM Oro NPC", "Stand", 2.55f, 0.15f));
            Finder.DefineCustomLocation(new VanillaFlowerLocation("Godseeker", "GG_Waterways", "Godseeker Awake", "Flower Greet", -1.9f, -1f));
            Finder.DefineCustomLocation(new VanillaFlowerLocation("Emilitia", "Ruins_House_03", "Emilitia NPC", "Flower", 0f, -0.5f));
            Finder.DefineCustomLocation(new VanillaFlowerLocation("White_Lady", "Room_Queen", "Queen", "Flower", -0.1f, 0.25f));

            // Custom NPCs
            Finder.DefineCustomLocation(new CustomDialogueFlowerLocation("Mato", "Room_nailmaster", "NM Mato NPC", "MATO_FLOWER", 0.05f, 0.4f));
            Finder.DefineCustomLocation(new CustomDialogueFlowerLocation("Sheo", "Room_nailmaster_02", "NM Sheo NPC", "SHEO_FLOWER", 0.05f, 0.3f));
            Finder.DefineCustomLocation(new CustomDialogueFlowerLocation("Marissa", "Ruins_Bathhouse", "Ghost NPC", "MARISSA_FLOWER", 0.2f, -0.1f));
            Finder.DefineCustomLocation(new CustomDialogueFlowerLocation("Midwife", "Deepnest_41", "NPC", "MIDWIFE_FLOWER", 0.4f, -1.3f));
            Finder.DefineCustomLocation(new DreamDialogueFlowerLocation("Isma", "Waterways_13", "ISMA_FLOWER", 0.7f, -0.5f));
            Finder.DefineCustomLocation(new DreamDialogueFlowerLocation("Radiance", "Mines_34", "RADIANCE_FLOWER", -1.3f, 0.5f));
            Finder.DefineCustomLocation(new DreamDialogueFlowerLocation("Pale_King", "White_Palace_09", "KING_FLOWER", 0.2f, -0.9f));
            Finder.DefineCustomLocation(new AbstractFlowerLocation("Pain", "White_Palace_20", "End Scene", "Conversation Control", "Hero Anim", "FINISHED", "Disappear Scne", 0.5f, -0.9f));
        }

        public static void AddObjects(RequestBuilder rb)
        {
            FlowerSettings settings = FlowerManager.Settings;
            if (!settings.Enabled)
                return;

            List<string> questList = [];
            if (settings.VanillaNPCs.Elderbug)
                questList.Add("Flower_Quest-Elderbug");
            if (settings.VanillaNPCs.Oro)
                questList.Add("Flower_Quest-Oro");
            if (settings.VanillaNPCs.Godseeker)
                questList.Add("Flower_Quest-Godseeker");
            if (settings.VanillaNPCs.Emilitia)
                questList.Add("Flower_Quest-Emilitia");
            if (settings.VanillaNPCs.WhiteLady)
                questList.Add("Flower_Quest-White_Lady");
            if (settings.CustomNPCs.Mato)
                questList.Add("Flower_Quest-Mato");
            if (settings.CustomNPCs.Sheo)
                questList.Add("Flower_Quest-Sheo");
            if (settings.CustomNPCs.Marissa)
                questList.Add("Flower_Quest-Marissa");
            if (settings.CustomNPCs.Midwife)
                questList.Add("Flower_Quest-Midwife");
            if (settings.CustomNPCs.Isma)
                questList.Add("Flower_Quest-Isma");
            if (settings.CustomNPCs.Radiance)
                questList.Add("Flower_Quest-Radiance");
            if (settings.CustomNPCs.PaleKing)
                questList.Add("Flower_Quest-Pale_King");
            if (settings.CustomNPCs.Pain)
                questList.Add("Flower_Quest-Pain");
            
            if (settings.RandomizeQuestsAtStart)
            {
                int questCount = Math.Min(settings.MaximumQuests, settings.VanillaNPCs.ActiveNPCs() + settings.CustomNPCs.ActiveNPCs());
                questCount -= settings.MinimumQuests;
                questCount = rb.rng.Next(questCount) + settings.MinimumQuests;

                for (int i = 0; i < questCount; i++)
                {
                    int index = rb.rng.Next(questList.Count);
                    rb.AddLocationByName(questList[index]);
                    questList.Remove(questList[index]);
                }
            }
            else
            {
                foreach (string quest in questList)
                {
                    rb.AddLocationByName(quest);
                }
            }
        }
        private static void RemoveLore(RequestBuilder rb)
        {
            // Remove these locations from LoreRando if flower quests are active
            if (rb.TryGetLocationRequest("Flower_Quest-Isma", out _))
                rb.RemoveLocationByName("Isma_Dream");
            if (rb.TryGetLocationRequest("Flower_Quest-Radiance", out _))
                rb.RemoveLocationByName("Radiance_Statue_Dream");
        }
    }
}