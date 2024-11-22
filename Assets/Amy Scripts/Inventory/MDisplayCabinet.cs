using System;
using System.Collections.Generic;
using UnityEngine;

public class MDisplayCabinet : MonoBehaviour
{
    /* ---- DATA: set in StatusController, don't drag these ---- */
    public int currKey;
    public DisplayCabinetData displayCabinetData;

    /* ---- UI: DRAGGED ---- */
    public List<GameObject> visibleDisplaySlots;
    public List<GameObject> visibleExtraSlots;

    // private helper function
    private void SetupSlotList(List<GameObject> visibleSlotList, List<string> dataSlotList, DisplayCabinetSide side)
    {
        AllInventoryController.instance.DisplayCabinetDebugPrint("In SetupSlotList(), side " + side);

        // update the SpriteClickMovables
        for (int i = 0; i < visibleSlotList.Capacity; i++)
        {
            GameObject obj = visibleSlotList[i];
            string itemKey = dataSlotList[i];
            obj.GetComponent<SpriteClickMovable>().inventoryLocation = InventoryLocation.CreateDisplayCabinetLocation(currKey, side, i);

            if (itemKey != "") // item is there!
            {
                obj.GetComponent<UnityEngine.UI.Image>().sprite = ItemConsts.instance.GetAndLoadSpriteUrl(itemKey);
            }
            else
            {
                obj.GetComponent<UnityEngine.UI.Image>().sprite = StatusController.instance.BLANK_SPRITE;
            }
        }
    }

    public void SetupDisplayCabinetByKey(int key)
    {
        AllInventoryController.instance.DisplayCabinetDebugPrint("In SetupDisplayCabinetByKey(), key " + key);

        currKey = key;
        displayCabinetData = AInventoryData.instance.GetDisplayCabinetByKey(currKey);

        SetupSlotList(visibleDisplaySlots, displayCabinetData.displaySlots, DisplayCabinetSide.DisplaySide);
        SetupSlotList(visibleExtraSlots, displayCabinetData.extraSlots, DisplayCabinetSide.ExtraSide);
    }

    private void Awake()
    {
        currKey = 0; // default to 0 at the beginning, StatusController will update when you open one
        displayCabinetData = null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetItemKeyFromLoc(InventoryLocation loc)
    {
        AllInventoryController.instance.DisplayCabinetDebugPrint("In GetItemKeyFromLoc(), loc " + loc);

        if (loc.keyDisplayCabinet != currKey)
        {
            UnityEngine.Debug.Log("ERROR: MDisplayCabinet.cs > GetItemKeyFromLoc > something wrong with InventoryLocation's keyDisplayCabinet");
            return null;
        }

        int index = loc.displayCabinetIndex;
        if (loc.displayCabinetSide == DisplayCabinetSide.DisplaySide)
        {
            return displayCabinetData.displaySlots[index];
        }
        else // ExtraSide
        {
            return displayCabinetData.extraSlots[index];
        }
    }

    private void AddItemToLocHelper(InventoryLocation loc, string itemKey)
    {
        AllInventoryController.instance.DisplayCabinetDebugPrint("In AddItemToLocHelper(), loc " + loc);

        int index = loc.displayCabinetIndex;
        if (loc.displayCabinetSide == DisplayCabinetSide.DisplaySide)
        {
            // Data
            displayCabinetData.displaySlots[index] = itemKey;

            // UI
            visibleDisplaySlots[index].GetComponent<UnityEngine.UI.Image>().sprite = ItemConsts.instance.GetAndLoadSpriteUrl(itemKey);
        }
        else // ExtraSide
        {
            // Data
            displayCabinetData.extraSlots[index] = itemKey;

            // UI
            visibleExtraSlots[index].GetComponent<UnityEngine.UI.Image>().sprite = ItemConsts.instance.GetAndLoadSpriteUrl(itemKey);
        }
    }

    private void RemoveItemFromLocHelper(InventoryLocation loc)
    {
        AllInventoryController.instance.DisplayCabinetDebugPrint("In RemoveItemFromLocHelper(), loc " + loc);

        int index = loc.displayCabinetIndex;
        if (loc.displayCabinetSide == DisplayCabinetSide.DisplaySide)
        {
            // Data
            displayCabinetData.displaySlots[index] = "";

            // UI
            visibleDisplaySlots[index].GetComponent<UnityEngine.UI.Image>().sprite = StatusController.instance.BLANK_SPRITE;
        }
        else // ExtraSide
        {
            // Data
            displayCabinetData.extraSlots[index] = "";

            // UI
            visibleExtraSlots[index].GetComponent<UnityEngine.UI.Image>().sprite = StatusController.instance.BLANK_SPRITE;
        }
    }

    public bool TryRemoveItemFromLoc(InventoryLocation loc)
    {
        // in theory, well.. for now, this function always runs, in the future could add code to take care of StatusController
        AllInventoryController.instance.DisplayCabinetDebugPrint("In TryRemoveItemFromLoc(), loc " + loc);

        if (loc.keyDisplayCabinet != currKey)
        {
            UnityEngine.Debug.Log("ERROR: MDisplayCabinet.cs > TryRemoveItemFromLoc > something wrong with InventoryLocation's keyDisplayCabinet");
            return false;
        }

        if (GetItemKeyFromLoc(loc) != "")
        {
            RemoveItemFromLocHelper(loc);
            return true;
        }
        return false;
    }

    public bool TryAddItemToSpecificLoc(InventoryLocation loc, string itemKey)
    {
        // in theory, well.. for now, this function always runs, in the future could add code to take care of StatusController
        AllInventoryController.instance.DisplayCabinetDebugPrint("In TryAddItemToSpecificLoc(), loc " + loc);

        if (loc.keyDisplayCabinet != currKey)
        {
            UnityEngine.Debug.Log("ERROR: MDisplayCabinet.cs > TryAddItemToSpecificLoc > something wrong with InventoryLocation's keyDisplayCabinet");
            return false;
        }

        if (GetItemKeyFromLoc(loc) == "")
        {
            AddItemToLocHelper(loc, itemKey);
            return true;
        }
        return false;
    }
}
