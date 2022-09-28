using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using valheimEnhancments.config;
using valheimEnhancments.extensions;
using valheimEnhancments.helpers;
using valheimEnhancments.Shared;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsItemDumpCommand : valheimEnhancmentsCommand
    {
        public override string Name => "dumpiteminfo";
        public override string Description => "dumps all item infos to a text file";
        public override string Syntax => "none or [includerecipeless]";

        public valheimEnhancmentsItemDumpCommand()
        {
            this.NeedsLocalPlayer = false;
        }

        public override void Execute(Terminal instance, List<string> arguments)
        {
            var fileInfo = new FileInfo(Paths.valheim.ItemDumpFileLocation);

            if (fileInfo.Exists && fileInfo.CreationTime != default && fileInfo.CreationTime.AddHours(24) > DateTime.Now)
            {
                ZLog.Log("ItemsDump less than 24 hours old, no need to update");
                return;
            }

            var includerecipeless = arguments.Count > 0 && arguments[0] == "includerecipeless";

            var lines = new List<List<object>>();
            var headers = new List<string>()
            {
                "type",
                "name",
                "description",
                "maxStackSize",
                "weight",
                "teleportable",
                "maxdurability",
                "hasrecipe",
                "dlc"
            };

            foreach (var currentItem in ItemHelper.GetAllItems(includerecipeless == false))
            {
                var values = new List<object>
                {
                    currentItem.Type,
                    currentItem.Item.name,
                    currentItem.Description,
                    currentItem.SharedData.m_maxStackSize,
                    currentItem.SharedData.m_weight,
                    currentItem.SharedData.m_teleportable,
                    currentItem.SharedData.m_maxDurability,
                    currentItem.Recipe != null,
                    currentItem.SharedData.m_dlc
                };

                lines.Add(values);

                //ZLog.Log($"{string.Join(", ", values)}");
            }

            ZLog.Log($"Dumping all items to {Paths.valheim.ItemDumpFileLocation}");
            CsvWriter.Write(Paths.valheim.ItemDumpFileLocation, headers, lines);
        }

        [HarmonyPatch(typeof(Player), "OnSpawned")]
        private static class valheimEnhancmentsPlayerOnSpawnedModification
        {
            private static void Postfix()
            {
                if (Console.instance == null
                    || Console.instance.IsConsoleEnabled() == false
                    || ConfigManager.Instance.InfoDumpOnStartup.Value == false)
                    return;

                Console.instance.TryRunCommand(new valheimEnhancmentsItemDumpCommand().Name);
            }
        }
    }
}
