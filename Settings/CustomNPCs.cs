namespace FlowerRandomizer.Settings
{
    public class CustomNPCs
    {
        public bool Mato { get; set;}
        public bool Sheo { get; set;}
        public bool Marissa { get; set; }
        public bool Midwife { get; set; }
        public bool Isma { get; set; }
        public bool Radiance { get; set; }
        public bool PaleKing { get; set; }
        public bool Pain { get; set; }
        public bool Any => ActiveNPCs() > 0;
        public int ActiveNPCs() {
            int activeNPCs = 0;
            activeNPCs += Mato ? 1 : 0;
            activeNPCs += Sheo ? 1 : 0;
            activeNPCs += Marissa ? 1 : 0;
            activeNPCs += Midwife ? 1 : 0;
            activeNPCs += Isma ? 1 : 0;
            activeNPCs += Radiance ? 1 : 0;
            activeNPCs += PaleKing ? 1 : 0;
            activeNPCs += Pain ? 1 : 0;
            return activeNPCs;
        }
    }
}