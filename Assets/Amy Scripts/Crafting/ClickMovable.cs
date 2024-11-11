using UnityEngine;
using UnityEngine.EventSystems;

public class ClickMovable : MonoBehaviour, IPointerClickHandler
{
    public CraftingStation.SlotName slotName;
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
        UnityEngine.Debug.Log("OMNOM 1 eventData" + eventData);

        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();

        /* ---- CRAFTING ---- */
        if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Crafting_InWorld)
        {
            UnityEngine.Debug.Log("OMNOM 2 littleStatus" + littleStatus);

            if (StatusController.instance.GetMouseCarryingBool() == false) // not holding anything
            {
                CraftingStation.instance.TryPickupThing(eventData, slotName, personalIndex);
            }
            else // holding something already
            {
                CraftingStation.instance.TryPutThing(eventData, slotName, personalIndex);
            }
        }
        /* ---- CHEST ---- */
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Inventory_InWorld)
        {
            // (do nothing now, but later might have click to move direct)
        }
    }
}
