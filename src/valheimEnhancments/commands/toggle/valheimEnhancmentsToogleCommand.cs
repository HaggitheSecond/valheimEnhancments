using System.Collections.Generic;

namespace valheimEnhancments.commands.toggle
{
    public abstract class valheimEnhancmentsToogleCommand : valheimEnhancmentsCommand
    {
        public override string Syntax => this.ToggleSyntax;
        public virtual string ToggleSyntax => "none or [true/false]";

        public abstract bool GetToggleValue();
        public abstract void SetToggleValue(bool newValue);

        public override void Execute(Terminal instance, List<string> arguments)
        {
            bool newValue;

            if (arguments.Count == 1 && bool.TryParse(arguments[0], out var argumentsValue))
                newValue = argumentsValue;
            else
                newValue = this.GetToggleValue() == false;

            this.SetToggleValue(newValue);

            instance.AddString(this.GetChatOutput(newValue));
        }

        public virtual string GetChatOutput(bool newValue)
        {
            return $"Set {this.Name} to {newValue.ToString().ToLowerInvariant()}";
        }
    }
}
