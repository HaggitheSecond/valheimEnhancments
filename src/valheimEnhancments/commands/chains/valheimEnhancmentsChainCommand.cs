using System.Collections.Generic;

namespace valheimEnhancments.commands.chains
{
    public abstract class valheimEnhancmentsChainCommand : valheimEnhancmentsCommand
    {
        public abstract List<valheimEnhancmentsCommand> Commands { get; }

        public virtual List<string> ModifyArguments(List<string> arguments)
        {
            return arguments;
        }

        public override void Execute(Terminal instance, List<string> arguments)
        {
            arguments = this.ModifyArguments(arguments);

            foreach (var currentCommand in Commands)
            {
                currentCommand.Execute(instance, arguments);
            }

            instance.AddString($"Executed chain command {Name}");
        }
    }
}
