using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using Wox.Plugin;
using Wox.Plugin.Logger;

namespace PowerToysRunPluginGet
{
    public class Program : IPlugin, ISettingProvider, IContextMenu
    {
        public static string PluginID => "b99074a6701b4042baf808a974877295";
        public string Name => "PluginGet";
        public string Description => "Search and install PowerToys Run Plugin through PluginGet";

        private static string? _icoPath;
        private PluginInitContext? _context;
        private PluginOption _pluginOption = new();

        public Program() { }

        #region Init Plugin and execute Query action

        public void Init(PluginInitContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _context.API.ThemeChanged += (Theme currentTheme, Theme newTheme) => { UpdateIconsPath(newTheme); };
            UpdateIconsPath(_context.API.GetCurrentTheme());

            static void UpdateIconsPath(Theme theme)
            {
                var isSettingLightTheme = theme == Theme.Light || theme == Theme.HighContrastWhite;
                _icoPath = isSettingLightTheme ? "Images/icon.light.png" : "Images/icon.dark.png";
            }
        }

        public List<Result> Query(Query query)
        {
            // TODO: Search plugin from GitHub repo, https://github.com/poychang/powertoys-run-plugins
            var results = new List<Result>
            {
                new Result
                {
                    Title = "Plugin Name",
                    SubTitle = "Plugin Name Description",
                    IcoPath = _icoPath,
                    QueryTextDisplay = "Plugin Name",
                    Action = _ =>
                    {
                        // TODO: Try to install the plugin
                        return true;
                    },
                    ToolTipData = new ToolTipData("Title", "Tip Text"),
                    ContextData = new ResultContextData(),
                }
            };

            Log.Info("Logging...", typeof(Program));

            return results.OrderBy(r => r.Title).ToList();
        }

        #endregion

        #region Setting: The option you can activate in PowerToys Run Plugin setting area

        public IEnumerable<PluginAdditionalOption> AdditionalOptions => new List<PluginAdditionalOption>
        {
            //new() {
            //    Key ="PluginSource",
            //    DisplayLabel = "Plugin Source",
            //    DisplayDescription = "A Url for PluginGet to search plugin",
            //    PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
            //    TextValue = "",
            //    TextBoxMaxLength = 300,
            //},
        };

        public Control CreateSettingPanel()
        {
            throw new NotImplementedException();
        }

        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            if (settings != null && settings.AdditionalOptions != null)
            {
                _pluginOption.TextOption = settings.AdditionalOptions.FirstOrDefault(x => x.Key == "PluginSource")?.TextValue ?? default;
            }
            else
            {
                _pluginOption.TextOption = default;
            }
        }

        #endregion

        #region Context Menu: The action button on the right of result item

        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            if (selectedResult.ContextData is not ResultContextData data)
            {
                return new List<ContextMenuResult>();
            }

            return data.CreateContextMenuResult();
        }

        #endregion
    }

    public class ResultContextData
    {
        private static readonly string _pluginName = Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty;
        public List<ContextMenuResult> CreateContextMenuResult()
        {
            // Character Map tool
            // To open the character map tool on Windows, press Win + R keys, then enter 'charmap'
            // Choose font as FontFamily then find the glyph you need
            return new List<ContextMenuResult>
            {
                new ContextMenuResult
                {
                    Title = "Install (Ctrl+I)",
                    Glyph = "\xE896",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    AcceleratorModifiers = ModifierKeys.Control,
                    AcceleratorKey = Key.I,
                    PluginName = _pluginName,
                    Action = _ =>
                    {
                        try
                        {
                            // TODO: Install the plugin
                        }
                        catch (Exception ex)
                        {
                            Log.Exception("Failed to install the plugin", ex, typeof(ResultContextData));
                        }
                        return true;
                    },
                },
                new ContextMenuResult
                {
                    Title = "Update (Ctrl+U)",
                    Glyph = "\xE895",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    AcceleratorModifiers = ModifierKeys.Control,
                    AcceleratorKey = Key.I,
                    PluginName = _pluginName,
                    Action = _ =>
                    {
                        try
                        {
                            // TODO: Update the plugin
                        }
                        catch (Exception ex)
                        {
                            Log.Exception("Failed to update the plugin", ex, typeof(ResultContextData));
                        }
                        return true;
                    },
                },
                new ContextMenuResult
                {
                    Title = "Remove (Ctrl+R)",
                    Glyph = "\xE74D",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    AcceleratorModifiers = ModifierKeys.Control,
                    AcceleratorKey = Key.R,
                    PluginName = _pluginName,
                    Action = _ =>
                    {
                        try
                        {
                            // TODO: Remove the plugin
                        }
                        catch (Exception ex)
                        {
                            Log.Exception("Failed to remove the plugin", ex, typeof(ResultContextData));
                        }
                        return true;
                    },
                },
            };
        }
    }
}
