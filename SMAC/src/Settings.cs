using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace SMAC
{
    /// <summary>
    /// Controls settings.json file used to store which API server is
    /// being used and what the Sim-Monsters API Server is persistantly.
    /// </summary>
    internal class Settings
    {
        private readonly string jsonSettingsFile = @"./settings.json";
        private readonly string smApiServer = "http://35.222.111.224:3000";
        private readonly JsonSerializerOptions options;

        private SettingsJson settingsJson;

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
        }

        /// <summary>
        /// Gets or sets Sim-Monsters API Server URI.
        /// Sets only if value is a valid URI.
        /// </summary>
        public string SmApiServer
        {
            get => settingsJson.SmApiServer;

            set => settingsJson.SmApiServer = IsValidUri(value) ? value : smApiServer;
        }

        /// <inheritdoc cref="SettingsJson.IsSmServerSelected"/>
        public bool IsSmServerSelected
        {
            get => settingsJson.IsSmServerSelected;

            set => settingsJson.IsSmServerSelected = value;
        }

        /// <summary>
        /// Checks if settings.json exists in active directory.
        /// </summary>
        /// <returns>True if file exits, throws exception if file does not exist.</returns>
        /// <exception cref="FileNotFoundException" />
        public bool CheckSettingsFile()
        {
            if (File.Exists(jsonSettingsFile))
            {
                return true;
            }
            else
            {
                throw new FileNotFoundException("Cannot find settings.json.\nCreating default file.");
            }
        }

        /// <summary>
        /// Attempts to load settings.json, deserialize it, and set the current
        /// Sim-Monsters API Server URI to <see cref="SmApiServer"/>.
        /// If settings.json cannot be found, the file will be created default settings will be applied.
        /// </summary>
        /// <returns>
        /// True if the file loaded successfully and completed,
        /// false if any exceptions are raised.
        /// </returns>
        /// <exception cref="FileNotFoundException" />
        public bool LoadSettingsFile()
        {
            try
            {
                if (CheckSettingsFile())
                {
                    settingsJson = Deserialize();
                    SmApiServer = settingsJson.SmApiServer;
                    return true;
                }
            }
            catch (FileNotFoundException)
            {
                CreateDefaultSettingsFile();
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }

        /// <summary>
        /// Saves <see cref="settingsJson"/> to settings.json.
        /// </summary>
        public void SaveSettingsFile()
        {
            File.WriteAllText(jsonSettingsFile, Serialize());
        }

        /// <summary>
        /// Verifys if a provided <paramref name="uri"/> is a valid URI.
        /// </summary>
        /// <param name="uri">The <paramref name="uri"/> to be checked.</param>
        /// <returns>True if valid, false if not valid.</returns>
        private static bool IsValidUri(string uri)
        {
            return Uri.TryCreate(uri, UriKind.Absolute, out Uri uriResult)
              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Creates new <see cref="SettingsJson"/> with default settings
        /// and writes it to settings.json.
        /// </summary>
        private void CreateDefaultSettingsFile()
        {
            settingsJson = new SettingsJson()
            {
                SmApiServer = smApiServer,
                IsSmServerSelected = false,
            };
            SaveSettingsFile();
        }

        /// <summary>
        /// Converts <see cref="settingsJson"/> into a string for saving to a file.
        /// </summary>
        /// <returns>String form of <see cref="settingsJson"/>.</returns>
        private string Serialize() => JsonSerializer.Serialize(settingsJson, options);

        /// <summary>
        /// Converts contents of <see cref="jsonSettingsFile"/> into a <see cref="SettingsJson"/> object.
        /// </summary>
        /// <returns><see cref="SettingsJson" /> representation of the <see cref="jsonSettingsFile"/>.</returns>
        private SettingsJson Deserialize() => JsonSerializer.Deserialize<SettingsJson>(File.ReadAllText(jsonSettingsFile));
    }
}