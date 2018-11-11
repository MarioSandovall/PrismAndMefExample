using MahApps.Metro;
using PrismAndMefExample.Models;
using Shared.Contracts.Services;
using Shared.Extensions;
using System;
using System.Configuration;
using System.IO;

namespace PrismAndMefExample.Service
{
    public class ConfigurationService : IConfigurationService
    {
        public string AccentColor { get; private set; }
        public string Theme { get; private set; }

        private readonly IWindowFactory _windowFactory;
        public ConfigurationService(
            IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public void LoadTheme()
        {
            try
            {
                var file = GetFile();
                if (!File.Exists(file)) CretateOrUpdate("BaseDark", "Blue", file);
                RefreshChanges(file);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateTheme(string theme, string accentColor)
        {
            try
            {
                var file = GetFile();
                CretateOrUpdate(theme, accentColor, file);
                RefreshChanges(file);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GetFile()
        {
            var directory = ConfigurationManager.AppSettings["ConfigurationDirectory"];
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            return Path.Combine(directory, $"{nameof(ConfigurationApp)}.json");
        }

        private void CretateOrUpdate(string theme, string accentColor, string filePath)
        {
            var configuration = new ConfigurationApp()
            {
                Theme = theme,
                AccentColor = accentColor
            };

            configuration.WriteJson(filePath);
        }

        private void RefreshChanges(string file)
        {
            var configuration = file.ReadJson<ConfigurationApp>();

            AccentColor = configuration.AccentColor;
            Theme = configuration.Theme;

            ThemeManager.ChangeAppStyle(_windowFactory.GetMainWindow(),
                ThemeManager.GetAccent(AccentColor),
                ThemeManager.GetAppTheme(Theme));
        }

    }
}
