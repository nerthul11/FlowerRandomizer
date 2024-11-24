using FlowerRandomizer.Interop;
using FlowerRandomizer.Manager;
using FlowerRandomizer.Settings;
using Modding;
using System;

namespace FlowerRandomizer
{
    public class FlowerRandomizer : Mod, IGlobalSettings<FlowerSettings> 
    {
        new public string GetName() => "FlowerRandomizer";
        public override string GetVersion() => "1.0.0.1";
        private static FlowerRandomizer _instance;
        public FlowerRandomizer() : base()
        {
            _instance = this;
        }
        internal static FlowerRandomizer Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException($"{nameof(FlowerRandomizer)} was never initialized");
                }
                return _instance;
            }
        }
        public FlowerSettings GS { get; internal set; } = new();
        public override void Initialize()
        {
            // Ignore completely if Randomizer 4 is inactive
            if (ModHooks.GetMod("Randomizer 4") is Mod)
            {
                Instance.Log("Initializing...");
                FlowerManager.Hook();
                if (ModHooks.GetMod("RandoSettingsManager") is Mod)
                    RSM_Interop.Hook();

                Instance.Log("Initialized.");
            }
        }
        public void OnLoadGlobal(FlowerSettings s) => GS = s;
        public FlowerSettings OnSaveGlobal() => GS;
    }   
}