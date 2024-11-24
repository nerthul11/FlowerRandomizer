using ItemChanger;
using ItemChanger.Util;
using Satchel;

namespace FlowerRandomizer.IC
{
    public class PainFlowerLocation : AbstractFlowerLocation
    {
        public string objectName;
        public string fsmName;
        public string sourceState;
        public string sourceEvent;
        public string targetState;
        public PainFlowerLocation(string _name, string _sceneName, string _objectName, string _fsmName, string _sourceState, string _sourceEvent, string _targetState)
        {
            name = $"Flower_Quest-{_name}";
            sceneName = _sceneName;
            objectName = _objectName;
            fsmName = _fsmName;
            sourceState = _sourceState;
            sourceEvent = _sourceEvent;
            targetState = _targetState;
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            Events.AddFsmEdit(sceneName, new (objectName, fsmName), GiveItem);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Events.RemoveFsmEdit(sceneName, new (objectName, fsmName), GiveItem);
        }

        private void GiveItem(PlayMakerFSM fsm)
        {
            fsm.AddState("Flower?");
            fsm.AddState("Give Flower");
            
            // Check if flower is present
            fsm.AddCustomAction("Flower?", () => {
                if (PlayerData.instance.hasXunFlower && !PlayerData.instance.xunFlowerBroken && !Placement.AllObtained())
                    fsm.SendEvent("FLOWER");
            });
            fsm.AddTransition("Flower?", "FINISHED", targetState);
            fsm.AddTransition("Flower?", "FLOWER", "Give Flower");

            // Trade flower for item
            
            fsm.AddCustomAction("Give Flower", () => PlayerData.instance.hasXunFlower = false);
            fsm.AddCustomAction("Give Flower", () => {
                ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo()
                {
                    FlingType = FlingType.Everywhere,
                    MessageType = MessageType.Corner,
                });

                Placement.AddVisitFlag(VisitState.Opened);
            });
            fsm.AddTransition("Give Flower", "FINISHED", targetState);

            // Insert flower check
            fsm.ChangeTransition(sourceState, sourceEvent, "Flower?");
        }
    }
}