using HarmonyLib;
using System.Collections.Generic;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsAllowUnderwaterCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "allowunderwater";

        public override string Description => "Turns off unequip on swimming";

        public static bool AllowUnderWaterActions { get; set; }

        public override bool GetToggleValue() => valheimEnhancmentsAllowUnderwaterCommand.AllowUnderWaterActions;

        public override void SetToggleValue(bool newValue) => valheimEnhancmentsAllowUnderwaterCommand.AllowUnderWaterActions = newValue;

        [HarmonyPatch(typeof(Character), "IsSwiming")]
        private static class valheimEnhancmentsAllowUnderWaterActionsIsSwimingModification
        {
            private static bool Prefix(Character __instance)
            {
                if (__instance == null || __instance.IsPlayer() == false)
                    return true;

                return valheimEnhancmentsAllowUnderwaterCommand.AllowUnderWaterActions == false;
            }
        }

        [HarmonyPatch(typeof(Character), "InLiquidDepth")]
        private static class valheimEnhancmentsAllowUnderWaterActionsInLiquidDepthModification
        {
            private static bool Prefix(Character __instance, ref float __result)
            {
                if (__instance == null || __instance.IsPlayer() == false)
                    return true;

                if(valheimEnhancmentsAllowUnderwaterCommand.AllowUnderWaterActions)
                {
                    __result = 0f;
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(GameCamera), "GetCameraPosition")]
        private static class valheimEnhancmentsAllowUnderWaterGameCameraModification
        {
            public static bool IsGettingCameraPosition { get; private set; }

            private static bool Prefix()
            {
                if (valheimEnhancmentsAllowUnderwaterCommand.AllowUnderWaterActions)
                    IsGettingCameraPosition = true;

                return true;
            }

            private static void Postfix()
            {
                IsGettingCameraPosition = false;
            }
        }

        [HarmonyPatch(typeof(Fireplace), "IsBurning")]
        private static class valheimEnhancmentsAllowUnderWaterFireModification
        {
            public static bool IsGettingIsBurning { get; private set; }

            private static bool Prefix()
            {
                if (valheimEnhancmentsAllowUnderwaterCommand.AllowUnderWaterActions)
                    IsGettingIsBurning = true;

                return true;
            }

            private static void Postfix()
            {
                IsGettingIsBurning = false;
            }
        }

        [HarmonyPatch(typeof(Floating), "GetLiquidLevel")]
        private static class valheimEnhancmentsAllowUnderWaterFloatingModification
        {
            private static bool Prefix(ref float __result)
            {
                if (valheimEnhancmentsAllowUnderWaterGameCameraModification.IsGettingCameraPosition == false 
                    && valheimEnhancmentsAllowUnderWaterFireModification.IsGettingIsBurning == false)
                    return true;

                __result = -10000f;
                return false;
            }
        }
    }
}

