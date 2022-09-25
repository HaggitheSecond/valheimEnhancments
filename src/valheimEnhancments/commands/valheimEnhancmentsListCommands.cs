using System;
using System.Linq;
using System.Reflection;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsListCommands : valheimEnhancmentsCommand
    {
        public override string Name => "list";

        public override string Description => "lists all available commands";

        public override string Syntax => "";

        public override void Execute(Terminal instance, string[] arguments)
        {
            foreach (var item in valheimEnhancmentsPlugin.GetCommands())
            {
                instance.AddString($"{item.Name}: {item.Description} {item.Syntax}");
            }
        }
    }
}
