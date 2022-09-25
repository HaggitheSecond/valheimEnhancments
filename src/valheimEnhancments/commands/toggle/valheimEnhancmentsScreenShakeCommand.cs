using HarmonyLib;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsScreenShakeCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "screenshake";

        public override string Description => "Turns off screen shake when hit by an enemy";

        public static bool NoScreenShake { get; set; }

        public override bool GetToggleValue() => NoScreenShake;

        public override void SetToggleValue(bool newValue) => valheimEnhancmentsScreenShakeCommand.NoScreenShake = newValue;

        [HarmonyPatch(typeof(CamShaker), "Trigger")]
        private static bool Prefix(ref bool __result)
        {
            if (valheimEnhancmentsScreenShakeCommand.NoScreenShake)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}

