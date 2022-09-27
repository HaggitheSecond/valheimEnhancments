using System.Collections.Generic;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsListCommands : valheimEnhancmentsCommand
    {
        public override string Name => "list";

        public override string Description => "lists all available commands";

        public override string Syntax => "";

        public override void Execute(Terminal instance, List<string> arguments)
        {
            foreach (var item in valheimEnhancmentsPlugin.GetCommands())
            {
                instance.AddString($"{item.Name}: {item.Description} {item.Syntax}");
            }
        }
    }
}
