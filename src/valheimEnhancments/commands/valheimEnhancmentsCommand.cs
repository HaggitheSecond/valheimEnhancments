using System;
using valheimEnhancments.extensions;

namespace valheimEnhancments.commands
{
    public abstract class valheimEnhancmentsCommand
    {
        public abstract string Name { get; }

        public abstract string Description { get; }
        public abstract string Syntax { get; }
        public abstract void Execute(Terminal instance, string[] arguments);

        public override string ToString()
        {
            return $"{this.Name}: {this.Description}{Environment.NewLine}{" ".Repeat(this.Name.Length+2)}/{this.Name} {this.Syntax}";
        }
    }
}
