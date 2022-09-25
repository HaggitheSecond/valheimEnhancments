using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using valheimEnhancments.extensions;
using valheimEnhancments.helpers;
using valheimEnhancments.Shared;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsInfoDumpCommand : valheimEnhancmentsCommand
    {
        public override string Name => "dumpiteminfo";
        public override string Description => "dumps all item infos to a text file";
        public override string Syntax => "";

        public override void Execute(Terminal instance, string[] arguments)
        {
            var fileInfo = new FileInfo(Paths.valheim.ItemDumpFileLocation);

            if (fileInfo.Exists && fileInfo.CreationTime != default && fileInfo.CreationTime.AddHours(24) > DateTime.Now)
            {
                ZLog.Log("ItemsDump less than 24 hours old, no need to update");
                return;
            }

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
                "hasrecipe"
            };

            foreach (var currentItemType in Enum.GetValues(typeof(ItemDrop.ItemData.ItemType)) as ItemDrop.ItemData.ItemType[])
            {
                var items = ObjectDB.instance.GetAllItems(currentItemType, string.Empty);

                foreach (var currentItem in items)
                {
                    var recipe = ObjectDB.instance.GetRecipe(currentItem.m_itemData);

                    var shared = currentItem.m_itemData.m_shared;

                    string description;
                    if (shared.m_description.Contains("$"))
                    {
                        description = Localization.instance.Localize(shared.m_description.Substring(shared.m_description.IndexOf("$")));
                    }
                    else
                    {
                        description = shared.m_description;
                    }

                    lines.Add(new List<object>
                    {
                        currentItemType,
                        currentItem.name,
                        description,
                        shared.m_maxStackSize,
                        shared.m_weight,
                        shared.m_teleportable,
                        shared.m_maxDurability,
                        recipe != null
                    });
                }
            }

            ZLog.Log($"Dumping all items to {Paths.valheim.ItemDumpFileLocation}");
            CsvWriter.Write(Paths.valheim.ItemDumpFileLocation, headers, lines);
        }

        [HarmonyPatch(typeof(Player), "OnSpawned")]
        private static class valheimEnhancmentsPlayerOnSpawnedModification
        {
            private static void Postfix()
            {
                if (Console.instance == null || Console.instance.IsConsoleEnabled() == false)
                    return;

                Console.instance.TryRunCommand(new valheimEnhancmentsInfoDumpCommand().Name);
            }
        }
    }
}
