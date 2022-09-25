using HarmonyLib;

namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsBuildSupportCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "nobuildsupport";
        public override string Description => "Removes the need for builds to have a support structure";

        public override bool GetToggleValue() => NoSupportCheck;
        public override void SetToggleValue(bool newValue) => valheimEnhancmentsBuildSupportCommand.NoSupportCheck = newValue;

        public static bool NoSupportCheck { get; set; }

        [HarmonyPatch(typeof(WearNTear), "UpdateSupport")]
        private static class valheimEnhancmentsRemoveSupportCheckModification
        {
            private static void Postfix(ref float ___m_support, ref ZNetView ___m_nview)
            {
                if (valheimEnhancmentsBuildSupportCommand.NoSupportCheck && ___m_nview != null)
                {
                    ___m_support = 500f;
                    ___m_nview.GetZDO().Set("support", ___m_support);
                }
            }
        }
    }
}

