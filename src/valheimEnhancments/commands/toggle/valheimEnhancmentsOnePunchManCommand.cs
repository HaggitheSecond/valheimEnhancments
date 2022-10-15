using HarmonyLib;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsOnePunchVikingCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "onepunchviking";
        public override string Description => "enables onepunchviking mode";
        public override bool GetToggleValue() => valheimEnhancmentsOnePunchVikingCommand.OnePunchViking;
        public override void SetToggleValue(bool newValue) => valheimEnhancmentsOnePunchVikingCommand.OnePunchViking = newValue;

        public static bool OnePunchViking { get; set; }

        [HarmonyPatch(typeof(HitData), "GetTotalDamage")]
        private static class valheimEnhancmentsGetTotalDamageModification
        {
            private static bool Prefix(HitData __instance, ref float __result)
            {
                if (valheimEnhancmentsOnePunchVikingCommand.OnePunchViking == false)
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

