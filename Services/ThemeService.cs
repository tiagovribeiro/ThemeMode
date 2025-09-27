using ThemeMode.Resources.Styles;

namespace ThemeMode.Services;

internal sealed class ThemeService : IThemeService
{
    private static List<ThemeMode> ThemeOptionsList => new()
    {
        new ThemeMode("System", "📱", ThemeOption.System),
        new ThemeMode("Light", "☀️", ThemeOption.Light),
        new ThemeMode("Dark", "🌙", ThemeOption.Dark),
        new ThemeMode("Dark", "🌊", ThemeOption.Ocean)
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
        if (Application.Current?.Resources?.MergedDictionaries == null)
        {
            return;
        }

        // Clear existing theme
        Application.Current.Resources.MergedDictionaries?.Clear();

        // Add appropriate theme
        if (ThemeOption == ThemeOption.Dark)
        {
            Application.Current.Resources.MergedDictionaries?.Add(new ResourceDictionary
            {
                Source = new Uri("Resources/Styles/DarkMode.xaml", UriKind.Relative)
            });
        }
        else if (ThemeOption == ThemeOption.Light)
        {
            Application.Current.Resources.MergedDictionaries?.Add(new ResourceDictionary
            {
                Source = new Uri("Resources/Styles/LightMode.xaml", UriKind.Relative)
            });
        }
        else if (ThemeOption == ThemeOption.Ocean)
        {
            Application.Current.Resources.MergedDictionaries?.Add(new OceanMode());
        }
        else
        {
            Uri uri = new Uri("Resources/Styles/OceanMode.xaml", UriKind.Relative);
#if ANDROID

            uri = new Uri("Resources/Styles/AndroidMode.xaml", UriKind.Relative);
#endif

#if IOS
            uri = new Uri("Resources/Styles/iOSMode.xaml", UriKind.Relative);          
#endif

#if WINDOWS
            uri = new Uri("Resources/Styles/WinUIMode.xaml", UriKind.Relative);
#endif

            Application.Current.Resources.MergedDictionaries?.Add(new ResourceDictionary
            {
                Source = uri
            });
        }
    }
}