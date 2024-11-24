using FlowerRandomizer.Manager;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Util;
using Modding;
using Satchel;

namespace FlowerRandomizer.IC
{
    public class DreamDialogueFlowerLocation : AbstractFlowerLocation
    {
        public string dialogueKey;
        public DreamDialogueFlowerLocation(string _name, string _sceneName, string _dialogueKey)
        {
            name = $"Flower_Quest-{_name}";
            sceneName = _sceneName;
            dialogueKey = _dialogueKey;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            ModHooks.LanguageGetHook += CustomDialogue;
            Events.AddFsmEdit(sceneName, new ("Dream Dialogue", "npc_dream_dialogue"), GiveItem);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            ModHooks.LanguageGetHook -= CustomDialogue;
            Events.RemoveFsmEdit(sceneName, new ("Dream Dialogue", "npc_dream_dialogue"), GiveItem);
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