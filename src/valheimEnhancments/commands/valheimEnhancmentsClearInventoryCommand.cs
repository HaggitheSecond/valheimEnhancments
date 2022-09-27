using HarmonyLib;
using System.Collections.Generic;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsClearInventoryCommand : valheimEnhancmentsCommand
    {
        public override string Name => "clearinventory";
        public override string Description => "removes all items from the players inventory";
        public override string Syntax => "";

        public override void Execute(Terminal instance, List<string> arguments)
        {
            if (Player.m_localPlayer == null)
                return;

            var inventory = Player.m_localPlayer.GetInventory();

            if (inventory == null)
                return;

            Player.m_localPlayer.UnequipAllItems();
            inventory?.RemoveAll();
            InventoryGui.instance.m_playerGrid.UpdateInventory(inventory, Player.m_localPlayer, null);
        }
    }
}
