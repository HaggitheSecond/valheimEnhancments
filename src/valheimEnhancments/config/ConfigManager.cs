using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace valheimEnhancments.config
{
    public class ConfigManager
    {
        private BaseUnityPlugin _plugin;

        public static ConfigManager Instance { get; } = new ConfigManager();

        public ConfigEntry<bool> InfoDumpOnStartup { get; private set; }

        public void Create(BaseUnityPlugin plugin)
        {
            this._plugin = plugin;

            var generalSectionName = "General";

            this.InfoDumpOnStartup = plugin.Config.Bind<bool>(
                new ConfigDefinition(generalSectionName, nameof(this.InfoDumpOnStartup)), 
                false, 
                new ConfigDescription("Automatically executes the dumpiteminfo command on spawn"));
        }

        public PerCharacterConfig GetPerCharacterConfig(string playerName)
        {
            var playerConfig = new PerCharacterConfig(playerName);

            var perCharacterSectionName = "PerCharacter."+playerName;

            playerConfig.IsGamemodeCreative = this._plugin.Config.Bind<bool>(
                new ConfigDefinition(perCharacterSectionName, nameof(playerConfig.IsGamemodeCreative)),
                false,
                new ConfigDescription("Automatically sets the player to gamemode creative on spawn"));

            return playerConfig;
        }
    }

    public class PerCharacterConfig
    {
        public string PlayerName { get; }
        public ConfigEntry<bool> IsGamemodeCreative { get; set; }

        public PerCharacterConfig(string playerName)
        {
            this.PlayerName = playerName;
        }
    }
}
