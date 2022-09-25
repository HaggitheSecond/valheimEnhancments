using HarmonyLib;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsStaminaCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "nostaminadrain";
        public override string Description => "Removes all stamina drain";
        public override bool GetToggleValue() => valheimEnhancmentsStaminaCommand.NoStaminaDrain;
        public override void SetToggleValue(bool newValue)
        {
            Player.m_localPlayer.AddStamina(50f);
            Player.m_localPlayer.m_maxCarryWeight = 50000;

            valheimEnhancmentsStaminaCommand.NoStaminaDrain = newValue;
        }

        public static bool NoStaminaDrain { get; set; }

        [HarmonyPatch(typeof(Player), "UseStamina")]
        private static class valheimEnhancmentsPlayerStaminaModification
        {
            private static bool Prefix()
            {
                return valheimEnhancmentsStaminaCommand.NoStaminaDrain == false;
            }
        }
    }
}
