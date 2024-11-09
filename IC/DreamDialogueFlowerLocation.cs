using System.Collections.Generic;
using FlowerRandomizer.Manager;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;
using ItemChanger.Util;
using Modding;
using Satchel;

namespace FlowerRandomizer.IC
{
    public class DreamDialogueFlowerLocation : AutoLocation
    {
        public string dialogueKey;
        public DreamDialogueFlowerLocation(string _name, string _sceneName, string _dialogueKey, float x, float y)
        {
            name = $"Flower_Quest-{_name}";
            sceneName = _sceneName;
            dialogueKey = _dialogueKey;
            tags = [FlowerLocationTag(x, y)];
        }

        private InteropTag FlowerLocationTag(float x, float y)
        {
            InteropTag tag = new();
            string mapSceneName = sceneName;
            Dictionary<string, string> sceneOverride = [];
            sceneOverride.Add(SceneNames.White_Palace_09, SceneNames.Abyss_05);

            if (sceneOverride.ContainsKey(sceneName))
                mapSceneName = sceneOverride[sceneName];

            tag.Properties["ModSource"] = "FlowerRandomizer";
            tag.Properties["PoolGroup"] = $"Flower Quests";
            tag.Properties["PinSprite"] = new FlowerSprite("Flower");
            tag.Properties["VanillaItem"] = name;
            tag.Properties["MapLocations"] = new (string, float, float)[] {(mapSceneName, x, y)};
            tag.Message = "RandoSupplementalMetadata";

            return tag;
        }
        protected override void OnLoad()
        {
            ModHooks.LanguageGetHook += CustomDialogue;
            Events.AddFsmEdit(sceneName, new ("Dream Dialogue", "npc_dream_dialogue"), GiveItem);
        }

        protected override void OnUnload()
        {
            ModHooks.LanguageGetHook -= CustomDialogue;
            Events.RemoveFsmEdit(sceneName, new ("Dream Dialogue", "npc_dream_dialogue"), GiveItem);
        }

        private string CustomDialogue(string key, string sheetTitle, string orig)
        {
            LanguageManager manager = new();
            if (manager.Get(key, sheetTitle) is not null)
                return manager.Get(key, sheetTitle);
            else
                return orig;
        }

        private void GiveItem(PlayMakerFSM fsm)
        {
            // Custom States
            fsm.AddState("Flower Dialogue");
            fsm.AddState("Give Flower");
            fsm.CopyState("Convo", "Flower Convo");
            fsm.InsertCustomAction("Box Up", () => {
                if (PlayerData.instance.hasXunFlower && !PlayerData.instance.xunFlowerBroken && !Placement.AllObtained())
                    fsm.SendEvent("FLOWER");
            }, 1);
            
            // Replace dialogue key
            CallMethodProper dialogue = fsm.GetFirstActionOfType<CallMethodProper>("Flower Convo");
            var myParams = new[] { new FsmVar(typeof(string)), new FsmVar(typeof(string)) };
            myParams[0].SetValue(dialogueKey);
            myParams[1].SetValue("Flower NPC");
            dialogue.parameters = myParams;

            // Once you give the Flower
            fsm.AddCustomAction("Give Flower", () => PlayerData.instance.hasXunFlower = false);
            fsm.AddCustomAction("Give Flower", () => {
                ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo()
                {
                    FlingType = FlingType.Everywhere,
                    MessageType = MessageType.Corner,
                });
                Placement.AddVisitFlag(VisitState.Opened);
            });
            fsm.AddTransition("Box Up", "FLOWER", "Flower Convo");
            fsm.ChangeTransition("Flower Convo", "CONVO_FINISH", "Give Flower");
            fsm.AddTransition("Give Flower", "FINISHED", "Box Down");
        }
    }
}