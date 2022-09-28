using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using valheimEnhancments.extensions;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsDebugModeCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "debug";
        public override string Description => "Toggles debug mode";
        public override bool GetToggleValue() => Player.m_debugMode;
        public override void SetToggleValue(bool newValue)
        {
            Player.m_debugMode = newValue;

            // we can only toggle this, so if it's already in the correct position we do nothing
            if (newValue == Player.m_localPlayer.NoCostCheat())
                return;

            Player.m_localPlayer.ToggleNoPlacementCost();
            Player.m_localPlayer.ClearHardDeath();
            Player.m_localPlayer.GetSEMan().RemoveAllStatusEffects(true);

            InventoryGui.instance.m_craftDuration = newValue ? 0.2f : 2f;
        }

        public override string GetChatOutput(bool newValue)
        {
            return base.GetChatOutput(newValue) + " - [Z] fly | [K] kill enemies | [B] no placment costs";
        }

        [HarmonyPatch(typeof(Player), "HaveRequirements", new[] { typeof(Piece), typeof(Player.RequirementMode) })]
        private static class valheimEnhancmentsPlayerHaveRequirementsModification
        {
            private static bool Prefix(ref bool __result, Piece piece, Player.RequirementMode mode)
            {
                if (Player.m_localPlayer == null || Player.m_localPlayer.NoCostCheat() == false)
                    return true;

                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(Player), "HaveRequirements", new[] { typeof(Recipe), typeof(bool), typeof(int) })]
        private static class valheimEnhancmentsPlayerHaveRequirements2Modification
        {
            private static bool Prefix(ref bool __result)
            {
                if (Player.m_localPlayer == null || Player.m_localPlayer.NoCostCheat() == false)
                    return true;

                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(StoreGui), "GetPlayerCoins")]
        private static class valheimEnhancmentsStoreGuidGetPlayerCoinsModification
        {
            private static bool Prefix(ref int __result)
            {
                if (Player.m_localPlayer == null || Player.m_localPlayer.NoCostCheat() == false)
                    return true;

                __result = 9999;
                return false;
            }
        }

        [HarmonyPatch(typeof(ItemStand), "UpdateAttach")]
        private static class valheimEnhancmentsItemStandUpdateAttachModification
        {
            // the goal here is simple: 
            // when adding an item to an ItemStand do NOT remove it from the inventory of the player
            // inside this method, UnequipItem and RemoveOneItem are called
            // so we simply ignore those methods if we're inside UpdateAttach

            public static bool IgnoreItemRemovals { get; private set; }

            private static bool Prefix()
            {
                if (Player.m_localPlayer != null && Player.m_localPlayer.NoCostCheat())
                {
                    IgnoreItemRemovals = true;
                }

                return true;
            }

            private static void Postfix()
            {
                IgnoreItemRemovals = false;
            }
        }

        [HarmonyPatch(typeof(Humanoid), "UnequipItem")]
        private static class valheimEnhancmentsHumanoidUnequipItemModification
        {
            private static bool Prefix()
            {
                return valheimEnhancmentsItemStandUpdateAttachModification.IgnoreItemRemovals == false;
            }
        }

        [HarmonyPatch(typeof(Inventory), "RemoveOneItem")]
        private static class valheimEnhancmentsInventoryRemoveOneItemModification
        {
            private static bool Prefix()
            {
                return valheimEnhancmentsItemStandUpdateAttachModification.IgnoreItemRemovals == false;
            }
        }
    }
}
