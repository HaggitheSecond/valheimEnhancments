using HarmonyLib;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsRavenCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "noraven";

        public override string Description => "Turns off the hint raven";

        public static bool NoRaven { get; set; }

        public override bool GetToggleValue() => valheimEnhancmentsRavenCommand.NoRaven;

        public override void SetToggleValue(bool newValue) => valheimEnhancmentsRavenCommand.NoRaven = newValue;

        [HarmonyPatch(typeof(Raven), "CheckSpawn")]
        private static class valheimEnhancmentsRemoveRavenModification
        {
            private static bool Prefix()
            {
                return valheimEnhancmentsRavenCommand.NoRaven == false;
            }
        }
    }
}

