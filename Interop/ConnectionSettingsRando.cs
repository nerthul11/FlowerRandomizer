using ConnectionSettingsRando;
using FlowerRandomizer.Manager;

namespace FlowerRandomizer.Interop
{
    internal static class CSR_Interop
    {
        public static void Hook()
        {
            CSR.Register(
            FlowerRandomizer.Instance.GetName(),
            () => FlowerManager.Settings,
            s => SettingsRandomizer.CopyTo(s, FlowerManager.Settings));
        }
    }
}