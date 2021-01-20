using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SMAC
{
    /// <summary>
    /// Represents Rigs of Rods config file.
    /// Has functionality to modify config file options
    /// for races, enable/disable online api, and
    /// changing online api.
    /// </summary>
    internal class Config
    {
        private readonly string enableOnlineApi = "Disable Online API=No";
        private readonly string onlineApiUrl = "Online API URL=";
        private readonly string enableRaces = "Races=Yes";
        private readonly string configPath;
        private readonly string cfgFilePath;
        private List<string> cfgFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class with
        /// the path to the Rigs of Rods config folder and path to RoR.cfg stored.
        /// </summary>
        public Config()
        {
            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            configPath = myDocumentsPath + @"\My Games\Rigs of Rods\config";
            cfgFilePath = configPath + @"\RoR.cfg";
        }

        /// <summary>
        /// Applys current changes to RoR.cfg if Rigs of Rods is not running.
        /// </summary>
        /// <param name="apiServer">Valid URI for Rigs of Rods API server.</param>
        /// <returns>True if changes are applied, false if not.</returns>
        /// <exception cref="RoRRunningException"/>
        /// <exception cref="FileNotFoundException" />
        /// <exception cref="DirectoryNotFoundException" />
        public bool ApplyChanges(string apiServer)
        {
            try
            {
                if (Process.GetProcessesByName("RoR").Length == 0)
                {
                    LoadConfigFile();
                    SetApiServer(apiServer);
                    EnableOnlineApi();
                    EnableRaces();
                    SaveConfigFile();
                    return true;
                }
                else
                {
                    throw new RoRRunningException("Cannot apply changes while Rigs of Rods is open!\nClose Rigs of Rods before applying changes!\n");
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
        }

        /// <summary>
        /// Checks that the <see cref="configPath"/> and
        /// <see cref="cfgFilePath"/> are both valid and exist.
        /// </summary>
        /// <returns>True if both are valid and exist, false if not.</returns>
        /// <exception cref="FileNotFoundException" />
        /// <exception cref="DirectoryNotFoundException" />
        public bool CheckConfigFile()
        {
            if (Directory.Exists(configPath))
            {
                if (File.Exists(cfgFilePath))
                {
                    return true;
                }
                else
                {
                    throw new FileNotFoundException($"File does not exist:\n{configPath}\\RoR.cfg\n\nDo you have Rigs of Rods 2020.xx installed?\n");
                }
            }
            else
            {
                throw new DirectoryNotFoundException($"Directory does not exist:\n{configPath}\n\nDo you have Rigs of Rods 2020.xx installed?\n");
            }
        }

        /// <summary>
        /// Attempts to load RoR.cfg into <see cref="cfgFile"/>.
        /// </summary>
        /// <returns>True if file exits, throws exception if file does not exist.</returns>
        /// <exception cref="FileNotFoundException" />
        /// <exception cref="DirectoryNotFoundException" />
        private bool LoadConfigFile()
        {
            try
            {
                if (CheckConfigFile())
                {
                    cfgFile = new List<string>(File.ReadAllLines(cfgFilePath));
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

        /// <summary>
        /// Attempts to save <see cref="cfgFile"/> into RoR.cfg.
        /// </summary>
        /// <returns>True if file exits, throws exception if file does not exist.</returns>
        /// <exception cref="FileNotFoundException" />
        /// <exception cref="DirectoryNotFoundException" />
        private bool SaveConfigFile()
        {
            try
            {
                if (CheckConfigFile())
                {
                    File.WriteAllLines(cfgFilePath, cfgFile);
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

        /// <summary>
        /// Set API Server URI to <paramref name="apiServer"/>.
        /// </summary>
        /// <param name="apiServer">A valid URI that will be used as the Rigs of Rods API Server.</param>
        private void SetApiServer(string apiServer)
        {
            cfgFile[cfgFile.FindIndex(x => x.Contains(onlineApiUrl))] = onlineApiUrl + apiServer;
        }

        /// <summary>
        /// Enable Rigs of Rods API messaging.  Used to send race data to server after race completion.
        /// </summary>
        private void EnableOnlineApi()
        {
            int apiStatusIndex = cfgFile.FindIndex(x => x.Contains("Disable Online API="));
            cfgFile[apiStatusIndex] = enableOnlineApi;
        }

        /// <summary>
        /// Allow Rigs of Rods to perform races on a given terrain.
        /// </summary>
        private void EnableRaces()
        {
            int enableRacesIndex = cfgFile.FindIndex(x => x.Contains("Races="));
            cfgFile[enableRacesIndex] = enableRaces;
        }
    }
}