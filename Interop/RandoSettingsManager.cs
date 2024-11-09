using RandoSettingsManager;
using RandoSettingsManager.SettingsManagement;
using RandoSettingsManager.SettingsManagement.Versioning;
using FlowerRandomizer.Settings;
using FlowerRandomizer.Manager;

namespace FlowerRandomizer.Interop
{
    internal static class RSM_Interop
    {
        public static void Hook()
        {
            RandoSettingsManagerMod.Instance.RegisterConnection(new FlowerSettingsProxy());
        }
    }

    internal class FlowerSettingsProxy : RandoSettingsProxy<FlowerSettings, string>
    {
        public override string ModKey => FlowerRandomizer.Instance.GetName();

        public override VersioningPolicy<string> VersioningPolicy { get; }
            = new EqualityVersioningPolicy<string>(FlowerRandomizer.Instance.GetVersion());

        public override void ReceiveSettings(FlowerSettings settings)
        {
            if (settings != null)
            {
                ConnectionMenu.Instance!.Apply(settings);
            }
            else
            {
                ConnectionMenu.Instance!.Disable();
            }
        }

        public override bool TryProvideSettings(out FlowerSettings settings)
        {
            settings = FlowerManager.Settings;
            return settings.Enabled;
        }
    }
}