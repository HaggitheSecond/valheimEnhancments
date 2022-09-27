using System;
using System.Collections.Generic;
using System.Linq;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsItemSetCommand : valheimEnhancmentsCommand
    {
        public override string Name => "itemset2";
        public override string Description => "adds itemsets to the players inventory";
        public override string Syntax => "[name]";

        private List<ItemSet> _sets;

        public valheimEnhancmentsItemSetCommand()
        {
            this._sets = new List<ItemSet>
            {
                new ItemSet()
                {
                    Name = "Creative",
                    Items = new List<Item>
                    {
                       new Item("Hammer", equip:true),
                       new Item("Hoe"),
                       new Item("Cultivator"),
                       new Item("HelmetMidsummerCrown", equip:true),
                       new Item("ArmorIronChest", equip:true),
                       new Item("ArmorIronLegs", equip:true),
                       new Item("CapeLinen", equip:true)
                    }
                }
            };
        }

        public override void Execute(Terminal instance, List<string> arguments)
        {
            if (arguments.Count == 0 || string.IsNullOrWhiteSpace(arguments[0]))
            {
                instance.AddString(this.Description);
                return;
            }

            var set = this._sets.FirstOrDefault(f => string.Equals(f.Name, arguments[0], StringComparison.InvariantCultureIgnoreCase));

            if (set == null)
            {
                instance.AddString($"Could not find set with name {arguments[0]}");
                return;
            }

            foreach (var currentItem in set.Items)
            {
                instance.TryRunCommand($"{new valheimEnhancmentsItemCommand().Name} {currentItem.Name} " +
                    $"{(currentItem.Amount.HasValue ? currentItem.Amount.ToString() : string.Empty)} " +
                    $"{(currentItem.Quality.HasValue ? currentItem.Quality.ToString() : string.Empty)} " +
                    $"{(currentItem.Variant.HasValue ? currentItem.Variant.ToString() : string.Empty)} " +
                    $"{currentItem.Equip}");
            }
        }

        private class ItemSet
        {
            public string Name { get; set; }
            public List<Item> Items { get; set; }
        }

        private class Item
        {
            public string Name { get; set; }
            public int? Amount { get; set; }
            public int? Quality { get; set; }
            public int? Variant { get; set; }
            public bool Equip { get; set; }

            public Item(string name, int? amount = null, int? quality = null, int? variant = null, bool equip = false)
            {
                this.Name = name;

                this.Amount = amount;
                this.Quality = quality;
                this.Variant = variant;
                this.Equip = equip;
            }
        }
    }
}
