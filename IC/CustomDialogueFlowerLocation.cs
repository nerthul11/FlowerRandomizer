using System.Collections.Generic;
using FlowerRandomizer.Manager;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.Util;
using Modding;
using Satchel;

namespace FlowerRandomizer.IC
{
    public class CustomDialogueFlowerLocation : AbstractFlowerLocation
    {
        public string objectName;
        public string dialogueKey;
        public CustomDialogueFlowerLocation(string _name, string _sceneName, string _objectName, string _dialogueKey)
        {
            name = $"Flower_Quest-{_name}";
            sceneName = _sceneName;
            objectName = _objectName;
            dialogueKey = _dialogueKey;
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            ModHooks.LanguageGetHook += CustomDialogue;
            Events.AddFsmEdit(sceneName, new (objectName, "Conversation Control"), GiveItem);
            if (name.Contains("Marissa"))
                Events.AddFsmEdit(sceneName, new (objectName, "ghost_npc_death"), PreventDeath);
            if (name.Contains("Sheo"))
                Events.AddFsmEdit(sceneName, new ("NM Sheo NPC Modeller", "Conversation Control"), GiveItem);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            ModHooks.LanguageGetHook -= CustomDialogue;
            Events.RemoveFsmEdit(sceneName, new (objectName, "Conversation Control"), GiveItem);
            if (objectName.Contains("Marissa"))
                Events.RemoveFsmEdit(sceneName, new (objectName, "ghost_npc_death"), PreventDeath);
            if (objectName.Contains("Sheo"))
                Events.RemoveFsmEdit(sceneName, new ("NM Sheo NPC Modeller", "Conversation Control"), GiveItem);
        }

        private void PreventDeath(PlayMakerFSM fsm)
        {
            fsm.AddState("Active Flower?");
            fsm.AddCustomAction("Active Flower?", () => {
                if (PlayerData.instance.hasXunFlower && !PlayerData.instance.xunFlowerBroken)
                    fsm.SendEvent("FLOWER");
            });
            fsm.AddTransition("Active Flower?", "FINISHED", "Revek");
            fsm.AddTransition("Active Flower?", "FLOWER", "Idle");
            fsm.ChangeTransition("Init", "DESTROYED", "Active Flower?");
            fsm.InsertCustomAction("Init", () => FlowerRandomizer.Instance.Log("State INIT"), 0);
        }

        private string CustomDialogue(string key, string sheetTitle, string orig)
        {
            LanguageManager manager = new();
            return manager.Get(key, sheetTitle) ?? orig;
        }

        private void GiveItem(PlayMakerFSM fsm)
        {
            // Custom States
            fsm.AddState("Flower Dialogue");
            fsm.AddState("Give Flower");
            fsm.CopyState("Repeat", "Flower Convo");
            
            fsm.AddCustomAction("Box Up", () => {
                if (PlayerData.instance.hasXunFlower && !PlayerData.instance.xunFlowerBroken && !Placement.AllObtained())
                    fsm.SendEvent("FLOWER");
            });

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
            fsm.AddTransition("Give Flower", "FINISHED", "Talk Finish");
        }
    }
}