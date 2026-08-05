using FlowerRandomizer.Interop;
using FlowerRandomizer.Manager;
using FlowerRandomizer.Settings;
using Modding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlowerRandomizer
{
    public class FlowerRandomizer : Mod, IGlobalSettings<FlowerSettings> 
    {
        new public string GetName() => "FlowerRandomizer";
        public override string GetVersion() => "1.0.2.0";
        public GameObject flowerSource;
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
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            flowerSource = preloadedObjects["Fungus3_49"]["gg_white_flower"];
            // Ignore completely if Randomizer 4 is inactive
            if (ModHooks.GetMod("Randomizer 4") is Mod)
            {
                Instance.Log("Initializing...");
                FlowerManager.Hook();
                if (ModHooks.GetMod("ConnectionSettingsRando") is Mod)
                    CSR_Interop.Hook();
                if (ModHooks.GetMod("RandoSettingsManager") is Mod)
                    RSM_Interop.Hook();

                Instance.Log("Initialized.");
            }
        }
        public override List<(string, string)> GetPreloadNames()
{
        return new List<(string, string)>
        {
            ("Fungus3_49", "gg_white_flower"),
        };
}
        public void OnLoadGlobal(FlowerSettings s) => GS = s;
        public FlowerSettings OnSaveGlobal() => GS;
    }   
}