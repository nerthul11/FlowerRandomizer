using System;
using FlowerRandomizer.Manager;
using MenuChanger;
using MenuChanger.Extensions;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using RandomizerMod.Menu;
using UnityEngine;

namespace FlowerRandomizer.Settings
{
    public class ConnectionMenu 
    {
        // Top-level definitions
        internal static ConnectionMenu Instance { get; private set; }
        private readonly SmallButton pageRootButton;

        // Menu page and elements
        private readonly MenuPage settingsPage;
        private MenuElementFactory<FlowerSettings> topLevelElementFactory;

        // NPC pages
        private SmallButton vanillaPageButton;
        internal MenuElementFactory<VanillaNPCs> vanillaMEF;
        private SmallButton customPageButton;
        internal MenuElementFactory<CustomNPCs> customMEF;

        public static void Hook()
        {
            RandomizerMenuAPI.AddMenuPage(ConstructMenu, HandleButton);
            MenuChangerMod.OnExitMainMenu += () => Instance = null;
        }

        private static bool HandleButton(MenuPage landingPage, out SmallButton button)
        {
            button = Instance.pageRootButton;
            button.Text.color = FlowerManager.Settings.Enabled ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
            return true;
        }

        private static void ConstructMenu(MenuPage connectionPage)
        {
            Instance = new(connectionPage);
        }

        private ConnectionMenu(MenuPage connectionPage)
        {
            // Define connection page
            settingsPage = new MenuPage("settingsPage", connectionPage);
            topLevelElementFactory = new(settingsPage, FlowerManager.Settings);
            VerticalItemPanel topLevelPanel = new(settingsPage, new Vector2(0, 400), 60, true, topLevelElementFactory.Elements);
            foreach (IValueElement element in topLevelElementFactory.Elements)
                element.SelfChanged += EnableSwitch;
            
            // Add additional pages
            vanillaPageButton = new(settingsPage, "Vanilla NPCs");
            vanillaPageButton.AddHideAndShowEvent(VanillaPage());
            customPageButton = new(settingsPage, "Custom NPCs");
            customPageButton.AddHideAndShowEvent(CustomPage());
            topLevelPanel.Add(vanillaPageButton);
            topLevelPanel.Add(customPageButton);
            
            topLevelPanel.ResetNavigation();
            topLevelPanel.SymSetNeighbor(Neighbor.Down, settingsPage.backButton);
            topLevelPanel.SymSetNeighbor(Neighbor.Up, settingsPage.backButton);
            pageRootButton = new SmallButton(connectionPage, "Flower Randomizer");
            pageRootButton.AddHideAndShowEvent(connectionPage, settingsPage);
        }

        private MenuPage VanillaPage()
        {
            MenuPage vanillaPage = new("Vanilla NPCs", settingsPage);
            MenuLabel header = new(vanillaPage, "Vanilla NPCs");
            vanillaMEF = new(vanillaPage, FlowerManager.Settings.VanillaNPCs);
            VerticalItemPanel vanillaPanel = new(vanillaPage, new Vector2(0, 400), 60, true, vanillaMEF.Elements);
            //vanillaPanel.Add(header);
            vanillaPanel.ResetNavigation();
            vanillaPanel.SymSetNeighbor(Neighbor.Down, vanillaPage.backButton);
            vanillaPanel.SymSetNeighbor(Neighbor.Up, vanillaPage.backButton);
            SetButtonColor(vanillaPageButton, () => FlowerManager.Settings.VanillaNPCs.Any);
            return vanillaPage;
        }

        private MenuPage CustomPage()
        {
            MenuPage customPage = new("Custom NPCs", settingsPage);
            MenuLabel header = new(customPage, "Custom NPCs");
            customMEF = new(customPage, FlowerManager.Settings.CustomNPCs);
            VerticalItemPanel customPanel = new(customPage, new Vector2(0, 400), 60, true, customMEF.Elements);
            //vanillaPanel.Add(header);
            customPanel.ResetNavigation();
            customPanel.SymSetNeighbor(Neighbor.Down, customPage.backButton);
            customPanel.SymSetNeighbor(Neighbor.Up, customPage.backButton);
            SetButtonColor(customPageButton, () => FlowerManager.Settings.CustomNPCs.Any);
            return customPage;
        }

        private void SetButtonColor(SmallButton target, Func<bool> condition)
        {
            target.Parent.BeforeShow += () =>
            {
                target.Text.color = condition() ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
            };
        }

        // Define parameter changes
        private void EnableSwitch(IValueElement obj)
        {
            bool isActive = FlowerManager.Settings.Enabled;
            isActive &= FlowerManager.Settings.VanillaNPCs.Any || FlowerManager.Settings.CustomNPCs.Any;
            pageRootButton.Text.color = isActive ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
        }

        // Apply proxy settings
        public void Disable()
        {
            IValueElement elem = topLevelElementFactory.ElementLookup[nameof(FlowerSettings.Enabled)];
            elem.SetValue(false);
        }

        public void Apply(FlowerSettings settings)
        {
            topLevelElementFactory.SetMenuValues(settings);        
        }
    }
}