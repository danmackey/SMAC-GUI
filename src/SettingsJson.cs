namespace SMAC
{
    /// <summary>
    /// Represents settings.json.
    /// </summary>
    public class SettingsJson
    {
        /// <summary>
        /// Gets or sets Sim-Monsters API Server URI.
        /// </summary>
        public string SmApiServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Sim-Monsters API Server is selected.
        /// </summary>
        public bool IsSmServerSelected { get; set; }
    }
}