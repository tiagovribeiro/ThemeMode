
namespace ThemeMode;

public class ThemeService : IThemeService
{
    private static List<ThemeMode> ThemeOptionsList => new()
    {
        new ThemeMode("Light", "☀️", ThemeOption.Light),
        new ThemeMode("Dark", "🌙", ThemeOption.Dark),
        new ThemeMode("System", "📱", ThemeOption.System)
    };

    private const string ThemeOptionPreference = "ThemeOptionPreferences";

    private ThemeOption _themeOption;

    public ThemeOption ThemeOption
    {
        get => _themeOption;
        private set
        {
            _themeOption = value;
            ThemeChanged?.Invoke(this, value);
        }
    }

    public event EventHandler<ThemeOption>? ThemeChanged;

    public ThemeService()
    {
        // Load saved preference or use system default
        var opt = Preferences.Get(ThemeOptionPreference, null);
        if (opt != null)
        {
            _themeOption = (ThemeOption)char.Parse(opt);
            return;
        }

        _themeOption = ThemeOption.System;
    }

    public IEnumerable<ThemeMode> GetThemes()
    {
        return ThemeOptionsList;
    }

    public void SetTheme(ThemeOption themeOption)
    {
        ThemeOption = themeOption;
        Preferences.Set(ThemeOptionPreference, ((char)themeOption).ToString());
        ApplyTheme();
    }

    private void ApplyTheme()
    {
        var mergedDictionaries = Application.Current?.Resources.MergedDictionaries;

        if (mergedDictionaries == null)
        {
            return;
        }

        // Clear existing theme
        mergedDictionaries?.Clear();

        // Add appropriate theme
        if (ThemeOption == ThemeOption.Dark)
        {
            mergedDictionaries?.Add(new ResourceDictionary
            {
                Source = new Uri("Resources/Styles/DarkMode.xaml", UriKind.Relative)
            });
        }
        else if (ThemeOption == ThemeOption.Light)
        {
            mergedDictionaries?.Add(new ResourceDictionary
            {
                Source = new Uri("Resources/Styles/LightMode.xaml", UriKind.Relative)
            });
        }
        else
        {
            if(DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                mergedDictionaries?.Add(new ResourceDictionary
                {
                    Source = new Uri("Resources/Styles/AndroidMode.xaml", UriKind.Relative)
                });
            }
            else if(DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                mergedDictionaries?.Add(new ResourceDictionary
                {
                    Source = new Uri("Resources/Styles/iOSMode.xaml", UriKind.Relative)
                });
            }
            else if(DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                mergedDictionaries?.Add(new ResourceDictionary
                {
                    Source = new Uri("Resources/Styles/WinUIMode.xaml", UriKind.Relative)
                });
            }
        }
    }
}
