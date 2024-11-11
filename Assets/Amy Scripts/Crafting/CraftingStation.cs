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

    /* ---- UI: DRAGGED ---- */
    public PersonalInventory personalInventory;

    public GameObject waterEmptyImg;
    public GameObject waterFullImg;
    public GameObject ingredient1Slot; // these + below can be clicked
    public GameObject ingredient2Slot;
    public GameObject simpleResultSlot;
    public GameObject upgradeIngr1Slot; // these + below can be null!
    public GameObject upgradeIngr2Slot;
    public GameObject upgradeResultSlot;
    public GameObject decorIngrSlot;
    public GameObject decorResultSlot;

    /* ---- CONSTANTS ---- */
    public enum SlotName
    {
        Personal,
        Ingredient1Key,
        Ingredient2Key,
        SimpleResultKey,
        UpgradeIngr1Key,
        UpgradeIngr2Key,
        UpgradeResultKey,
        DecorIngrKey,
        DecorResultKey,
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
        slotNameToItemKey[SlotName.UpgradeIngr1Key] = "";
        slotNameToItemKey[SlotName.UpgradeIngr2Key] = "";
        slotNameToItemKey[SlotName.UpgradeResultKey] = "";
        slotNameToItemKey[SlotName.DecorIngrKey] = "";
        slotNameToItemKey[SlotName.DecorResultKey] = "";
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
    string LookupItemKey(CraftingStation.SlotName slotName, int personalIndex)
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

    public bool TryPickupThing(PointerEventData eventData, CraftingStation.SlotName slotName, int personalIndex)
    {
        UnityEngine.Debug.Log("OMNOM 3 slotName" + slotName);

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

        UnityEngine.Debug.Log("OMNOM 4 made it here itemKey" + itemKey);

        // UI
        // change image..
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

        // Occupied square
        string itemKey = LookupItemKey(slotName, personalIndex);
        if (itemKey != "")
        {
            UnityEngine.Debug.Log("Clicked on occupied square, do nothing");
            return false;
        }

        // Data


        // UI

        // set status: if it got here, it worked
        StatusController.instance.StopMouseCarrying();
        return true;
    }
}
