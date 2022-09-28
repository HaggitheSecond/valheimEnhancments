using HarmonyLib;
using System;
using System.Collections.Generic;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsTestCommand : valheimEnhancmentsCommand
    {
        public override string Name => "test";
        public override string Description => "testcommand";
        public override string Syntax => "";

        public valheimEnhancmentsTestCommand()
        {
            this.NeedsLocalPlayer = false;
        }

        public override void Execute(Terminal instance, List<string> arguments)
        {
            instance.AddString("Test is working!");
        }
    }
}
