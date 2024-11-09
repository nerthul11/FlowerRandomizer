using System.Collections.Generic;
using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;
using ItemChanger.Util;
using Satchel;

namespace FlowerRandomizer.IC
{
    public class AbstractFlowerLocation : AutoLocation
    {
        public string objectName;
        public string fsmName;
        public string sourceState;
        public string sourceEvent;
        public string targetState;
        public AbstractFlowerLocation(string _name, string _sceneName, string _objectName, string _fsmName, string _sourceState, string _sourceEvent, string _targetState, float x, float y)
        {
            name = $"Flower_Quest-{_name}";
            sceneName = _sceneName;
            objectName = _objectName;
            fsmName = _fsmName;
            sourceState = _sourceState;
            sourceEvent = _sourceEvent;
            targetState = _targetState;
            tags = [FlowerLocationTag(x, y)];
        }

        private InteropTag FlowerLocationTag(float x, float y)
        {
            InteropTag tag = new();
            string mapSceneName = sceneName;
            Dictionary<string, string> sceneOverride = [];
            sceneOverride.Add(SceneNames.White_Palace_20, SceneNames.Abyss_05);

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
            Events.AddFsmEdit(sceneName, new (objectName, fsmName), GiveItem);
        }

        protected override void OnUnload()
        {
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