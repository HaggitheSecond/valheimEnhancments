using HarmonyLib;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsScreenShakeCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "noscreenshake";

        public override string Description => "Turns off screen shake when hit by an enemy";

        public static bool NoScreenShake { get; set; }

        public override bool GetToggleValue() => valheimEnhancmentsScreenShakeCommand.NoScreenShake;

        public override void SetToggleValue(bool newValue) => valheimEnhancmentsScreenShakeCommand.NoScreenShake = newValue;

        [HarmonyPatch(typeof(GameCamera), "AddShake")]
        private static class valheimEnhancmentsRemoveScreenShakeModification
        {
            private static bool Prefix()
            {
                return valheimEnhancmentsScreenShakeCommand.NoScreenShake == false;
            }
        }
    }
}

