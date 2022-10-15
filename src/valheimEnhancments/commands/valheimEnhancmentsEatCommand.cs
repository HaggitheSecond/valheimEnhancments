using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using valheimEnhancments.extensions;
using valheimEnhancments.helpers;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsEatCommand : valheimEnhancmentsCommand
    {
        public override string Name => "eat";

        public override string Description => "gives the player the best available food";

        public override string Syntax => "";

        public override void Execute(Terminal instance, List<string> arguments)
        {
            if (Player.m_localPlayer is null)
                return;

            var items = ItemHelper.GetAllItems();
            var foodNames = new List<string>()
            {
                "SerpentStew",
                "LoxPie",
                "BloodPudding"
            };

            var foods = items.Where(f => string.IsNullOrWhiteSpace(f.Item.name) is false
                && f.ItemData != null
                && foodNames.Contains(f.Item.name));

            foreach (var currentFood in foods)
            {
                if (Player.m_localPlayer.GetInventory().AddItem(currentFood.ItemData))
                    Player.m_localPlayer.ConsumeItem(Player.m_localPlayer.GetInventory(), currentFood.ItemData);
            }
        }
    }
}
