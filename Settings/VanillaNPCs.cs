namespace FlowerRandomizer.Settings
{
    public class VanillaNPCs
    {
        public bool Elderbug { get; set;}
        public bool Oro { get; set;}
        public bool Godseeker { get; set; }
        public bool Emilitia { get; set; }
        public bool WhiteLady { get; set; }
        public bool Any => ActiveNPCs() > 0;
        public int ActiveNPCs() {
            int activeNPCs = 0;
            activeNPCs += Elderbug ? 1 : 0;
            activeNPCs += Oro ? 1 : 0;
            activeNPCs += Godseeker ? 1 : 0;
            activeNPCs += Emilitia ? 1 : 0;
            activeNPCs += WhiteLady ? 1 : 0;
            return activeNPCs;
        }
    }
}