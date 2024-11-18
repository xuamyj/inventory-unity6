using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MCraftingStation : MonoBehaviour
{
    public StatusController.CraftingElemType craftingElemType;
    public StatusController.CraftingSizeType craftingSizeType;

    /* ---- DATA ---- */
    private Dictionary<CraftingSlotName, string> slotNameToItemKey;

    /* ---- UI: DRAGGED ---- */
    // water
    public GameObject waterEmptyImg;
    public GameObject waterFullImg;
    // slots
    private Dictionary<CraftingSlotName, UnityEngine.UI.Image> slotNameToActualSlot;
    public UnityEngine.UI.Image ingredient1Slot;
    public UnityEngine.UI.Image ingredient2Slot;
    public UnityEngine.UI.Image simpleResultSlot;
    public UnityEngine.UI.Image decorIngrSlot; // these + below can be null!
    public UnityEngine.UI.Image decorResultSlot;
    public UnityEngine.UI.Image upgradeIngr1Slot;
    public UnityEngine.UI.Image upgradeIngr2Slot;
    public UnityEngine.UI.Image upgradeResultSlot;

    /* ---- STATIC ---- */
    private void Awake()
    {
        /* ---- DATA ---- */
        slotNameToItemKey = new Dictionary<CraftingSlotName, string>();
        slotNameToItemKey[CraftingSlotName.Ingredient1Key] = "";
        slotNameToItemKey[CraftingSlotName.Ingredient2Key] = "";
        slotNameToItemKey[CraftingSlotName.SimpleResultKey] = "";
        slotNameToItemKey[CraftingSlotName.DecorIngrKey] = "";
        slotNameToItemKey[CraftingSlotName.DecorResultKey] = "";
        slotNameToItemKey[CraftingSlotName.UpgradeIngr1Key] = "";
        slotNameToItemKey[CraftingSlotName.UpgradeIngr2Key] = "";
        slotNameToItemKey[CraftingSlotName.UpgradeResultKey] = "";

        /* ---- UI ---- */
        slotNameToActualSlot = new Dictionary<CraftingSlotName, UnityEngine.UI.Image>();
        slotNameToActualSlot[CraftingSlotName.Ingredient1Key] = ingredient1Slot;
        slotNameToActualSlot[CraftingSlotName.Ingredient2Key] = ingredient2Slot;
        slotNameToActualSlot[CraftingSlotName.SimpleResultKey] = simpleResultSlot;
        slotNameToActualSlot[CraftingSlotName.DecorIngrKey] = decorIngrSlot;
        slotNameToActualSlot[CraftingSlotName.DecorResultKey] = decorResultSlot;
        slotNameToActualSlot[CraftingSlotName.UpgradeIngr1Key] = upgradeIngr1Slot;
        slotNameToActualSlot[CraftingSlotName.UpgradeIngr2Key] = upgradeIngr2Slot;
        slotNameToActualSlot[CraftingSlotName.UpgradeResultKey] = upgradeResultSlot;

        ingredient1Slot.sprite = StatusController.instance.BLANK_SPRITE;
        ingredient2Slot.sprite = StatusController.instance.BLANK_SPRITE;
        simpleResultSlot.sprite = StatusController.instance.BLANK_SPRITE;
        if (craftingSizeType == StatusController.CraftingSizeType.DecorOnly || craftingSizeType == StatusController.CraftingSizeType.UpgradeDecor)
        {
            decorIngrSlot.sprite = StatusController.instance.BLANK_SPRITE;
            decorResultSlot.sprite = StatusController.instance.BLANK_SPRITE;
        }
        if (craftingSizeType == StatusController.CraftingSizeType.UpgradeDecor)
        {
            upgradeIngr1Slot.sprite = StatusController.instance.BLANK_SPRITE;
            upgradeIngr2Slot.sprite = StatusController.instance.BLANK_SPRITE;
            upgradeResultSlot.sprite = StatusController.instance.BLANK_SPRITE;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetItemKeyFromSlot(CraftingSlotName slotName)
    {
        return slotNameToItemKey[slotName];
    }

    // these are currently private / called ..Helper because they don't check if the space is empty / item is present, they just do the thing
    private void AddItemToSlotHelper(CraftingSlotName slotName, string itemKey)
    {
        // Data
        slotNameToItemKey[slotName] = itemKey;

        // UI
        slotNameToActualSlot[slotName].sprite = ItemConsts.instance.GetAndLoadSpriteUrl(itemKey);
    }
    private void RemoveItemFromSlotHelper(CraftingSlotName slotName)
    {
        // Data
        slotNameToItemKey[slotName] = "";

        // UI
        slotNameToActualSlot[slotName].sprite = StatusController.instance.BLANK_SPRITE;
    }

    public bool IsValidCraftingSlotForItem(CraftingSlotName slotName, string itemKey)
    {
        // note, this only checks validity, not if it's an empty slot
        ItemInfo item = ItemConsts.instance.itemInfoMap[itemKey];

        if ((slotName == CraftingSlotName.Ingredient1Key
            || slotName == CraftingSlotName.Ingredient2Key
            || slotName == CraftingSlotName.DecorIngrKey
            || slotName == CraftingSlotName.UpgradeIngr1Key
            || slotName == CraftingSlotName.UpgradeIngr2Key) &&
            (item.isRaw))
        {
            return true;
        }
        if ((slotName == CraftingSlotName.SimpleResultKey) && // also ingredient for decor
            (!item.isRaw && item.canBeDecorated))
        {
            return true;
        }
        if ((slotName == CraftingSlotName.DecorResultKey) && // also ingredient for upgrade
            (!item.isRaw && item.canBeUpgraded))
        {
            return true;
        }
        if ((slotName == CraftingSlotName.UpgradeResultKey) && // i guess this is storage for any crafted item? 
            !item.isRaw)
        {
            return true;
        }
        // otherwise, not a valid combo
        return false;
    }

    public bool TryRemoveItemFromSlot(CraftingSlotName slotName)
    {
        // in theory, well.. for now, this function always runs, in the future could add code to take care of StatusController

        if (slotNameToItemKey[slotName] != "")
        {
            RemoveItemFromSlotHelper(slotName);
            return true;
        }
        return false;
    }

    public bool TryAddItemToSpecificSlot(CraftingSlotName slotName, string itemKey)
    {
        // in theory, well.. for now, this function always runs, in the future could add code to take care of StatusController

        AllInventoryController.instance.PersonalInventoryDebugPrint("Hit MCraftingStation.cs: TryAddItemToSpecificSlot( " + slotName + " , " + itemKey + " )");

        if ((slotNameToItemKey[slotName] == "") &&
            IsValidCraftingSlotForItem(slotName, itemKey))
        {
            AddItemToSlotHelper(slotName, itemKey);

            AllInventoryController.instance.PersonalInventoryDebugPrint("True! slotName " + slotName);
            return true;
        }
        AllInventoryController.instance.PersonalInventoryDebugPrint("False!");
        return false;
    }
}
