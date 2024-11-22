using System.Collections.Generic;
using UnityEngine;

public class NSellingCrate : MonoBehaviour
{
    /* ---- DATA: set in Awake() ---- */
    public SellingCrateData sellingCrateData;

    /* ---- UI: DRAGGED ---- */
    public List<GameObject> visibleSlots;

    private void Awake()
    {
        // UnityEngine.Debug.Log("INIT HERE NSellingCrate start");
        sellingCrateData = null;
        // UnityEngine.Debug.Log("INIT HERE NSellingCrate end");
    }

    public void SetupSellingCrate()
    {
        sellingCrateData = AInventoryData.instance.GetSellingCrate();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetItemKeyFromIndex(int index)
    {
        return sellingCrateData.realSlots[index];
    }
    private void AddItemToIndexHelper(int index, string itemKey)
    {
        // Data
        sellingCrateData.realSlots[index] = itemKey;

        // UI
        visibleSlots[index].GetComponent<UnityEngine.UI.Image>().sprite = ItemConsts.instance.GetAndLoadSpriteUrl(itemKey);
    }
    private void RemoveItemFromIndexHelper(int index)
    {
        // Data
        sellingCrateData.realSlots[index] = "";

        // UI
        visibleSlots[index].GetComponent<UnityEngine.UI.Image>().sprite = StatusController.instance.BLANK_SPRITE;
    }

    public bool TryAddItemToEmptySlot(string itemKey)
    {
        // in theory, can get items from cutscene (different BigStatus) or dialogue (different LittleStatus). so this function always runs, need the calling function to take care of StatusController

        for (int i = 0; i < sellingCrateData.realSlots.Capacity; i++)
        {
            if (sellingCrateData.realSlots[i] == "")
            {
                AddItemToIndexHelper(i, itemKey);
                return true;
            }
        }
        return false;
    }

    public bool TryRemoveItemFromIndex(int index)
    {
        // in theory, well.. for now, this function always runs, in the future could add code to take care of StatusController

        if (sellingCrateData.realSlots[index] != "")
        {
            RemoveItemFromIndexHelper(index);
            return true;
        }
        return false;
    }
}
