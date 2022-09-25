using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using UnityEngine;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsItemClipingCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "noitemclip";
        public override string Description => "Removes all placment checks while building";
        public override bool GetToggleValue() => valheimEnhancmentsItemClipingCommand.NoItemClip;
        public override void SetToggleValue(bool newValue) => valheimEnhancmentsItemClipingCommand.NoItemClip = newValue;

        public static bool NoItemClip { get; set; }

        [HarmonyPatch(typeof(Piece), "CanBeRemoved")]
        private static class valheimEnhacmentsPieceCanBeRemovedCheck
        {
            private static bool Prefix(Piece __instance, ref bool __result)
            {
                if (valheimEnhancmentsItemClipingCommand.NoItemClip == false || __instance == null)
                    return true;

                if (__instance.IsPlacedByPlayer() == false)
                {
                    return true;
                }

                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(Location), "IsInsideNoBuildLocation")]
        private static class valheimEnhancmentsLocationIsInsideNoBuildLocationPatch
        {
            private static bool Prefix(ref bool __result)
            {
                if (valheimEnhancmentsItemClipingCommand.NoItemClip == false)
                    return true;

                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(PrivateArea), "CheckAccess")]
        private static class valheimEnhancmentsPrivateAreaCheckAccessPatch
        {
            private static bool Prefix(ref bool __result)
            {
                if (valheimEnhancmentsItemClipingCommand.NoItemClip == false)
                    return true;

                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(Player), "CheckCanRemovePiece")]
        private static class valheimEnhancmentsPlayerCheckCanRemovePieceePatch
        {
            private static bool Prefix(ref bool __result)
            {
                if (valheimEnhancmentsItemClipingCommand.NoItemClip == false)
                    return true;

                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(StationExtension), "OtherExtensionInRange")]
        private static class valheimEnhancmentsStationExtensionOtherExtensionInRangePatch
        {
            private static bool Prefix(ref bool __result)
            {
                if (valheimEnhancmentsItemClipingCommand.NoItemClip == false)
                    return true;

                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(StationExtension), "StartConnectionEffect", new[] { typeof(CraftingStation) })]
        private static class valheimEnhancmentsStationExtensionStartConnectionEffectPatch
        {
            private static bool Prefix(CraftingStation station)
            {
                if (valheimEnhancmentsItemClipingCommand.NoItemClip == false)
                    return true;

                if (station == null)
                    return false;

                return true;
            }
        }

        [HarmonyPatch(typeof(StationExtension), "StartConnectionEffect", new[] { typeof(Vector3) })]
        private static class valheimEnhancmentsStationExtensionStartConnectionEffect2Patch
        {
            private static bool Prefix(Vector3 targetPos)
            {
                if (valheimEnhancmentsItemClipingCommand.NoItemClip == false)
                    return true;

                if (targetPos == null || targetPos == default(Vector3))
                    return false;

                return true;
            }
        }

        [HarmonyPatch(typeof(Player), "SetPlacementGhostValid")]
        private static class valheimEnhancmentsPlayerUpdatePlacmentPatch
        {
            private static bool Prefix(Player __instance, bool valid)
            {
                if (valheimEnhancmentsItemClipingCommand.NoItemClip == false)
                    return true;

                var placmentGhost = AccessTools.Field(typeof(Player), "m_placementGhost")?.GetValue(__instance) as GameObject;

                if (placmentGhost == null)
                    return true;

                placmentGhost.GetComponent<Piece>().SetInvalidPlacementHeightlight(false);

                AccessTools.Field(typeof(Player), "m_placementStatus")?.SetValue(__instance, 0);

                return false;
            }
        }       
    }
}
