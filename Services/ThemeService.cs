using ThemeMode.Resources.Colors;
using ThemeMode.Resources.Styles;

namespace ThemeMode.Services;

internal sealed class ThemeService : IThemeService
{
    private static List<ThemeMode> ThemeOptionsList => new()
    {
        new ThemeMode("Ocean", "🌊", ThemeOption.Ocean),
        new ThemeMode("Light", "☀️", ThemeOption.Light),
        new ThemeMode("Dark", "🌙", ThemeOption.Dark),
        new ThemeMode("WinUi", "🪟", ThemeOption.WinUi),
        new ThemeMode("WinUi95", "🖥️", ThemeOption.WinUi95),
        new ThemeMode("iOS", "🍏", ThemeOption.iOS),
        new ThemeMode("Android", "🤖", ThemeOption.Android),
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

        _themeOption = ThemeOption.Ocean;
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

        Application.Current.Resources.MergedDictionaries?.Add(new Styles());

        // Add appropriate theme
        switch (ThemeOption)
        {
            case ThemeOption.Ocean:
                Application.Current.Resources.MergedDictionaries?.Add(new OceanMode());
                break;
            case ThemeOption.Dark:
                Application.Current.Resources.MergedDictionaries?.Add(new DarkMode());
                break;
            case ThemeOption.Light:
                Application.Current.Resources.MergedDictionaries?.Add(new LightMode());
                break;
            case ThemeOption.WinUi:
                Application.Current.Resources.MergedDictionaries?.Add(new WinUiMode());
                break;
            case ThemeOption.WinUi95:
                Application.Current.Resources.MergedDictionaries?.Add(new WinUI95Mode());
                break;
            case ThemeOption.iOS:
                Application.Current.Resources.MergedDictionaries?.Add(new iOSMode());
                break;
            case ThemeOption.Android:
                Application.Current.Resources.MergedDictionaries?.Add(new AndroidMode());
                break;
        }
    }
}