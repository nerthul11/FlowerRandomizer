using System.Linq;
using ItemChanger;
using ItemChanger.Modules;
using ItemChanger.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FlowerRandomizer.Modules
{
    public class FlowerPreviewModule : Module
    {
        public delegate string FlowerPreview();
        public static event FlowerPreview OnFlowerPreview;
        public override void Initialize()
        {
            if (OnFlowerPreview.GetInvocationList().Cast<FlowerPreview>().Count() > 0)
            {
                Events.AddSceneChangeEdit("Room_Mansion", SpawnMournerTablet);
                Events.AddSceneChangeEdit("Fungus3_49", SpawnTraitorTablet);
                Events.AddLanguageEdit(new("Lore Tablets", "TUT_TAB_02"), FlowerPreviews);
            }
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
                foreach (FlowerPreview handler in OnFlowerPreview.GetInvocationList().Cast<FlowerPreview>())
                    value += handler.Invoke();
            }
        }
    }
}