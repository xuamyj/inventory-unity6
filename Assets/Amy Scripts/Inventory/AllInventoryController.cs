using UnityEngine;

public class AllInventoryController : MonoBehaviour
{
    /* ---- DATA: DRAGGED ---- */
    public MPersonalInventory mPersonalInventory;
    public MCraftingStation mCraftingStation; // TODO: this is not going to work if you change the crafting size

    /* ---- CONSTANTS: DRAGGED ---- */
    public UnityEngine.Sprite BLANK_SPRITE;

    /* ---- DEBUG PRINT ---- */
    public bool debugPrintCrafting;
    public bool debugPrintPersonalInventory;

    /* ---- STATIC ---- */
    public static AllInventoryController instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /* ---- DEBUG PRINT ---- */
    public void CraftingDebugPrint(string text)
    {
        if (debugPrintCrafting)
        {
            UnityEngine.Debug.Log("CRAFTING_DEBUG: " + text);
        }
    }
    public void PersonalInventoryDebugPrint(string text)
    {
        if (debugPrintPersonalInventory)
        {
            UnityEngine.Debug.Log("PERSONAL_INVENTORY_DEBUG: " + text);
        }
    }

    /* ---- ACTUAL CONTROLLER: HELPERS ---- */
    private string GetItemKeyFromLocHelper(InventoryLocation loc)
    {
        string itemKey = "";
        if (loc.inventoryType == InventoryType.MPersonalInventory)
        {
            return mPersonalInventory.GetItemKeyFromIndex(loc.personalInventoryIndex);
        }
        else if (loc.inventoryType == InventoryType.MCraftingStation)
        {
            return mCraftingStation.GetItemKeyFromSlot(loc.craftingSlotName);
        }
        else if (loc.inventoryType == InventoryType.MDisplayCabinet)
        {
            // these guys don't exist so add this later
        }
        else if (loc.inventoryType == InventoryType.NStorageChest)
        {
            // these guys don't exist so add this later
        }
        else if (loc.inventoryType == InventoryType.NSellingCrate)
        {
            // these guys don't exist so add this later
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > GetItemKeyFromLocHelper > something wrong with InventoryLocation");
        }
        return itemKey;
    }

    private void PutItemKeyInLocHelper(InventoryLocation loc, string itemKey)
    {
        if (loc.inventoryType == InventoryType.MPersonalInventory)
        {
            mPersonalInventory.TryAddItemToSpecificIndex(loc.personalInventoryIndex, itemKey);
        }
        else if (loc.inventoryType == InventoryType.MCraftingStation)
        {
            mCraftingStation.TryAddItemToSpecificSlot(loc.craftingSlotName, itemKey);
        }
        else if (loc.inventoryType == InventoryType.MDisplayCabinet)
        {
            // these guys don't exist so add this later
        }
        else if (loc.inventoryType == InventoryType.NStorageChest)
        {
            // these guys don't exist so add this later
        }
        else if (loc.inventoryType == InventoryType.NSellingCrate)
        {
            // these guys don't exist so add this later
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > PutItemKeyInLocHelper > something wrong with InventoryLocation");
        }
    }

    private void RemoveItemFromLocHelper(InventoryLocation loc)
    {
        if (loc.inventoryType == InventoryType.MPersonalInventory)
        {
            mPersonalInventory.RemoveItemFromIndexHelper(loc.personalInventoryIndex);
        }
        else if (loc.inventoryType == InventoryType.MCraftingStation)
        {
            mCraftingStation.RemoveItemFromSlotHelper(loc.craftingSlotName);
        }
        else if (loc.inventoryType == InventoryType.MDisplayCabinet)
        {
            // these guys don't exist so add this later
        }
        else if (loc.inventoryType == InventoryType.NStorageChest)
        {
            // these guys don't exist so add this later
        }
        else if (loc.inventoryType == InventoryType.NSellingCrate)
        {
            // these guys don't exist so add this later
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > RemoveItemFromLocHelper > something wrong with InventoryLocation");
        }
    }

    private bool IsValidLocForItemKey(InventoryLocation loc, string itemKey)
    {
        if (loc.inventoryType == InventoryType.MPersonalInventory ||
            loc.inventoryType == InventoryType.MDisplayCabinet ||
            loc.inventoryType == InventoryType.NStorageChest ||
            loc.inventoryType == InventoryType.NSellingCrate)
        {
            return true;
        }
        else if (loc.inventoryType == InventoryType.MCraftingStation)
        {
            return mCraftingStation.IsValidCraftingSlotForItem(loc.craftingSlotName, itemKey);
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > IsValidLocForItemKey > something wrong with InventoryLocation");
            return false;
        }
    }

    private bool ShoveFromPersonalInventoryHelper(StatusController.LittleStatus littleStatus, string carryingItemKey)
    {
        if (littleStatus == StatusController.LittleStatus.StorageChest_InWorld)
        {
            // TODO: these guys don't exist so add this later
            return true; // should be: return storageChest.Try...()
        }
        else if (littleStatus == StatusController.LittleStatus.SellingCrate_InWorld)
        {
            // TODO: these guys don't exist so add this later
            return true; // should be: return sellingCrate.Try...()
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > ShoveFromPersonalInventoryHelper > something wrong with LittleStatus");
            return false;
        }
    }

    /* ---- ACTUAL CONTROLLER: MAIN ---- */

