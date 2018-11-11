namespace Shared.Contracts.Services
{
    public interface IConfigurationService
    {
        void LoadTheme();
        void UpdateTheme(string theme, string accentColor);
    }
}