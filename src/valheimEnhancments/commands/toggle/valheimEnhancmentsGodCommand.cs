namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsGodCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "god";
        public override string Description => "enable god mode";
        public override bool GetToggleValue() => Player.m_localPlayer.InGodMode();
        public override void SetToggleValue(bool newValue) => Player.m_localPlayer.SetGodMode(newValue);
    }
}

