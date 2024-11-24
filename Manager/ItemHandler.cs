using System;
using System.Collections.Generic;
using FlowerRandomizer.IC;
using FlowerRandomizer.Settings;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Tags;
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

        public static InteropTag FlowerTag(string sceneName, float x, float y)
        {
            InteropTag tag = new();
            string mapSceneName = sceneName;

            Dictionary<string, string> sceneOverride = new Dictionary<string, string>
            {
                { SceneNames.Room_nailmaster, SceneNames.Cliffs_02 },
                { SceneNames.Room_nailmaster_02, SceneNames.Fungus1_15 },
                { SceneNames.Room_nailmaster_03, SceneNames.Deepnest_East_06 },
                { SceneNames.Room_Queen, SceneNames.Fungus3_48 },
                { SceneNames.Ruins_House_03, SceneNames.Ruins2_04 },
                { SceneNames.White_Palace_09, SceneNames.Abyss_05 },
                { SceneNames.White_Palace_20, SceneNames.Abyss_05 }
            };

            if (sceneOverride.ContainsKey(sceneName))
                mapSceneName = sceneOverride[sceneName];

            tag.Properties["ModSource"] = "FlowerRandomizer";
            tag.Properties["PoolGroup"] = "Flower Quests";
            tag.Properties["PinSprite"] = new FlowerSprite("Flower");
            tag.Properties["MapLocations"] = new (string, float, float)[] { (mapSceneName, x, y) };
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        public static void DefineObjects()
        {
            // Vanilla NPCs
            Finder.DefineCustomLocation(new VanillaFlowerLocation("Elderbug", "Town", "Elderbug", "Flower Accept")
            {
                tags = [FlowerTag("Town", 0f, 0.2f)]
            });
            Finder.DefineCustomLocation(new VanillaFlowerLocation("Oro", "Room_nailmaster_03", "NM Oro NPC", "Stand")
            {
                tags = [FlowerTag("Room_nailmaster_03", 2.55f, 0.15f)]
            });
            Finder.DefineCustomLocation(new VanillaFlowerLocation("Godseeker", "GG_Waterways", "Godseeker Awake", "Flower Greet")
            {
                tags = [FlowerTag("GG_Waterways", -1.9f, -1f)]
            });
            Finder.DefineCustomLocation(new VanillaFlowerLocation("Emilitia", "Ruins_House_03", "Emilitia NPC", "Flower")
            {
                tags = [FlowerTag("Ruins_House_03", 0f, -0.5f)]
            });
            Finder.DefineCustomLocation(new VanillaFlowerLocation("White_Lady", "Room_Queen", "Queen", "Flower")
            {
                tags = [FlowerTag("Room_Queen", -0.1f, 0.25f)]
            });

            // Custom NPCs
            Finder.DefineCustomLocation(new CustomDialogueFlowerLocation("Mato", "Room_nailmaster", "NM Mato NPC", "MATO_FLOWER")
            {
                tags = [FlowerTag("Room_nailmaster", 0.05f, 0.4f)]
            });
            Finder.DefineCustomLocation(new CustomDialogueFlowerLocation("Sheo", "Room_nailmaster_02", "NM Sheo NPC", "SHEO_FLOWER")
            {
                tags = [FlowerTag("Room_nailmaster_02", 0.05f, 0.3f)]
            });
            Finder.DefineCustomLocation(new CustomDialogueFlowerLocation("Marissa", "Ruins_Bathhouse", "Ghost NPC", "MARISSA_FLOWER")
            {
                tags = [FlowerTag("Ruins_Bathhouse", 0.2f, -0.1f)]
            });
            Finder.DefineCustomLocation(new CustomDialogueFlowerLocation("Midwife", "Deepnest_41", "NPC", "MIDWIFE_FLOWER")
            {
                tags = [FlowerTag("Deepnest_41", 0.4f, -1.3f)]
            });
            Finder.DefineCustomLocation(new DreamDialogueFlowerLocation("Isma", "Waterways_13", "ISMA_FLOWER")
            {
                tags = [FlowerTag("Waterways_13", 0.7f, -0.5f)]
            });
            Finder.DefineCustomLocation(new DreamDialogueFlowerLocation("Radiance", "Mines_34", "RADIANCE_FLOWER")
            {
                tags = [FlowerTag("Mines_34", -1.3f, 0.5f)]
            });
            Finder.DefineCustomLocation(new DreamDialogueFlowerLocation("Pale_King", "White_Palace_09", "KING_FLOWER")
            {
                tags = [FlowerTag("White_Palace_09", 0.2f, -0.9f)]
            });
            Finder.DefineCustomLocation(new PainFlowerLocation("Pain", "White_Palace_20", "End Scene", "Conversation Control", "Hero Anim", "FINISHED", "Disappear Scne")
            {
                tags = [FlowerTag("White_Palace_20", 0.5f, -0.9f)]
            });
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