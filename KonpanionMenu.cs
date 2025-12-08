using Satchel.BetterMenus;
using System.IO;
using System.Linq;

namespace Konpanion
{
    internal class KonpanionMenu
    {
        private static Menu MenuRef;
        private static string[] availableSkins = new string[] { "No skins found" };

        internal static Menu PrepareMenu()
        {
            LoadAvailableSkins();

            var menu = new Menu("Konpanion Skin", new Element[]
            {
                new HorizontalOption(
                    "Option",
                    "",
                    new string[] { "off", "CurrentKnight", "Custom" },
                    (index) => 
                    {
                        Konpanion.Instance.GlobalSettings.SelectedSkinOption = index;
                        Konpanion.Instance.SaveSettings();
                        Konpanion.Instance.OnOptionChanged();
                        UpdateDescription();
                    },
                    () => Konpanion.Instance.GlobalSettings.SelectedSkinOption,
                    Id: "SkinOption"
                ),
                new HorizontalOption(
                    "Skin",
                    "",
                    availableSkins,
                    (index) => 
                    {
                        Konpanion.Instance.GlobalSettings.CustomSubOption = index;
                        Konpanion.Instance.SaveSettings();
                        Konpanion.Instance.OnOptionChanged();
                    },
                    () => Konpanion.Instance.GlobalSettings.CustomSubOption,
                    Id: "CustomSubOption"
                )
                {
                    isVisible = false
                }
            });

            return menu;
        }

        private static void LoadAvailableSkins()
        {
            try
            {
                string modPath = Path.GetDirectoryName(typeof(Konpanion).Assembly.Location);
                string skinsPath = Path.Combine(modPath, "Skins");

                if (Directory.Exists(skinsPath))
                {
                    string[] skinFolders = Directory.GetDirectories(skinsPath);
                    
                    if (skinFolders.Length > 0)
                    {
                        availableSkins = skinFolders.Select(path => Path.GetFileName(path)).ToArray();
                        Konpanion.Instance.Log($"Found {availableSkins.Length} skins: {string.Join(", ", availableSkins)}");
                    }
                    else
                    {
                        availableSkins = new string[] { "No skins found" };
                        Konpanion.Instance.Log("Skins folder exists but is empty");
                    }
                }
                else
                {
                    availableSkins = new string[] { "No skins found" };
                    Konpanion.Instance.Log($"Skins folder not found at: {skinsPath}");
                }
            }
            catch (System.Exception e)
            {
                availableSkins = new string[] { "Error loading skins" };
                Konpanion.Instance.LogError($"Error loading skins: {e.Message}");
            }
        }

        internal static void UpdateDescription()
        {
            if (MenuRef == null) return;

            var skinOption = MenuRef.Find("SkinOption") as HorizontalOption;
            var customSubOption = MenuRef.Find("CustomSubOption");
            
            if (skinOption != null)
            {
                int selectedIndex = Konpanion.Instance.GlobalSettings.SelectedSkinOption;
                
                if (selectedIndex == 0)
                {
                    skinOption.Description = "";
                }
                else if (selectedIndex == 1)
                {
                    skinOption.Description = "Your Knights current skin";
                }
                else if (selectedIndex == 2)
                {
                    skinOption.Description = "Custom skin options";
                }
            }

            if (customSubOption != null)
            {
                customSubOption.isVisible = Konpanion.Instance.GlobalSettings.SelectedSkinOption == 2;
                MenuRef.Update();
            }
        }

        internal static MenuScreen GetMenu(MenuScreen lastMenu)
        {
            MenuRef ??= PrepareMenu();
            
            MenuRef.OnBuilt += (_, _) =>
            {
                UpdateDescription();
            };

            return MenuRef.GetMenuScreen(lastMenu);
        }
    }
}
