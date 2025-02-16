using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.Util;
using Satchel;

namespace FlowerRandomizer.IC
{
    public class VanillaFlowerLocation : AbstractFlowerLocation
    {
        public string objectName;
        public string stateName;
        public VanillaFlowerLocation(string _name, string _sceneName, string _objectName, string _stateName)
        {
            name = $"Flower_Quest-{_name}";
            sceneName = _sceneName;
            objectName = _objectName;
            stateName = _stateName;
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            Events.AddFsmEdit(sceneName, new (objectName, "Conversation Control"), GiveItem);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Events.RemoveFsmEdit(sceneName, new (objectName, "Conversation Control"), GiveItem);
        }

        private void GiveItem(PlayMakerFSM fsm)
        {
            FsmState state = fsm.GetValidState(stateName);
            state.AddCustomAction(() => {
                ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo()
                {
                    FlingType = FlingType.Everywhere,
                    MessageType = MessageType.Corner,
                });

                Placement.AddVisitFlag(VisitState.Opened);
            });
        }
    }
}