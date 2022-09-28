using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using valheimEnhancments.commands;
using valheimEnhancments.commands.chains;
using valheimEnhancments.commands.toggle;
using valheimEnhancments.config;
using static Terminal;
using PluginPaths = valheimEnhancments.Shared.Paths.valheimEnhancementsPlugin;

namespace valheimEnhancments
{
    [BepInPlugin(PluginPaths.Guid, PluginPaths.Name, PluginPaths.Version)]
    public class valheimEnhancmentsPlugin : BaseUnityPlugin
    {
        private void Awake()
        {
            ConfigManager.Instance.Create(this);
            Harmony.CreateAndPatchAll(Assembly.GetAssembly(this.GetType()));
        }

        [HarmonyPatch(typeof(Terminal), "InitTerminal")]
        private static class valheimEnhancmentsTerminalInitModification
        {
            private static void Postfix(Terminal __instance)
            {
                GenerateConsoleCommands();
            }
        }

        [HarmonyPatch(typeof(Player), "OnSpawned")]
        private static class valheimEnhancmentsPlayerAwakeModification
        {
            private static void Postfix(Player __instance)
            {
                if (__instance == null)
                    return;

                var config = ConfigManager.Instance.GetPerCharacterConfig(__instance.GetPlayerName());

                if(config.IsGamemodeCreative.Value)
                {
                    Console.instance.TryRunCommand(new valheimEnhancmentsCreativeChainCommand().Name);
                }
            }
        }

        private static void GenerateConsoleCommands()
        {
            foreach (var currentCommand in GetCommands())
            {
                new ConsoleCommand(
                    currentCommand.Name, 
                    currentCommand.ToString(), 
                    (ConsoleEventArgs args) =>
                    {
                        ZLog.Log(string.Join(" ", args.Args) + Environment.NewLine + currentCommand.ToString());

                        if (args.Context == null)
                            return;

                        if(currentCommand.NeedsLocalPlayer && Player.m_localPlayer == null)
                        {
                            args.Context.AddString($"Command {currentCommand.Name} needs a local player to run!");
                            return;
                        }

                        if (args.Context.IsCheatsEnabled() == false)
                        {
                            args.Context.TryRunCommand("devcommands");
                            args.Context.AddString("");
                        }

                        if (args.Args.Length > 1)
                            currentCommand.Execute(args.Context, args.Args.ToList().Skip(1).ToList());
                        else
                            currentCommand.Execute(args.Context, new List<string>());
                    }, 
                    currentCommand.IsCheat,
                    currentCommand.IsNetwork,
                    currentCommand.OnlyServer,
                    currentCommand.IsSecret,
                    currentCommand.AllowInDevBuild,
                    optionsFetcher: new Terminal.ConsoleOptionsFetcher(currentCommand.GetOptions));
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
