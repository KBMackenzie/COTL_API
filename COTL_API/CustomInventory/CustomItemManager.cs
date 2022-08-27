using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using COTL_API.Guid;
using UnityEngine;
using System.Linq;
using HarmonyLib;
using static InventoryItem;
using Lamb.UI;

namespace COTL_API.CustomInventory;

[HarmonyPatch]
public class CustomItemManager
{
    public static Dictionary<InventoryItem.ITEM_TYPE, CustomInventoryItem> customItems = new();

    public static InventoryItem.ITEM_TYPE Add(CustomInventoryItem item)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var itemType = GuidManager.GetEnumValue<InventoryItem.ITEM_TYPE>(guid, item.InternalName);
        item.ItemType = itemType;
        item.ModPrefix = guid;

        customItems.Add(itemType, item);

        return itemType;
    }

    // Patch `ItemInfoCard` not using `InventoryItem`'s method
    [HarmonyPatch(typeof(ItemInfoCard), nameof(ItemInfoCard.Configure))]
    [HarmonyPostfix]
    public static void ItemInfoCard_Configure(ItemInfoCard __instance, InventoryItem.ITEM_TYPE config)
    {
        if (!customItems.ContainsKey(config)) return;

        __instance._inventoryIcon.Configure(config, false);
        __instance._itemHeader.text = InventoryItem.Name(config);
        __instance._itemLore.text = InventoryItem.Lore(config);
        __instance._itemDescription.text = InventoryItem.Description(config);
    }

    [HarmonyPatch(typeof(FontImageNames), nameof(FontImageNames.GetIconByType))]
    [HarmonyPrefix]
    public static bool FontImageNames_GetIconByType(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!customItems.ContainsKey(Type)) return true;
        __result = $"<sprite name=\"icon_{customItems[Type].ModPrefix}.${customItems[Type].InternalName}\">";
        return false;
    }

    [HarmonyPatch(typeof(Lamb.UI.Assets.InventoryIconMapping), nameof(Lamb.UI.Assets.InventoryIconMapping.GetImage), typeof(InventoryItem.ITEM_TYPE))]
    [HarmonyPrefix]
    public static bool InventoryIconMapping_GetImage(InventoryItem.ITEM_TYPE type, ref Sprite __result)
    {
        if (!customItems.ContainsKey(type)) return true;
        __result = customItems[type].InventoryIcon;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), "Name")]
    [HarmonyPrefix]
    public static bool InventoryItem_Name(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!customItems.ContainsKey(Type)) return true;
        __result = customItems[Type].Name();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.LocalizedName))]
    [HarmonyPrefix]
    public static bool InventoryItem_LocalizedName(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!customItems.ContainsKey(Type)) return true;
        __result = customItems[Type].LocalizedName();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Description))]
    [HarmonyPrefix]
    public static bool InventoryItem_Description(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!customItems.ContainsKey(Type)) return true;
        __result = customItems[Type].Description();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.LocalizedDescription))]
    [HarmonyPrefix]
    public static bool InventoryItem_LocalizedDescription(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!customItems.ContainsKey(Type)) return true;
        __result = customItems[Type].LocalizedDescription();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.Lore))]
    [HarmonyPrefix]
    public static bool InventoryItem_Lore(InventoryItem.ITEM_TYPE Type, ref string __result)
    {
        if (!customItems.ContainsKey(Type)) return true;
        __result = customItems[Type].Lore();
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.GetItemCategory))]
    [HarmonyPrefix]
    public static bool InventoryItem_ItemCategory(InventoryItem.ITEM_TYPE type, ref InventoryItem.ITEM_CATEGORIES __result)
    {
        if (!customItems.ContainsKey(type)) return true;
        __result = customItems[type].ItemCategory;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.GetSeedType))]
    [HarmonyPrefix]
    public static bool InventoryItem_GetSeedType(InventoryItem.ITEM_TYPE type, ref InventoryItem.ITEM_TYPE __result)
    {
        if (!customItems.ContainsKey(type)) return true;
        __result = customItems[type].SeedType;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.FuelWeight), typeof(InventoryItem.ITEM_TYPE))]
    [HarmonyPrefix]
    public static bool InventoryItem_FuelWeight(InventoryItem.ITEM_TYPE type, ref int __result)
    {
        if (!customItems.ContainsKey(type)) return true;
        __result = customItems[type].FuelWeight;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.FoodSatitation))]
    [HarmonyPrefix]
    public static bool InventoryItem_FoodSatitation(InventoryItem.ITEM_TYPE Type, ref int __result)
    {
        if (!customItems.ContainsKey(Type)) return true;
        __result = customItems[Type].FoodSatitation;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.IsFish))]
    [HarmonyPrefix]
    public static bool InventoryItem_IsFish(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!customItems.ContainsKey(Type)) return true;
        __result = customItems[Type].IsFish;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.IsFood))]
    [HarmonyPrefix]
    public static bool InventoryItem_IsFood(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!customItems.ContainsKey(Type)) return true;
        __result = customItems[Type].IsFood;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.IsBigFish))]
    [HarmonyPrefix]
    public static bool InventoryItem_IsBigFish(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!customItems.ContainsKey(Type)) return true;
        __result = customItems[Type].IsBigFish;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.CanBeGivenToFollower))]
    [HarmonyPrefix]
    public static bool InventoryItem_CanBeGivenToFollower(InventoryItem.ITEM_TYPE Type, ref bool __result)
    {
        if (!customItems.ContainsKey(Type)) return true;
        __result = customItems[Type].IsBigFish;
        return false;
    }

    [HarmonyPatch(typeof(InventoryItem), nameof(InventoryItem.CapacityString))]
    [HarmonyPrefix]
    public static bool InventoryItem_CapacityString(InventoryItem.ITEM_TYPE type, int minimum, ref string __result)
    {
        if (!customItems.ContainsKey(type)) return true;
        __result = customItems[type].CapacityString(minimum);
        return false;
    }

    [HarmonyPatch(typeof(CookingData), nameof(CookingData.GetAllFoods))]
    public static class CookingData_GetAllFoods_Patch
    {
        static void Postfix(ref InventoryItem.ITEM_TYPE[] __result)
        {
            InventoryItem.ITEM_TYPE[] copy = __result;
            __result = __result.Concat((customItems.Where((i) => !copy.Contains(i.Key) && i.Value.IsFood).Select(i => i.Key))).ToArray();
        }
    }

    [HarmonyPatch(typeof(InventoryMenu))]
    public static class InventoryMenu_Patches
    {
        [HarmonyPatch(nameof(InventoryMenu.OnShowStarted))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> OnShowStarted(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                yield return instruction;

                if (instruction.LoadsField(typeof(InventoryMenu).GetField("_currencyFilter", BindingFlags.NonPublic | BindingFlags.Instance)))
                {
                    yield return new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(() => AppendCustomCurrencies(null)));
                }
            }
        }

        internal static List<InventoryItem.ITEM_TYPE> AppendCustomCurrencies(List<InventoryItem.ITEM_TYPE> currencyFilter)
        {
            return currencyFilter.Concat(customItems.Where((i) => !currencyFilter.Contains(i.Key) && i.Value.IsCurrency).Select(i => i.Key)).ToList();
        }
    }
}
