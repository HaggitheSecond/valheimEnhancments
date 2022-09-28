using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using valheimEnhancments.helpers;
using static Mono.Security.X509.X520;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsItemCommand : valheimEnhancmentsCommand
    {
        public override string Name => "item";
        public override string Description => "adds items to the players inventory";
        public override string Syntax => "[name] [amount] [quality?] [variant?] [autoequip?]";

        public override List<string> GetOptions()
        {
            return ItemHelper.GetAllItems().Select(f => f.Item.name).ToList();
        }

        public override void Execute(Terminal instance, List<string> arguments)
        {
            if (arguments.Count == 0 || string.IsNullOrWhiteSpace(arguments[0]))
            {
                instance.AddString(this.Description);
                return;
            }

            var item = this.GetItemByName(arguments[0]);
            if (item == null)
            {
                instance.AddString($"Could not find item with name '{arguments[0]}'.");
                return;
            }

            var player = Player.m_localPlayer;

            if (player == null)
                return;

            var amount = GetIntValueFromArgument(arguments.ElementAtOrDefault(1), item.m_itemData.m_shared.m_maxStackSize);
            var quality = GetIntValueFromArgument(arguments.ElementAtOrDefault(2), item.m_itemData.m_shared.m_maxQuality);
            var variant = GetIntValueFromArgument(arguments.ElementAtOrDefault(3), 0);
            var equip = false;

            if (arguments.ElementAtOrDefault(4) != null && bool.TryParse(arguments.ElementAtOrDefault(4), out var equip2))
                equip = equip2;

            var addedItem = player.GetInventory()
                  .AddItem(item.name,
                      amount,
                      quality,
                      variant,
                      Player.m_localPlayer.GetPlayerID(),
                      Player.m_localPlayer.GetPlayerName());

            if (equip && addedItem.IsEquipable())
                player.EquipItem(addedItem, false);

            instance.AddString($"Added {amount}x {item.name} with quality {quality} and variant {variant} to {Player.m_localPlayer.GetPlayerName()}'s inventory" + (equip ? ", and equiped it." : string.Empty));

            int GetIntValueFromArgument(string argument, int defaultValue)
            {
                if (string.IsNullOrWhiteSpace(argument))
                    return defaultValue;

                if (int.TryParse(argument, out var returnValue) == false)
                    return defaultValue;

                if (returnValue == 0)
                    return defaultValue;

                return returnValue;
            }
        }

        public ItemDrop GetItemByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return ItemHelper.GetAllItems().FirstOrDefault(f => string.Equals(f.Item.name, name, StringComparison.InvariantCultureIgnoreCase))?.Item;
        }
    }
}
