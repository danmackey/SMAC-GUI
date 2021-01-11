using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace SMAC
{
    public class Controller
    {
        private readonly string _rorApiServer = "http://api.rigsofrods.org";
        private readonly string _smApiServer = "http://35.222.111.224:3000";
        private readonly string _enableOnlineApi = "Disable Online API=No";
        private readonly string _enableRaces = "Races=Yes";
        private readonly string _cfgFilePath;
        private List<string> _cfgFile;
        private readonly string _jsonSettingsFile = @".\settings.json";
        
        public Settings settings;

        public Controller()
        {
            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string configPath = myDocumentsPath + @"\My Games\Rigs of Rods\config";
            _cfgFilePath = configPath + @"\RoR.cfg";

            if (Directory.Exists(configPath))
            {
                _cfgFile = new List<string>(File.ReadAllLines(_cfgFilePath));
            }
            else
            {
                MessageBox.Show($"ERROR: Cannot read directory\n{configPath}\nDo you have Rigs of Rods 2020.xx installed?", "Sim-Monsters Anti Cheat");
            }

            LoadSettings();
        }

        public void LoadSettings()
        {
            if (File.Exists(_jsonSettingsFile))
            {
                try
                {
                    string json = File.ReadAllText(_jsonSettingsFile);
                    settings = JsonConvert.DeserializeObject<Settings>(json);
                }
                catch (Exception)
                {
                    MessageBox.Show("ERROR: Cannot read contents of settings.json.  Resetting to default.", "Sim-Monsters Anti Cheat");
                    CreateDefaultSettings();
                }
                finally
                {
                    bool isValidApiServer = Uri.TryCreate(settings.smApiServer, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                    if (!isValidApiServer)
                    {
                        MessageBox.Show("ERROR: Cannot verify valid API Server. Resetting to default.", "Sim-Monsters Anti Cheat");
                        settings.smApiServer = _smApiServer;
                    }
                }
            }
            else
            {
                MessageBox.Show("ERROR: Cannot read contents of settings.json.  Resetting to default.", "Sim-Monsters Anti Cheat");
                CreateDefaultSettings();
            }
        }

        public void CreateDefaultSettings()
        {
            settings = new Settings()
            {
                smApiServer = _smApiServer,
                smApiServerActive = false
            };
            SaveSettings();
        }

        public void SaveSettings()
        {
            string json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(_jsonSettingsFile, json);
            MessageBox.Show("Saved settings to settings.json", "Sim-Monsters Anti Cheat");
        }

        public bool ApplyChanges()
        {
            bool changesAppliedSuccessfully = false;

            // Only apply changes if RoR is not open!
            if (Process.GetProcessesByName("RoR").Length == 0)
            {
                int apiServerIndex = _cfgFile.FindIndex(x => x.Contains("Online API URL="));
                int apiEnableIndex = _cfgFile.FindIndex(x => x.Contains("Disable Online API="));
                int enableRacesIndex = _cfgFile.FindIndex(x => x.Contains("Races="));

                string currentApiServer = (settings.smApiServerActive ? settings.smApiServer : _rorApiServer);
                _cfgFile[apiServerIndex] = $"Online API URL={currentApiServer}";
                _cfgFile[apiEnableIndex] = _enableOnlineApi;
                _cfgFile[enableRacesIndex] = _enableRaces;

                File.WriteAllLines(_cfgFilePath, _cfgFile);
            
                string currentApiServerName = (settings.smApiServerActive ? "Sim-Monsters Anti Cheat": "Rigs of Rods API");
                MessageBox.Show($"API Server changed to {currentApiServerName}.\nAPI and races have been enabled.", "Sim-Monsters Anti Cheat");
                changesAppliedSuccessfully = true;
            }
            else
            {
                MessageBox.Show("ERROR: Cannot apply changes while Rigs of Rods is open!\nClose Rigs of Rods before applying changes!", "Sim-Monsters Anti Cheat");
            }
            return changesAppliedSuccessfully;
        }

        public bool SwitchServers(bool isSMApiServer)
        {
            settings.smApiServerActive = isSMApiServer;
            bool changesApplied = ApplyChanges();
            if (changesApplied)
            {
                SaveSettings();
            }
            return changesApplied;
        }
    }

    public class Settings
    {
        public string smApiServer { get; set; }
        public bool smApiServerActive { get; set; }
    }
}
