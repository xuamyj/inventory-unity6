using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingStation : MonoBehaviour
{
    public StatusController.CraftingElemType craftingElemType;
    public StatusController.CraftingSizeType craftingSizeType;

    /* ---- DATA ---- */
    private Dictionary<SlotName, string> slotNameToItemKey;
    private UnityEngine.Sprite BLANK_SPRITE; // init in Awake() but treat like constant

    /* ---- UI: DRAGGED ---- */
    public PersonalInventory personalInventory;

    public GameObject waterEmptyImg;
    public GameObject waterFullImg;
    public UnityEngine.UI.Image ingredient1Slot; // these + below can be clicked
    public UnityEngine.UI.Image ingredient2Slot;
    public UnityEngine.UI.Image simpleResultSlot;
    public UnityEngine.UI.Image decorIngrSlot; // these + below can be null!
    public UnityEngine.UI.Image decorResultSlot;
    public UnityEngine.UI.Image upgradeIngr1Slot;
    public UnityEngine.UI.Image upgradeIngr2Slot;
    public UnityEngine.UI.Image upgradeResultSlot;

    private Dictionary<SlotName, UnityEngine.UI.Image> slotNameToActualSlot;


    /* ---- CONSTANTS ---- */
    public enum SlotName
    {
        Personal,
        Ingredient1Key,
        Ingredient2Key,
        SimpleResultKey,
        DecorIngrKey,
        DecorResultKey,
        UpgradeIngr1Key,
        UpgradeIngr2Key,
        UpgradeResultKey,
    }

    /* ---- STATIC ---- */
    public static CraftingStation instance { get; private set; }
    private void Awake()
    {
        instance = this;

        /* ---- DATA ---- */
        slotNameToItemKey = new Dictionary<SlotName, string>();
        slotNameToItemKey[SlotName.Ingredient1Key] = "";
        slotNameToItemKey[SlotName.Ingredient2Key] = "";
        slotNameToItemKey[SlotName.SimpleResultKey] = "";
        slotNameToItemKey[SlotName.DecorIngrKey] = "";
        slotNameToItemKey[SlotName.DecorResultKey] = "";
        slotNameToItemKey[SlotName.UpgradeIngr1Key] = "";
        slotNameToItemKey[SlotName.UpgradeIngr2Key] = "";
        slotNameToItemKey[SlotName.UpgradeResultKey] = "";

        slotNameToActualSlot = new Dictionary<SlotName, UnityEngine.UI.Image>();
        slotNameToActualSlot[SlotName.Ingredient1Key] = ingredient1Slot;
        slotNameToActualSlot[SlotName.Ingredient2Key] = ingredient2Slot;
        slotNameToActualSlot[SlotName.SimpleResultKey] = simpleResultSlot;
        slotNameToActualSlot[SlotName.DecorIngrKey] = decorIngrSlot;
        slotNameToActualSlot[SlotName.DecorResultKey] = decorResultSlot;
        slotNameToActualSlot[SlotName.UpgradeIngr1Key] = upgradeIngr1Slot;
        slotNameToActualSlot[SlotName.UpgradeIngr2Key] = upgradeIngr2Slot;
        slotNameToActualSlot[SlotName.UpgradeResultKey] = upgradeResultSlot;

        /* ---- UI ---- */
        BLANK_SPRITE = Resources.Load<Sprite>(InventoryConsts.BLANK_SPRITE_URL);
        ingredient1Slot.sprite = BLANK_SPRITE;
        ingredient2Slot.sprite = BLANK_SPRITE;
        simpleResultSlot.sprite = BLANK_SPRITE;
        if (craftingSizeType == StatusController.CraftingSizeType.DecorOnly || craftingSizeType == StatusController.CraftingSizeType.UpgradeDecor)
        {
            decorIngrSlot.sprite = BLANK_SPRITE;
            decorResultSlot.sprite = BLANK_SPRITE;
        }
        if (craftingSizeType == StatusController.CraftingSizeType.UpgradeDecor)
        {
            upgradeIngr1Slot.sprite = BLANK_SPRITE;
            upgradeIngr2Slot.sprite = BLANK_SPRITE;
            upgradeResultSlot.sprite = BLANK_SPRITE;
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

    // private helper function
    private string LookupItemKey(CraftingStation.SlotName slotName, int personalIndex)
    {
        string itemKey = "";
        if (slotName == SlotName.Personal)
        {
            itemKey = personalInventory.GetItemKeyFromIndex(personalIndex);
        }
        else
        {
            itemKey = slotNameToItemKey[slotName];
        }
        return itemKey;
    }

    // private helper function
    private bool IsValidSlot(string carryingItemKey, CraftingStation.SlotName slotName, int personalIndex)
    {
        // assume it's an empty slot (already checked in main function)
        ItemInfo item = InventoryConsts.instance.itemInfoMap[carryingItemKey];
        if (slotName == SlotName.Personal)
        {
            return true;
        }
        if (item.isRaw &&
            (slotName == SlotName.Ingredient1Key
            || slotName == SlotName.Ingredient2Key
            || slotName == SlotName.DecorIngrKey
            || slotName == SlotName.UpgradeIngr1Key
            || slotName == SlotName.UpgradeIngr2Key))
        {
            return true;
        }
        if ((slotName == SlotName.SimpleResultKey) && // also ingredient for decor
            (!item.isRaw && item.canBeDecorated))
        {
            return true;
        }
        if ((slotName == SlotName.DecorResultKey) && // also ingredient for upgrade
            (!item.isRaw && item.canBeUpgraded))
        {
            return true;
        }
        if ((slotName == SlotName.UpgradeResultKey) && // i guess this is storage for crafted item? 
            !item.isRaw)
        {
            return true;
        }
        // otherwise, not a valid combo
        return false;
    }

    // private helper-helper function
    private void CraftingPutDataAndUIHelper(string carryingItemKey, CraftingStation.SlotName slotName)
    {
        // Data
        slotNameToItemKey[slotName] = carryingItemKey;

        // UI
        ItemInfo item = InventoryConsts.instance.itemInfoMap[carryingItemKey];
        string spriteUrl = item.spriteUrl;
        UnityEngine.UI.Image img = slotNameToActualSlot[slotName];
        img.sprite = Resources.Load<Sprite>(spriteUrl);
    }

    // private helper function
    private void PutInValidSlot(string carryingItemKey, CraftingStation.SlotName slotName, int personalIndex)
    {
        // PERSONAL SLOT
        if (slotName == SlotName.Personal)
        {
            // Personal controls its own Data & UI
            // remove from locked slot, add to new slot
            personalInventory.TryRemoveLockedItem(carryingItemKey);
            personalInventory.TryAddItemToSpecificSlot(carryingItemKey, personalIndex);
        }
        // CRAFTING SLOT
        else
        {
            // Personal controls its own Data & UI
            // remove from locked Personal slot
            personalInventory.TryRemoveLockedItem(carryingItemKey);

            // add to Crafting slot
            CraftingPutDataAndUIHelper(carryingItemKey, slotName);
        }
    }


    public bool TryPickupThing(PointerEventData eventData, CraftingStation.SlotName slotName, int personalIndex)
    {
        StatusController.instance.CraftingDebugPrint("OMNOM 3 slotName" + slotName);

        // repeat status check
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();

        if (!(bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Crafting_InWorld && StatusController.instance.GetMouseCarryingBool() == false))
        {
            UnityEngine.Debug.Log("ERROR: CraftingStation.cs > TryPickupThing > something wrong with BigStatus or LittleStatus");
            return false;
        }

        // Blank square
        string itemKey = LookupItemKey(slotName, personalIndex);
        if (itemKey == "")
        {
            UnityEngine.Debug.Log("Clicked on blank square, do nothing");
            return false;
        }

        // Data
        if (slotName == SlotName.Personal) // lock the item, "ToPut" happens at the end
        {
            personalInventory.LockIndex(personalIndex);
        }
        else { } // "ToPut" happens at the end, empty on purpose 

        StatusController.instance.CraftingDebugPrint("OMNOM 4 made it here itemKey" + itemKey);

        // UI
        // gray personalInventory image..

        // change mouseCarrying image..
        string spriteUrl = InventoryConsts.instance.itemInfoMap[itemKey].spriteUrl;
        StatusController.instance.mouseCarryingImageUI.sprite = Resources.Load<Sprite>(spriteUrl);
        // move position to mouse + get it to follow = both are in MouseCarryingController.cs

        // set status: if it got here, it worked
        StatusController.instance.StartMouseCarrying(itemKey);
        return true;
    }

    public bool TryPutThing(PointerEventData eventData, CraftingStation.SlotName slotName, int personalIndex)
    {
        // repeat status check
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();

        if (!(bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Crafting_InWorld && StatusController.instance.GetMouseCarryingBool() == true))
        {
            UnityEngine.Debug.Log("ERROR: CraftingStation.cs > TryPutThing > something wrong with BigStatus or LittleStatus");
            return false;
        }

        string carryingItemKey = StatusController.instance.GetMouseCarryingItemKey();
        string destItemKey = LookupItemKey(slotName, personalIndex);

        // OCCUPIED SLOT
        if (destItemKey != "")
        {
            if (carryingItemKey == destItemKey && personalInventory.GetLockedIndexes().Contains(personalIndex))
            { // orig locked: put it back

                // Data: unlock
                personalInventory.UnlockIndex(personalIndex);

                // UI
                // ungray personalInventory image..

                // don't return here! need to finish function
            }
            else
            { // otherwise occupied: do nothing
                UnityEngine.Debug.Log("Clicked on occupied square, do nothing");
                return false;
            }
        }
        // EMPTY SLOT
        else
        {
            if (IsValidSlot(carryingItemKey, slotName, personalIndex))
            { // valid blank square: put it there

                // Data + UI
                PutInValidSlot(carryingItemKey, slotName, personalIndex);

                // don't return here! need to finish function
            }
            else
            { // not legal place: do nothing
                UnityEngine.Debug.Log("Clicked on empty but not valid square, do nothing");
                return false;
            }
        }
        // clear mouseCarrying image..
        StatusController.instance.mouseCarryingImageUI.sprite = BLANK_SPRITE;

        // set status: if it got here, it worked
        StatusController.instance.StopMouseCarrying();
        return true;
    }
}
