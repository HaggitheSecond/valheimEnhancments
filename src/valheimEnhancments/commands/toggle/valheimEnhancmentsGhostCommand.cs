namespace valheimEnhancments.commands.toggle
{
    public class valheimEnhancmentsGhostCommand : valheimEnhancmentsToogleCommand
    {
        public override string Name => "ghost";
        public override string Description => "enable ghost mode";
        public override bool GetToggleValue() => Player.m_localPlayer.InGhostMode();
        public override void SetToggleValue(bool newValue) => Player.m_localPlayer.SetGhostMode(newValue);
    }
}
