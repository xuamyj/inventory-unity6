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

    public bool TryRemoveLockedItem(string carryingItemKey)
    {
        // in theory, well.. for now, this function always runs, in the future could add code to take care of StatusController

        AllInventoryController.instance.PersonalInventoryDebugPrint("Hit TryRemoveLockedItem( " + carryingItemKey + " )");

        foreach (int index in lockedIndexes)
        {
            if (carryingItemKey == realSlots[index])
            {
                /* ---- DATA ---- */
                UnlockIndex(index);
                realSlots[index] = "";

                /* ---- UI ---- */
                UnityEngine.UI.Image img = visibleSlots[index].GetComponent<UnityEngine.UI.Image>();
                img.sprite = AllInventoryController.instance.BLANK_SPRITE;

                AllInventoryController.instance.PersonalInventoryDebugPrint("True! index " + index);
                return true;
            }
        }
        AllInventoryController.instance.PersonalInventoryDebugPrint("False!");
        return false;
    }

    // private helper function
    private void AddItemDataAndUIHelper(string itemKey, int index)
    {
        /* ---- DATA ---- */
        realSlots[index] = itemKey;

        /* ---- UI ---- */
        ItemInfo item = ItemConsts.instance.itemInfoMap[itemKey];
        string spriteUrl = item.spriteUrl;
        UnityEngine.UI.Image img = visibleSlots[index].GetComponent<UnityEngine.UI.Image>();
        img.sprite = Resources.Load<Sprite>(spriteUrl);
    }

    public bool TryAddItemToEmptySlot(string itemKey)
    {
        // in theory, can get items from cutscene (different BigStatus) or dialogue (different LittleStatus). so this function always runs, need the calling function to take care of StatusController

        AllInventoryController.instance.PersonalInventoryDebugPrint("Hit TryAddItemToEmptySlot( " + itemKey + " )");

        for (int i = 0; i < realSlots.Capacity; i++)
        {
            if (realSlots[i] == "")
            {
                AddItemDataAndUIHelper(itemKey, i);

                AllInventoryController.instance.PersonalInventoryDebugPrint("True! index " + i);
                return true;
            }
        }
        AllInventoryController.instance.PersonalInventoryDebugPrint("False!");
        return false;
    }

    public bool TryAddItemToSpecificSlot(string itemKey, int personalIndex)
    {
        // in theory, well.. for now, this function always runs, in the future could add code to take care of StatusController

        AllInventoryController.instance.PersonalInventoryDebugPrint("Hit TryAddItemToSpecificSlot( " + itemKey + " , " + personalIndex + " )");

        if (realSlots[personalIndex] == "")
        {
            AddItemDataAndUIHelper(itemKey, personalIndex);

            AllInventoryController.instance.PersonalInventoryDebugPrint("True! index " + personalIndex);
            return true;
        }
        AllInventoryController.instance.PersonalInventoryDebugPrint("False!");
        return false;
    }
}
