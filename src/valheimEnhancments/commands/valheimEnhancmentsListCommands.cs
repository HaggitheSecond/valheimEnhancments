using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using valheimEnhancments.extensions;
using static Terminal;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsListCommands : valheimEnhancmentsCommand
    {
        public override string Name => "list";

        public override string Description => "lists all available commands";

        public override string Syntax => "[index]";

        public valheimEnhancmentsListCommands()
        {
            this.NeedsLocalPlayer = false;
        }
        
        public override void Execute(Terminal instance, List<string> arguments)
        {
            var page = 1;

            if (arguments.Count >= 1 && string.IsNullOrWhiteSpace(arguments[0]) == false && int.TryParse(arguments[0], out var argumentPage) && argumentPage > 0)
                page = argumentPage;

            var commands = this.GetAllCommands().ToList();

            var commandsPerPage = 15;

            if (arguments.Count >= 2 && string.IsNullOrWhiteSpace(arguments[1]) == false && int.TryParse(arguments[1], out var argumentcommandsPerPage) && argumentcommandsPerPage > 0)
                commandsPerPage = argumentcommandsPerPage;

            var pages = (int)Math.Ceiling(commands.Count() / (decimal)commandsPerPage);

            if (page > pages)
                page = pages;

            instance.AddString($"Showing command page: {page}/{pages}");

            foreach (var (name, description, syntax, origin) in commands.Skip((page - 1) * commandsPerPage).Take(commandsPerPage))
            {
                instance.AddString(name 
                    + (string.IsNullOrWhiteSpace(description) ? string.Empty : " | " + description.CapitalizeFirstLetter()) 
                    + (string.IsNullOrWhiteSpace(syntax) ? string.Empty : " | " + syntax));
            }
        }

        private IEnumerable<(string name, string description, string syntax, CommandOrigin origin)> GetAllCommands()
        {
            var terminalCommands = (Dictionary<string, ConsoleCommand>)Traverse.Create(typeof(Terminal)).Field("commands").GetValue();
            var ownCommands = valheimEnhancmentsPlugin.GetCommands();

            foreach (var currentTerminalCommand in terminalCommands.OrderBy(f => f.Key))
            {
                var ownCommand = ownCommands.FirstOrDefault(f => f.Name == currentTerminalCommand.Key);

                yield return (currentTerminalCommand.Key,
                    ownCommand?.Description ?? currentTerminalCommand.Value.Description,
                    ownCommand?.Syntax,
                    (ownCommand is null ? CommandOrigin.Base : CommandOrigin.Plugin));
            }

        }

        private enum CommandOrigin
        {
            Base,
            Plugin
        }
    }
}
