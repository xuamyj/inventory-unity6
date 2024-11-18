using UnityEngine;

public class AllInventoryController : MonoBehaviour
{

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


}
