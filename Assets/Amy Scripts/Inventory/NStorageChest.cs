using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NStorageChest : MonoBehaviour
{
    /* ---- DATA: set in StatusController, don't drag these ---- */
    public int currKey;
    public StorageChestData storageChestData;

    /* ---- UI: DRAGGED ---- */
    public List<GameObject> visibleSlots;

    public void SetupStorageChestByKey(int key)
    {
        currKey = key;
        storageChestData = AInventoryData.instance.GetStorageChestByKey(currKey);

        // update the SpriteClickMovables
        for (int i = 0; i < visibleSlots.Capacity; i++)
        {
            GameObject obj = visibleSlots[i];
            string itemKey = storageChestData.realSlots[i];
            obj.GetComponent<SpriteClickMovable>().inventoryLocation = InventoryLocation.CreateStorageChestLocation(key, i);

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

    private void Awake()
    {
        currKey = 0; // default to 0 at the beginning, StatusController will update when you open one
        storageChestData = null;
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
        return storageChestData.realSlots[index];
    }
    private void AddItemToIndexHelper(int index, string itemKey)
    {
        // Data
        storageChestData.realSlots[index] = itemKey;

        // UI
        visibleSlots[index].GetComponent<UnityEngine.UI.Image>().sprite = ItemConsts.instance.GetAndLoadSpriteUrl(itemKey);
    }
    private void RemoveItemFromIndexHelper(int index)
    {
        // Data
        storageChestData.realSlots[index] = "";

        // UI
        visibleSlots[index].GetComponent<UnityEngine.UI.Image>().sprite = StatusController.instance.BLANK_SPRITE;
    }

    public bool TryAddItemToEmptySlot(string itemKey)
    {
        // in theory, can get items from cutscene (different BigStatus) or dialogue (different LittleStatus). so this function always runs, need the calling function to take care of StatusController

        for (int i = 0; i < storageChestData.realSlots.Capacity; i++)
        {
            if (storageChestData.realSlots[i] == "")
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

        if (storageChestData.realSlots[index] != "")
        {
            RemoveItemFromIndexHelper(index);
            return true;
        }
        return false;
    }
}
