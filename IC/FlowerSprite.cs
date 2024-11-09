using ItemChanger;
using ItemChanger.Internal;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace FlowerRandomizer.IC
{
    [Serializable]
    public class FlowerSprite : ISprite
    {
        private static SpriteManager EmbeddedSpriteManager = new(typeof(FlowerSprite).Assembly, "FlowerRandomizer.Resources.Sprites.");
        public string Key { get; set; }
        public FlowerSprite(string key)
        {
            if (!string.IsNullOrEmpty(key))
                Key = key;
        }
        [JsonIgnore]
        public Sprite Value => EmbeddedSpriteManager.GetSprite(Key);
        public ISprite Clone() => (ISprite)MemberwiseClone();
    }
}