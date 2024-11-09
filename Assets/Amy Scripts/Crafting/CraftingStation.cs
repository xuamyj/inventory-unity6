using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class CraftingStation : MonoBehaviour
{
    public StatusController.CraftingElemType craftingElemType;
    public StatusController.CraftingSizeType craftingSizeType;

    /* ---- DATA ---- */
    private Dictionary<SlotName, string> slotNameToItemKey;

    /* ---- UI: DRAGGED ---- */
    // public List<GameObject> visibleSlots;
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
        Personal_0,
        Personal_1,
        Personal_2,
        Personal_3,
        Personal_4,
        Personal_5,
        Personal_6,
        Personal_7,
        Personal_8,
        Personal_9,
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

    public bool TryPickupThing(CraftingStation.SlotName slotName)
    {
        // repeat status check
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();

        if (!(bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Crafting_InWorld && StatusController.instance.GetMouseCarryingBool() == false))
        {
            UnityEngine.Debug.Log("ERROR: CraftingStation.cs > TryPickupThing > something wrong with BigStatus or LittleStatus");
            return false;
        }

        // Data

        // UI

        // set status: if it got here, it worked
        StatusController.instance.SetMouseCarryingBool(true);
        return true;
    }

    public bool TryPutThing(CraftingStation.SlotName slotName)
    {
        // repeat status check
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();

        if (!(bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Crafting_InWorld && StatusController.instance.GetMouseCarryingBool() == true))
        {
            UnityEngine.Debug.Log("ERROR: CraftingStation.cs > TryPutThing > something wrong with BigStatus or LittleStatus");
            return false;
        }

        // Data

        // UI

        // set status: if it got here, it worked
        StatusController.instance.SetMouseCarryingBool(false);
        return true;
    }
}
