using System.Collections.Generic;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsStuckCommand : valheimEnhancmentsCommand
    {
        public override string Name => "stuck";
        public override string Description => "Unstucks the player by teleporting 50 blocks up";
        public override string Syntax => "";

        public override void Execute(Terminal instance, List<string> arguments)
        {
            var localPlayer = Player.m_localPlayer;

            var currentPosition = localPlayer.transform.position;

            localPlayer.SetGodMode(true);
            localPlayer.TeleportTo(currentPosition + new UnityEngine.Vector3(0, 50, 0),
                localPlayer.transform.rotation,
                false);

            var text = $"Unstuck player {localPlayer.GetPlayerName()}";
            instance.AddString(text);
            ZLog.Log(text);
        }
    }
}
