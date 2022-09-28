using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace valheimEnhancments.helpers
{
    public static class ItemHelper
    {
        public static List<ItemExtended> GetAllItems(bool onlyWithRecipe = true)
        {
            var items = new List<ItemExtended>();

            foreach (var currentItemType in Enum.GetValues(typeof(ItemDrop.ItemData.ItemType)) as ItemDrop.ItemData.ItemType[])
            {
                var itemsForType = ObjectDB.instance.GetAllItems(currentItemType, string.Empty);

                foreach (var currentItem in itemsForType)
                {
                    var recipe = ObjectDB.instance.GetRecipe(currentItem.m_itemData);

                    var shared = currentItem.m_itemData.m_shared;

                    string description;
                    if (shared.m_description.Contains("$"))
                    {
                        description = Localization.instance.Localize(shared.m_description.Substring(shared.m_description.IndexOf("$")));
                    }
                    else
                    {
                        description = shared.m_description;
                    }

                    var hasRecipe = recipe != null;

                    if (onlyWithRecipe == false && hasRecipe == false)
                        continue;

                    items.Add(new ItemExtended
                    {
                        Item = currentItem,
                        Description = description,
                        Recipe = recipe,
                        Type = currentItemType
                    });
                }
            }

            return items;
        }

        public class ItemExtended
        {
            public ItemDrop Item { get; set; }
            public ItemDrop.ItemData ItemData => this.Item.m_itemData;
            public ItemDrop.ItemData.SharedData SharedData => this.ItemData.m_shared;

            public string Description { get; set; }

            public Recipe Recipe { get; set; }
            public ItemDrop.ItemData.ItemType Type { get; set; }
        }
    }
}
