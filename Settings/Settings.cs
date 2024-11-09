using MenuChanger.Attributes;

namespace FlowerRandomizer.Settings
{
    public class FlowerSettings
    {
        public bool Enabled { get; set; } = false;
        public VanillaNPCs VanillaNPCs { get; set;} = new();
        public CustomNPCs CustomNPCs { get; set;} = new();

        public bool RandomizeQuestsAtStart { get; set;}
        [MenuRange(1, 13)]
        [DynamicBound(nameof(MaximumQuests), true)]
        public int MinimumQuests { get; set; } = 1;
        [MenuRange(1, 13)]
        [DynamicBound(nameof(MinimumQuests), false)]
        public int MaximumQuests { get; set; } = 13;
    }
}