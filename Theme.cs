namespace ThemeMode;

public static class Theme
{
    public static IServiceCollection AddThemeService(this MauiAppBuilder builder)
    {
        return builder.Services.AddSingleton<IThemeService, ThemeService>();
    }
}