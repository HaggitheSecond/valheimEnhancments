using System;
using System.Collections;
using System.Collections.Generic;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsItemCommand : valheimEnhancmentsCommand
    {
        public override string Name => "item";
        public override string Description => "adds items to the players inventory";
        public override string Syntax => "[name] [amount] [quality?] [variant?]";

        public override void Execute(Terminal instance, List<string> arguments)
        {
            if (arguments.Count == 0 || string.IsNullOrWhiteSpace(arguments[0]))
            {
                instance.AddString(this.Description);
                return;
            }

            if (this.TryGetItemByName(arguments[0], out var item) == false)
            {
                instance.AddString($"Could not find item with name '{arguments[0]}'.");
                return;
            }

            var player = Player.m_localPlayer;

            if (player == null)
            {
                return;
            }

            int amount;
            if (arguments.Count >= 2 && string.IsNullOrWhiteSpace(arguments[1]) == false)
                amount = int.Parse(arguments[1]);
            else
                amount = item.m_itemData.m_shared.m_maxStackSize;

            int quality;
            if (arguments.Count >= 3 && string.IsNullOrWhiteSpace(arguments[2]) == false)
                quality = int.Parse(arguments[2]);
            else
                quality = item.m_itemData.m_shared.m_maxQuality;

            int variant;
            if (arguments.Count >= 4 && string.IsNullOrWhiteSpace(arguments[3]) == false)
                variant = int.Parse(arguments[3]);
            else
                variant = 0;

            bool equip;
            if (arguments.Count >= 5 && string.IsNullOrWhiteSpace(arguments[4]) == false)
                equip = bool.Parse(arguments[4]);
            else
                equip = false;

            var addedItem = player.GetInventory()
                  .AddItem(item.name,
                      amount,
                      quality,
                      variant,
                      Player.m_localPlayer.GetPlayerID(),
                      Player.m_localPlayer.GetPlayerName());

            if (equip)
                player.EquipItem(addedItem, false);

            instance.AddString($"Added {amount}x {item.name} to {Player.m_localPlayer.GetPlayerName()}'s inventory");
        }

        public bool TryGetItemByName(string name, out ItemDrop item)
        {
            item = null;

            if (string.IsNullOrWhiteSpace(name))
                return false;

            foreach (var currentItemType in Enum.GetValues(typeof(ItemDrop.ItemData.ItemType)) as ItemDrop.ItemData.ItemType[])
            {
                var items = ObjectDB.instance.GetAllItems(currentItemType, string.Empty);

                foreach (var currentActualItem in items)
                {
                    if (string.Equals(currentActualItem.name, name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        item = currentActualItem;

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
