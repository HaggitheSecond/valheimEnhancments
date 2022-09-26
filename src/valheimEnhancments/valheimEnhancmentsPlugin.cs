using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using valheimEnhancments.commands;
using valheimEnhancments.commands.chains;
using valheimEnhancments.commands.toggle;
using static Terminal;
using PluginPaths = valheimEnhancments.Shared.Paths.valheimEnhancementsPlugin;

namespace valheimEnhancments
{
    [BepInPlugin(PluginPaths.Guid, PluginPaths.Name, PluginPaths.Version)]
    public class valheimEnhancmentsPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPatch(typeof(Terminal), "InitTerminal")]
        private static class valheimEnhancmentsTerminalInitModification
        {
            private static void Postfix(Terminal __instance)
            {
                GenerateConsoleCommands();
            }
        }

        private static void GenerateConsoleCommands()
        {
            foreach (var currentCommand in GetCommands())
            {
                new ConsoleCommand(currentCommand.Name, currentCommand.ToString(), (ConsoleEventArgs args) =>
                {
                    ZLog.Log(string.Join(" ", args.Args) + Environment.NewLine + currentCommand.ToString());

                    if (args.Context == null)
                        return;

                    if (args.Context.IsCheatsEnabled() == false)
                    {
                        args.Context.TryRunCommand("devcommands");
                        args.Context.AddString("");
                    }

                    if (args.Args.Length > 1)
                        currentCommand.Execute(args.Context, args.Args.ToList().Skip(1).ToArray());
                    else
                        currentCommand.Execute(args.Context, args.Args);
                });
            }
        }

        public static List<valheimEnhancmentsCommand> GetCommands()
        {
            return Assembly
               .GetAssembly(typeof(valheimEnhancmentsPlugin))
               .GetTypes()
               .Where(f => (f.BaseType == typeof(valheimEnhancmentsCommand)
                         || f.BaseType == typeof(valheimEnhancmentsChainCommand)
                         || f.BaseType == typeof(valheimEnhancmentsToogleCommand))
                         && f.IsAbstract == false)
               .Select(f => Activator.CreateInstance(f))
               .Cast<valheimEnhancmentsCommand>()
               .ToList();
        }
    }
}
