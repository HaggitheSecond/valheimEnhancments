using HarmonyLib;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsOnePunchManCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "onepunchviking";
        public override string Description => "enables one punchman mode";
        public override bool GetToggleValue() => valheimEnhancmentsOnePunchManCommand.OnePunchMan;
        public override void SetToggleValue(bool newValue) => valheimEnhancmentsOnePunchManCommand.OnePunchMan = newValue;

        public static bool OnePunchMan { get; set; }

        [HarmonyPatch(typeof(HitData), "GetTotalDamage")]
        private static class valheimEnhancmentsGetTotalDamageModification
        {
            private static bool Prefix(HitData __instance, ref float __result)
            {
                if (valheimEnhancmentsOnePunchManCommand.OnePunchMan == false)
                    return true;

                var attacker = __instance.GetAttacker();

                if (attacker == null || attacker.IsPlayer() == false)
                    return true;

                __result = 99999f;
                return false;
            }
        }
    }
}