    private bool TryPickupThing(InventoryLocation clickInventoryLocation)
    {
        // verify status
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();
        if (bigStatus != StatusController.BigStatus.InWorld)
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > TryPickupThing > something wrong with BigStatus");
            return false;
        }
        if ((clickInventoryLocation.inventoryType == InventoryType.MPersonalInventory) ||
           (clickInventoryLocation.inventoryType == InventoryType.MCraftingStation && littleStatus == StatusController.LittleStatus.Crafting_InWorld) ||
           (clickInventoryLocation.inventoryType == InventoryType.MDisplayCabinet && littleStatus == StatusController.LittleStatus.DisplayCabinet_InWorld))
        {
            UnityEngine.Debug.Log("TryPickupThing: status looks good");
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > TryPickupThing > something wrong with LittleStatus and InventoryLocation");
            return false;
        }

        string itemKey = GetItemKeyFromLocHelper(clickInventoryLocation);

        if (itemKey == "") // blank square
        {
            UnityEngine.Debug.Log("Clicked on blank square, do nothing");
            return false;
        }

        // actually do it
        AllInventoryController.instance.CraftingDebugPrint("OMNOM 4 made it here itemKey" + itemKey);
        StatusController.instance.StartMouseCarrying(itemKey, clickInventoryLocation);
        // TODO: gray out clickInventoryLocation

        return true;
    }

    private bool TryPutThing(InventoryLocation clickInventoryLocation)
    {
        // verify status
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();
        if (bigStatus != StatusController.BigStatus.InWorld)
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > TryPutThing > something wrong with BigStatus");
            return false;
        }
        if ((clickInventoryLocation.inventoryType == InventoryType.MPersonalInventory) ||
           (clickInventoryLocation.inventoryType == InventoryType.MCraftingStation && littleStatus == StatusController.LittleStatus.Crafting_InWorld) ||
           (clickInventoryLocation.inventoryType == InventoryType.MDisplayCabinet && littleStatus == StatusController.LittleStatus.DisplayCabinet_InWorld))
        {
            UnityEngine.Debug.Log("TryPutThing: status looks good");
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > TryPutThing > something wrong with LittleStatus and InventoryLocation");
            return false;
        }

        InventoryLocation carryingOrigLoc = StatusController.instance.GetMouseCarryingOrigLoc();
        string carryingItemKey = StatusController.instance.GetMouseCarryingItemKey();

        if (clickInventoryLocation == carryingOrigLoc) // put it back
        {
            StatusController.instance.StopMouseCarrying();
            // TODO: ungray carryingOrigLoc
            return true;
        }

        string clickItemKey = GetItemKeyFromLocHelper(clickInventoryLocation);
        if (clickItemKey != "") // occupied square
        {
            UnityEngine.Debug.Log("Clicked on occupied square, do nothing");
            return false;
        }

        if (!IsValidLocForItemKey(clickInventoryLocation, carryingItemKey)) // invalid square
        {
            UnityEngine.Debug.Log("Clicked on invalid square, do nothing");
            return false;
        }

        // actually do it
        PutItemKeyInLocHelper(clickInventoryLocation, carryingItemKey);
        StatusController.instance.StopMouseCarrying();
        RemoveItemFromLocHelper(carryingOrigLoc);

        return true;
    }

    private bool TryShoveThing(InventoryLocation clickInventoryLocation)
    {
        // verify big status
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();
        if (bigStatus != StatusController.BigStatus.InWorld)
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > TryShoveThing > something wrong with BigStatus");
            return false;
        }

        string carryingItemKey = StatusController.instance.GetMouseCarryingItemKey();

        // here are the available types of shove
        if (clickInventoryLocation.inventoryType == InventoryType.MPersonalInventory)
        {
            return ShoveFromPersonalInventoryHelper(littleStatus, carryingItemKey);
        }
        else if (
            (clickInventoryLocation.inventoryType == InventoryType.NStorageChest && littleStatus == StatusController.LittleStatus.StorageChest_InWorld) ||
            (clickInventoryLocation.inventoryType == InventoryType.NSellingCrate && littleStatus == StatusController.LittleStatus.SellingCrate_InWorld))
        {
            return mPersonalInventory.TryAddItemToEmptySlot(carryingItemKey);
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > TryShoveThing > something wrong with LittleStatus and InventoryLocation");
            return false;
        }
    }

    public bool ClickOnMovable(InventoryLocation inventoryLocation)
    {
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();
        if (bigStatus != StatusController.BigStatus.InWorld)
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > ClickOnMovable > something wrong with BigStatus");
            return false;
        }

        AllInventoryController.instance.CraftingDebugPrint("OMNOM 2 littleStatus" + littleStatus);

        /* ---- PICKUP / PUT ---- */
        if (littleStatus == StatusController.LittleStatus.Crafting_InWorld || littleStatus == StatusController.LittleStatus.DisplayCabinet_InWorld)
        {
            if (StatusController.instance.GetMouseCarryingBool() == false) // not holding anything
            {
                return TryPickupThing(inventoryLocation);
            }
            else // holding something already
            {
                return TryPutThing(inventoryLocation);
            }
        }
        /* ---- PERSONAL INVENTORY 2nd screen ---- */
        else if (littleStatus == StatusController.LittleStatus.PersonalInventory_InWorld)
        {
            UnityEngine.Debug.Log("TODO: this one truly doesn't exist rn. add code here when it does");
            return false;
        }
        /* ---- SHOVE ---- */
        else if (littleStatus == StatusController.LittleStatus.StorageChest_InWorld || littleStatus == StatusController.LittleStatus.SellingCrate_InWorld)
        {
            return TryShoveThing(inventoryLocation);
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: AllInventoryController.cs > ClickOnMovable > something wrong with LittleStatus");
            return false;
        }
    }
}
