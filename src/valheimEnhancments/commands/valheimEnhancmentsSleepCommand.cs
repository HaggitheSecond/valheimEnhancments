using System.Collections.Generic;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsSleepCommand : valheimEnhancmentsCommand
    {
        public override string Name => "sleep";
        public override string Description => "fast forwards time";
        public override string Syntax => "";

        public override void Execute(Terminal instance, List<string> arguments)
        {
            if (EnvMan.instance == null)
                return;

            EnvMan.instance.SkipToMorning();
        }
    }
}
