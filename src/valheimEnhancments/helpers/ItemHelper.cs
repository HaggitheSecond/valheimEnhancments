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

        public static List<(string property, string value)> GetPropertyValues(ItemExtended item)
        {
            var list = new List<(string property, string value)>();

            if (item is null)
            {
                list.Add(("item", "null"));
                return list;
            }

            var itemPropertyPath = "item.item";
            if(item.Item is null)
            {
                list.Add((itemPropertyPath, "null"));
            }
            else
            {
                list.Add((itemPropertyPath + "name", item.Item.name));
                list.Add((itemPropertyPath + "hovername", item.Item.GetHoverName()));
                list.Add((itemPropertyPath + "hovertext", item.Item.GetHoverText()));

                var itemItemDataPropertyPath = itemPropertyPath + "ItemData.";
                if (item.ItemData is null)
                {
                    list.Add((itemItemDataPropertyPath, "null"));
                }
                else
                {
                    list.Add((itemItemDataPropertyPath + "durability", item.ItemData.m_durability.ToString()));
                    list.Add((itemItemDataPropertyPath + "stack", item.ItemData.m_stack.ToString()));
                    list.Add((itemItemDataPropertyPath + "quality", item.ItemData.m_quality.ToString()));
                    list.Add((itemItemDataPropertyPath + "variant", item.ItemData.m_variant.ToString()));
                    list.Add((itemItemDataPropertyPath + "crafterId", item.ItemData.m_crafterID.ToString()));
                    list.Add((itemItemDataPropertyPath + "crafterName", item.ItemData.m_crafterName.ToString()));
                    list.Add((itemItemDataPropertyPath + "isEquipable", item.ItemData.IsEquipable().ToString()));
                    list.Add((itemItemDataPropertyPath + "isWeapon", item.ItemData.IsWeapon().ToString()));
                    list.Add((itemItemDataPropertyPath + "tooltip", item.ItemData.GetTooltip().ToString()));

                    var itemSharedPropertyPath = itemPropertyPath + "Shared.";
                    if(item.ItemData.m_shared is null)
                    {
                        list.Add((itemSharedPropertyPath, "null"));
                    }
                    else
                    {
                        list.Add((itemSharedPropertyPath + "name", item.SharedData.m_name));
                        list.Add((itemSharedPropertyPath + "dlc", item.SharedData.m_dlc));
                        list.Add((itemSharedPropertyPath + "value", item.SharedData.m_value.ToString()));
                        list.Add((itemSharedPropertyPath + "m_teleportable", item.SharedData.m_teleportable.ToString()));

                        list.Add((itemSharedPropertyPath + "food", item.SharedData.m_food.ToString()));
                        list.Add((itemSharedPropertyPath + "foodStamina", item.SharedData.m_foodStamina.ToString()));
                        list.Add((itemSharedPropertyPath + "foodBurnTime", item.SharedData.m_foodBurnTime.ToString()));
                        list.Add((itemSharedPropertyPath + "foodRegen", item.SharedData.m_foodRegen.ToString()));

                        var itemSharedStatusEffectPropertyPath = itemSharedPropertyPath + "StatusEffect.";
                        if (item.SharedData.m_consumeStatusEffect is null)
                        {
                            list.Add((itemSharedStatusEffectPropertyPath, "null"));
                        }
                        else
                        {
                            list.Add((itemSharedStatusEffectPropertyPath + "name", item.SharedData.m_consumeStatusEffect.name));
                            list.Add((itemSharedStatusEffectPropertyPath + "name", item.SharedData.m_consumeStatusEffect.name));
                            list.Add((itemSharedStatusEffectPropertyPath + "tooltip", item.SharedData.m_consumeStatusEffect.GetTooltipString()));
                        }
                    }
                }
            }

            var recipePropertyPath = "recipe.";
            if(item.Recipe is null)
            {
                list.Add((recipePropertyPath, "null"));
            }
            else
            {
                list.Add((recipePropertyPath + "name", item.Recipe.name)); 
                list.Add((recipePropertyPath + "amount", item.Recipe.m_amount.ToString()));
                list.Add((recipePropertyPath + "craftingstation", item.Recipe.m_craftingStation?.name));
            }

            list.Add(("description", item.Description));
            list.Add(("itemType", item.Type.ToString()));

            return list;
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
