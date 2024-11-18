using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MPersonalInventory : MonoBehaviour
{
    /* ---- DATA ---- */
    private List<string> realSlots;
    private HashSet<int> lockedIndexes;

    /* ---- UI: DRAGGED ---- */
    public List<GameObject> visibleSlots;

    private void Awake()
    {
        /* ---- DATA ---- */
        realSlots = new List<string>(Enumerable.Repeat("", 10));
        lockedIndexes = new HashSet<int>();
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
        return realSlots[index];
    }

    // these are currently private / called ..Helper because they don't check if the space is empty / item is present, they just do the thing
    private void AddItemToIndexHelper(int index, string itemKey)
    {
        // Data
        realSlots[index] = itemKey;

        // UI
        visibleSlots[index].GetComponent<UnityEngine.UI.Image>().sprite = ItemConsts.instance.GetAndLoadSpriteUrl(itemKey);
    }
    private void RemoveItemFromIndexHelper(int index)
    {
        // Data
        realSlots[index] = "";

        // UI
        visibleSlots[index].GetComponent<UnityEngine.UI.Image>().sprite = StatusController.instance.BLANK_SPRITE;
    }

    public HashSet<int> GetLockedIndexes()
    {
        return lockedIndexes;
    }
    public void LockIndex(int index)
    {
        lockedIndexes.Add(index);
    }
    public void UnlockIndex(int index)
    {
        lockedIndexes.Remove(index);
    }

    // public bool TryRemoveLockedItem(string carryingItemKey)
    // {
    //     // in theory, well.. for now, this function always runs, in the future could add code to take care of StatusController

    //     AllInventoryController.instance.PersonalInventoryDebugPrint("Hit TryRemoveLockedItem( " + carryingItemKey + " )");

    //     foreach (int index in lockedIndexes)
    //     {
    //         if (carryingItemKey == realSlots[index])
    //         {
    //             UnlockIndex(index);
    //             RemoveItemFromIndexHelper(index);

    //             AllInventoryController.instance.PersonalInventoryDebugPrint("True! index " + index);
    //             return true;
    //         }
    //     }
    //     AllInventoryController.instance.PersonalInventoryDebugPrint("False!");
    //     return false;
    // }

    public bool TryAddItemToEmptySlot(string itemKey)
    {
        // in theory, can get items from cutscene (different BigStatus) or dialogue (different LittleStatus). so this function always runs, need the calling function to take care of StatusController

        AllInventoryController.instance.PersonalInventoryDebugPrint("Hit TryAddItemToEmptySlot( " + itemKey + " )");

        for (int i = 0; i < realSlots.Capacity; i++)
        {
            if (realSlots[i] == "")
            {
                AddItemToIndexHelper(i, itemKey);

                AllInventoryController.instance.PersonalInventoryDebugPrint("True! index " + i);
                return true;
            }
        }
        AllInventoryController.instance.PersonalInventoryDebugPrint("False!");
        return false;
    }

    public bool TryRemoveItemFromIndex(int index)
    {
        // in theory, well.. for now, this function always runs, in the future could add code to take care of StatusController

        if (realSlots[index] != "")
        {
            RemoveItemFromIndexHelper(index);
            return true;
        }
        return false;
    }

    public bool TryAddItemToSpecificIndex(int index, string itemKey)
    {
        // in theory, well.. for now, this function always runs, in the future could add code to take care of StatusController

        AllInventoryController.instance.PersonalInventoryDebugPrint("Hit MPersonalInventory.cs: TryAddItemToSpecificIndex( " + index + " , " + itemKey + " )");

        if (realSlots[index] == "")
        {
            AddItemToIndexHelper(index, itemKey);

            AllInventoryController.instance.PersonalInventoryDebugPrint("True! index " + index);
            return true;
        }
        AllInventoryController.instance.PersonalInventoryDebugPrint("False!");
        return false;
    }
}
