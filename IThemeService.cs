namespace ThemeMode;

public interface IThemeService
{
    IEnumerable<ThemeMode> GetThemes();

    void SetTheme(ThemeOption themeOption);

    event EventHandler<ThemeOption> ThemeChanged;

    ThemeOption ThemeOption { get; }
}

