using HarmonyLib;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsSlideCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "noslide";
        public override string Description => "Removes all stamina drain";
        public override bool GetToggleValue() => valheimEnhancmentsSlideCommand.NoSlideAngle;
        public override void SetToggleValue(bool newValue) => valheimEnhancmentsSlideCommand.NoSlideAngle = newValue;

            public static bool NoSlideAngle { get; private set; }

        [HarmonyPatch(typeof(Character), "GetSlideAngle")]
        private static class valheimEnhancmentsCharacterSlideModification
        {
            private static bool Prefix(Character __instance, ref float __result)
            {
                if (__instance == null || __instance.IsPlayer() == false || valheimEnhancmentsSlideCommand.NoSlideAngle == false)
                    return true;

                __result = 5000f;
                return false;
            }
        }
    }
}
