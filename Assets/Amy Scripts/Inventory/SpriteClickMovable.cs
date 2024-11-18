using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteClickMovable : MonoBehaviour, IPointerClickHandler
{
    public InventoryLocation inventoryLocation;

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
        if (StatusController.instance.IsInWorldAndInventoryStatus())
        {
            AllInventoryController.instance.CraftingDebugPrint("OMNOM 1 eventData" + eventData);

            AllInventoryController.instance.ClickOnMovable(inventoryLocation);
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: SpriteClickMovable.cs > OnPointerClick > something wrong with IsInWorldAndInventoryStatus");
        }
    }
}
