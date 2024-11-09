using System.Collections.Generic;
using HutongGames.PlayMaker;
using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;
using ItemChanger.Util;
using Satchel;

namespace FlowerRandomizer.IC
{
    public class VanillaFlowerLocation : AutoLocation
    {
        public string objectName;
        public string stateName;
        public VanillaFlowerLocation(string _name, string _sceneName, string _objectName, string _stateName, float x, float y)
        {
            name = $"Flower_Quest-{_name}";
            sceneName = _sceneName;
            objectName = _objectName;
            stateName = _stateName;
            tags = [FlowerLocationTag(x, y)];
        }

        private InteropTag FlowerLocationTag(float x, float y)
        {
            InteropTag tag = new();
            string mapSceneName = sceneName;
            Dictionary<string, string> sceneOverride = [];
            sceneOverride.Add(SceneNames.Room_nailmaster_03, SceneNames.Deepnest_East_06);
            sceneOverride.Add(SceneNames.Room_Queen, SceneNames.Fungus3_48);
            sceneOverride.Add(SceneNames.Ruins_House_03, SceneNames.Ruins2_04);

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
            Events.AddFsmEdit(sceneName, new (objectName, "Conversation Control"), GiveItem);
        }

        protected override void OnUnload()
        {
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