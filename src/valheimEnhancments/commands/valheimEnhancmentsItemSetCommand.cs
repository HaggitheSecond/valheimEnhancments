using System;
using System.Collections.Generic;
using System.Linq;
using valheimEnhancments.helpers;

namespace valheimEnhancments.commands
{
    public class valheimEnhancmentsItemSetCommand : valheimEnhancmentsCommand
    {
        public override string Name => "itemset2";
        public override string Description => "adds itemsets to the players inventory";
        public override string Syntax => "[name]";

        private List<ItemSet> _sets;

        public override List<string> GetOptions()
        {
            return this._sets.Select(f => f.Name).ToList();
        }

        public valheimEnhancmentsItemSetCommand()
        {
            this._sets = new List<ItemSet>
            {
                new ItemSet()
                {
                    Name = "Creative",
                    Items = new List<ItemSetItem>
                    {
                       new ItemSetItem("Hammer", equip:true),
                       new ItemSetItem("Hoe"),
                       new ItemSetItem("Cultivator"),
                       new ItemSetItem("AxeBlackMetal"),
                       new ItemSetItem("PickaxeIron"),
                       new ItemSetItem("HelmetMidsummerCrown", equip:true),
                       new ItemSetItem("ArmorIronChest", equip:true),
                       new ItemSetItem("ArmorIronLegs", equip:true),
                       new ItemSetItem("CapeLinen", equip:true)
                    }
                },
                new ItemSet()
                {
                    Name = "Food",
                    Items = new List<ItemSetItem>
                    {
                       new ItemSetItem("BloodPudding"),
                       new ItemSetItem("LoxPie"),
                       new ItemSetItem("SerpentStew"),
                    }
                },
                new ItemSet()
                {
                    Name = "Bosstrophies",
                    Items = new List<ItemSetItem>
                    {
                       new ItemSetItem("TrophyEikthyr"),
                       new ItemSetItem("TrophyTheElder"),
                       new ItemSetItem("TrophyBonemass"),
                       new ItemSetItem("TrophyDragonQueen"),
                       new ItemSetItem("TrophyGoblinKing"),
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
                    $"{currentItem.Amount.GetValueOrDefault()} " +
                    $"{currentItem.Quality.GetValueOrDefault()} " +
                    $"{currentItem.Variant.GetValueOrDefault()} " +
                    $"{currentItem.Equip}");
            }
        }

        private class ItemSet
        {
            public string Name { get; set; }
            public List<ItemSetItem> Items { get; set; }
        }

        private class ItemSetItem
        {
            public string Name { get; set; }
            public int? Amount { get; set; }
            public int? Quality { get; set; }
            public int? Variant { get; set; }
            public bool Equip { get; set; }

            public ItemSetItem(string name, int? amount = null, int? quality = null, int? variant = null, bool equip = false)
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
