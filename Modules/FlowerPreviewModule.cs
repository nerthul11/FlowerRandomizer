using System.Collections.Generic;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.Internal;
using ItemChanger.Modules;
using ItemChanger.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FlowerRandomizer.Modules
{
    public class FlowerPreviewModule : Module
    {
        public override void Initialize()
        {
            Events.AddSceneChangeEdit("Room_Mansion", SpawnMournerTablet);
            Events.AddSceneChangeEdit("Fungus3_49", SpawnTraitorTablet);
            Events.AddLanguageEdit(new("Lore Tablets", "TUT_TAB_02"), FlowerPreviews);
        }

        public override void Unload()
        {
            Events.RemoveSceneChangeEdit("Room_Mansion", SpawnMournerTablet);
            Events.RemoveSceneChangeEdit("Fungus3_49", SpawnTraitorTablet);
            Events.RemoveLanguageEdit(new("Lore Tablets", "TUT_TAB_02"), FlowerPreviews);
        }

        private void SpawnMournerTablet(Scene scene)
        {
            GameObject tablet = TabletUtility.InstantiateTablet("Flower_Preview");
            tablet.transform.localPosition = new(16.4f, 7f, 2f);
            tablet.SetActive(true);
        }

        private void SpawnTraitorTablet(Scene scene)
        {
            GameObject tablet = TabletUtility.InstantiateTablet("Flower_Preview");
            tablet.transform.localPosition = new(29.3f, 7f, 2f);
            tablet.SetActive(true);
        }

        private void FlowerPreviews(ref string value)
        {
            if (GameManager._instance.sceneName == SceneNames.Room_Mansion || GameManager._instance.sceneName == SceneNames.Fungus3_49)
            {
                value = "Available items:";
                foreach (KeyValuePair<string, AbstractPlacement> placement in Ref.Settings.Placements)
                {
                    if (placement.Value.Name.Contains("Flower_Quest"))
                    {
                        value += "<br>";
                        value += placement.Value.Name.Split('-')[1].Replace('_', ' ');
                        value += " - ";
                        value += placement.Value.GetUIName();
                        if (placement.Value.Items.AnyEverObtained())
                            value += "Obtained";
                    }
                }
            }
        }
    }
}