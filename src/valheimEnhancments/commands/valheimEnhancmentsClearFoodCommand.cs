using System.Collections.Generic;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsClearFoodCommand : valheimEnhancmentsCommand
    {
        public override string Name => "clearfood";

        public override string Description => "removes all eaten food";

        public override string Syntax => "";

        public override void Execute(Terminal instance, List<string> arguments)
        {
            Player.m_localPlayer.ClearFood();
        }
    }
}
