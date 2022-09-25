
using HarmonyLib;
using System.Linq;

namespace valheimEnhancments.fixes
{
    [HarmonyPatch(typeof(InventoryGrid), "UpdateGui")]
    public static class valheimEnhancmentsInvalidItemInInventoryUpdateGuiFix
    {
        private static bool Prefix(InventoryGrid __instance, Player player, ItemDrop.ItemData dragItem)
        {
            var inventory = player.GetInventory();

            foreach (var currentItem in inventory.GetAllItems())
            {
                if (currentItem.m_shared.m_name == "Bow")
                {
                    inventory.RemoveItem(currentItem);
                    ZLog.Log("removed invalid item - Bow");
                }
            }   
            
            return true;
        }
    }
}
