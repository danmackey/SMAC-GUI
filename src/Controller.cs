using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

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

        /// <summary>
        /// Gets or sets Sim-Monsters API Server URI.
        /// </summary>
        public string SmApiServer
        {
            get => settings.SmApiServer;

            set => settings.SmApiServer = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Sim-Monsters API Server is selected.
        /// </summary>
        public bool IsSmServerSelected
        {
            get => settings.IsSmServerSelected;

            set => settings.IsSmServerSelected = value;
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

            // bool isSuccessful = false;
            // try
            // {
            //    isSuccessful = config.ApplyChanges(currentApiServer);
            // }
            // catch (FileNotFoundException ex)
            // {
            //    MessageBox.Show($"ERROR: Cannot read file\n{ex.Message}\nDo you have Rigs of Rods 2020.xx installed?", "Sim-Monsters Anti Cheat");
            //    Debug.WriteLine(ex);
            // }
            // catch (DirectoryNotFoundException ex)
            // {
            //    MessageBox.Show($"ERROR: Cannot read directory\n{ex.Message}\nDo you have Rigs of Rods 2020.xx installed?", "Sim-Monsters Anti Cheat");
            //    Debug.WriteLine(ex);
            // }
            // catch (RoRRunningException ex)
            // {
            //    MessageBox.Show("ERROR: Cannot apply changes while Rigs of Rods is open!\nClose Rigs of Rods before applying changes!", "Sim-Monsters Anti Cheat");
            //    Debug.WriteLine(ex);
            // }
            // finally
            // {
            //    if (isSuccessful)
            //    {
            //        MessageBox.Show($"API Server changed to {currentApiServer}.\nAPI and races have been enabled.", "Sim-Monsters Anti Cheat");
            //    }
            // }
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

            // if (!settings.LoadSettingsFile())
            // {
            //    MessageBox.Show("ERROR: Cannot read contents of settings.json.  Resetting to default.", "Sim-Monsters Anti Cheat");
            // }
        }

        /// <inheritdoc cref="Settings.SaveSettingsFile()"/>
        public void SaveSettingsFile()
        {
            settings.SaveSettingsFile();

            // if (settings.SaveSettingsFile())
            // {
            //    MessageBox.Show("Saved settings to settings.json", "Sim-Monsters Anti Cheat");
            // }
        }
    }
}