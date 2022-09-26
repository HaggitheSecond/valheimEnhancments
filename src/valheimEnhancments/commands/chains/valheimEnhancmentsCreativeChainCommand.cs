using System;
using System.Collections.Generic;
using valheimEnhancments.commands.toggle;

namespace valheimEnhancments.commands.chains
{
    public class valheimEnhancmentsCreativeChainCommand : valheimEnhancmentsChainCommand
    {
        public override string Name => "gamemode";
        public override string Description => "several commands to enable creative mode";
        public override string Syntax => "none or [true/false/creative/survival]";

        public override List<valheimEnhancmentsCommand> Commands => new List<valheimEnhancmentsCommand>
        {
            new valheimEnhnacmnetsDebugModeCommand(),
            new valheimEnhancmentsGodCommand(),
            new valheimEnhancmentsGhostCommand(),
            new valheimEnhancmentsStaminaCommand(),
            new valheimEnhancmentsNoItemDropsCommand(),
            new valheimEnhancmentsDurabilityCommand(),
            new valheimEnhancmentsItemClipingCommand(),
            new valheimEnhancmentsWeatherCommand(),
            new valheimEnhancmentsScreenShakeCommand(),
            new valheimEnhancmentsRavenCommand()
        };

        public override string[] ModifyArguments(string[] arguments)
        {
            if (arguments.Length != 1)
                return this._defaultReturnValue;

            var argument = arguments[0] ?? string.Empty;

            if (string.Equals("false", argument, StringComparison.InvariantCultureIgnoreCase) || string.Equals("survival", argument, StringComparison.InvariantCultureIgnoreCase))
            {
                return new[]
                {
                    "false"
                };
            }

            return this._defaultReturnValue;
        }

        public string[] _defaultReturnValue => new[]
                {
                    "true"
                };
    }
}
