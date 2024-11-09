using UnityEngine;

public class ClickMovable : MonoBehaviour
{
    public CraftingStation.SlotName slotName; // personal_#, or one of the 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // onclick
    void Something()
    {
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();

        /* ---- CRAFTING ---- */
        if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Crafting_InWorld)
        {
            if (StatusController.instance.GetMouseCarryingBool() == false) // not holding anything
            {
                CraftingStation.instance.TryPickupThing(slotName);
            }
            else // holding something already
            {
                CraftingStation.instance.TryPutThing(slotName);
            }
        }
        /* ---- CHEST ---- */
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Inventory_InWorld)
        {
            // (do nothing now, but later might have click to move direct)
        }
    }
}
