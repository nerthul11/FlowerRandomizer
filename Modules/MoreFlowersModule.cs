using ItemChanger;
using ItemChanger.Modules;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FlowerRandomizer.Modules
{
    public class MoreFlowers : Module
    {
        public override void Initialize()
        {
            Events.AddSceneChangeEdit("Room_Mansion", SpawnFlower);        
        }

        public override void Unload()
        {
            Events.RemoveSceneChangeEdit("Room_Mansion", SpawnFlower);
        }

        private void SpawnFlower(Scene scene)
        {
            GameObject flowerSource = UnityEngine.Object.Instantiate(FlowerRandomizer.Instance.flowerSource);
            flowerSource.transform.position = new(25.0f, 6.4f, 2f);
            flowerSource.SetActive(true);
        }
    }
}