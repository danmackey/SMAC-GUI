using System.IO;

namespace SMAC
{
    /// <summary>
    /// Manages the <see cref="Config"/> and <see cref="Settings"/>.
    /// </summary>
    public class Controller
    {
        private readonly string rorApiServer = "http://api.rigsofrods.org";

        private readonly Config config;

        private readonly Settings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        public Controller()
        {
            config = new Config();

            if (CheckConfigFile())
            {
                settings = new Settings();
            }
        }

        /// <inheritdoc cref="SettingsJson.IsSmServerSelected"/>
        public bool IsSmServerSelected
        {
            get => settings.IsSmServerSelected;

            set => settings.IsSmServerSelected = value;
        }

        /// <summary>
        /// Gets current server url.
        /// </summary>
        public string CurrentServerUrl => IsSmServerSelected ? SmApiServer : rorApiServer;

        /// <inheritdoc cref="Settings.SmApiServer"/>
        private string SmApiServer
        {
            get => settings.SmApiServer;

            set => settings.SmApiServer = value;
        }

        /// <inheritdoc cref="Config.CheckConfigFile"/>
        public bool CheckConfigFile()
        {
            try
            {
                if (config.CheckConfigFile())
                {
                    return true;
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }

            return false;
        }

        /// <inheritdoc cref="Config.ApplyChanges(string)"/>
        public bool ApplyConfigChanges()
        {
            try
            {
                string currentApiServer = IsSmServerSelected ? SmApiServer : rorApiServer;

                if (config.ApplyChanges(currentApiServer))
                {
                    return true;
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }

            return false;
        }

        /// <inheritdoc cref="Settings.LoadSettingsFile()"/>
        public bool LoadSettingsFile()
        {
            try
            {
                if (settings.LoadSettingsFile())
                {
                    return true;
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }

            return false;
        }

        /// <inheritdoc cref="Settings.SaveSettingsFile()"/>
        public void SaveSettingsFile()
        {
            settings.SaveSettingsFile();
        }
    }
}