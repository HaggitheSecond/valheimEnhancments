using HarmonyLib;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsDurabilityCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "nodurabilitydrain";
        public override string Description => "Removes durability drain from tools";
        public override bool GetToggleValue() => valheimEnhancmentsDurabilityCommand.NoDurabilityDrain;
        public override void SetToggleValue(bool newValue) => valheimEnhancmentsDurabilityCommand.NoDurabilityDrain = newValue;

        public static bool NoDurabilityDrain { get; set; }

        [HarmonyPatch(typeof(Player), "DamageArmorDurability")]
        private static class valheimEnhancmentsPlayerDamageArmorDurabilityModification
        {
            private static bool Prefix()
            {
                return valheimEnhancmentsDurabilityCommand.NoDurabilityDrain == false;
            }
        }

        [HarmonyPatch(typeof(Player), "UpdatePlacement")]
        private static class valheimEnhancmentsUpdatePlacementModification
        {
            private static bool? _useDurability;

            private static bool Prefix()
            {
                if (Player.m_localPlayer == null || valheimEnhancmentsDurabilityCommand.NoDurabilityDrain == false)
                    return true;

                var rightItem = Player.m_localPlayer.GetRightItem();

                if (rightItem != null && rightItem.m_shared.m_useDurability)
                {
                    _useDurability = rightItem.m_shared.m_useDurability;
                    rightItem.m_shared.m_useDurability = false;
                }

                return true;
            }

            private static void Postfix()
            {
                if (Player.m_localPlayer == null || valheimEnhancmentsDurabilityCommand.NoDurabilityDrain == false)
                    return;

                var rightItem = Player.m_localPlayer.GetRightItem();

                if (rightItem != null && _useDurability.HasValue)
                {
                    rightItem.m_shared.m_useDurability = _useDurability.Value;
                }

                _useDurability = null;
            }
        }

        [HarmonyPatch(typeof(Attack), "OnAttackTrigger")]
        private static class valheimEnhancmentsAttackOnAttackTriggerModification
        {
            private static bool? _useDurability;

            private static bool Prefix(ItemDrop.ItemData ___m_weapon)
            {
                if (___m_weapon == null || valheimEnhancmentsDurabilityCommand.NoDurabilityDrain == false)
                    return true;

                if (___m_weapon.m_shared.m_useDurability)
                {
                    _useDurability = ___m_weapon.m_shared.m_useDurability;
                    ___m_weapon.m_shared.m_useDurability = false;
                }

                return true;
            }

            private static void Postfix(ItemDrop.ItemData ___m_weapon)
            {
                if (___m_weapon == null || valheimEnhancmentsDurabilityCommand.NoDurabilityDrain == false)
                    return;

                if (_useDurability.HasValue)
                {
                    ___m_weapon.m_shared.m_useDurability = _useDurability.Value;
                }

                _useDurability = null;
            }
        }
    }
}
