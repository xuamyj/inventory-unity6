using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteClickMovable : MonoBehaviour, IPointerClickHandler
{
    public MCraftingStation.SlotName slotName;
    public int personalIndex; // only applicable if CraftingStation.SlotName = Personal

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // onclick
    public void OnPointerClick(PointerEventData eventData)
    {
        AllInventoryController.instance.CraftingDebugPrint("OMNOM 1 eventData" + eventData);

        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();

        /* ---- CRAFTING ---- */
        if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Crafting_InWorld)
        {
            AllInventoryController.instance.CraftingDebugPrint("OMNOM 2 littleStatus" + littleStatus);

            if (StatusController.instance.GetMouseCarryingBool() == false) // not holding anything
            {
                MCraftingStation.instance.TryPickupThing(eventData, slotName, personalIndex);
            }
            else // holding something already
            {
                MCraftingStation.instance.TryPutThing(eventData, slotName, personalIndex);
            }
        }
        /* ---- CHEST ---- */
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.DisplayCabinet_InWorld)
        {
            // (do nothing now, but later add things)
        }
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.StorageChest_InWorld)
        {
            // (do nothing now, but later add things)
        }
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.SellingCrate_InWorld)
        {
            // (do nothing now, but later add things)
        }
        /* ---- PERSONAL INVENTORY 2nd screen ---- */
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.PersonalInventory_InWorld)
        {
            // (do nothing now, but later add things)
        }
    }
}
