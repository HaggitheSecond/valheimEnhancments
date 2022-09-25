using HarmonyLib;
using UnityEngine;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsNoClipCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "noclip";
        public override string Description => "Allows player clipping into structures";
        public override bool GetToggleValue() => valheimEnhancmentsNoClipCommand.NoClip;
        public override void SetToggleValue(bool newValue)
        {
            valheimEnhancmentsNoClipCommand.NoClip = newValue;

            if (Player.m_localPlayer.InDebugFlyMode() != newValue)
                Player.m_localPlayer.ToggleDebugFly();

            foreach (var currentCollider in Player.m_localPlayer.GetComponentsInChildren<Collider>())
            {
                currentCollider.enabled = newValue == false;
            }
        }

        public static bool NoClip { get; set; }

        [HarmonyPatch(typeof(Player), "EdgeOfWorldKill")]
        private static class valheimEnhancmentsItemDropCanPickupModification
        {
            private static bool Prefix()
            {
                return valheimEnhancmentsNoClipCommand.NoClip == false;
            }
        }
    }
}
