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

        public override void Execute(Terminal instance, string[] arguments)
        {
            instance.AddString("Test is working!");
        }
    }
}
