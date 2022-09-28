using HarmonyLib;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsNoItemDropsCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "noitemdrops";
        public override string Description => "Removes all item drops";
        public override bool GetToggleValue() => valheimEnhancmentsNoItemDropsCommand.NoItemDrops;
        public override void SetToggleValue(bool newValue) => valheimEnhancmentsNoItemDropsCommand.NoItemDrops = newValue;

        public static bool NoItemDrops { get; set; }


        [HarmonyPatch(typeof(ItemDrop), "TimedDestruction")]
        private static class valheimEnhancmentsItemDropModification
        {
            private static bool Prefix(ref ZNetView ___m_nview)
            {
                if (valheimEnhancmentsNoItemDropsCommand.NoItemDrops && ___m_nview != null)
                {
                    ___m_nview.Destroy();
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ItemDrop), "CanPickup")]
        private static class valheimEnhancmentsItemDropCanPickupModification
        {
            private static bool Prefix(ref bool __result)
            {
                if (valheimEnhancmentsNoItemDropsCommand.NoItemDrops)
                {
                    __result = false;
                    return false;
                }

                return true;
            }
        }
    }
}
