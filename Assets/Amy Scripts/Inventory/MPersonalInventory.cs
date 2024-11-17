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
    private UnityEngine.Sprite BLANK_SPRITE; // init in Awake() but treat like constant

    /* ---- UI: DRAGGED ---- */
    public List<GameObject> visibleSlots;

    private void Awake()
    {
        /* ---- DATA ---- */
        realSlots = new List<string>(Enumerable.Repeat("", 10));
        lockedIndexes = new HashSet<int>();

        /* ---- UI ---- */
        BLANK_SPRITE = Resources.Load<Sprite>(InventoryConsts.BLANK_SPRITE_URL);
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

        StatusController.instance.PersonalInventoryDebugPrint("Hit TryRemoveLockedItem( " + carryingItemKey + " )");

        foreach (int index in lockedIndexes)
        {
            if (carryingItemKey == realSlots[index])
            {
                /* ---- DATA ---- */
                UnlockIndex(index);
                realSlots[index] = "";

                /* ---- UI ---- */
                UnityEngine.UI.Image img = visibleSlots[index].GetComponent<UnityEngine.UI.Image>();
                img.sprite = BLANK_SPRITE;

                StatusController.instance.PersonalInventoryDebugPrint("True! index " + index);
                return true;
            }
        }
        StatusController.instance.PersonalInventoryDebugPrint("False!");
        return false;
    }

    // private helper function
    private void AddItemDataAndUIHelper(string itemKey, int index)
    {
        /* ---- DATA ---- */
        realSlots[index] = itemKey;

        /* ---- UI ---- */
        ItemInfo item = InventoryConsts.instance.itemInfoMap[itemKey];
        string spriteUrl = item.spriteUrl;
        UnityEngine.UI.Image img = visibleSlots[index].GetComponent<UnityEngine.UI.Image>();
        img.sprite = Resources.Load<Sprite>(spriteUrl);
    }

    public bool TryAddItemToEmptySlot(string itemKey)
    {
        // in theory, can get items from cutscene (different BigStatus) or dialogue (different LittleStatus). so this function always runs, need the calling function to take care of StatusController

        StatusController.instance.PersonalInventoryDebugPrint("Hit TryAddItemToEmptySlot( " + itemKey + " )");

        for (int i = 0; i < realSlots.Capacity; i++)
        {
            if (realSlots[i] == "")
            {
                AddItemDataAndUIHelper(itemKey, i);

                StatusController.instance.PersonalInventoryDebugPrint("True! index " + i);
                return true;
            }
        }
        StatusController.instance.PersonalInventoryDebugPrint("False!");
        return false;
    }

    public bool TryAddItemToSpecificSlot(string itemKey, int personalIndex)
    {
        // in theory, well.. for now, this function always runs, in the future could add code to take care of StatusController

        StatusController.instance.PersonalInventoryDebugPrint("Hit TryAddItemToSpecificSlot( " + itemKey + " , " + personalIndex + " )");

        if (realSlots[personalIndex] == "")
        {
            AddItemDataAndUIHelper(itemKey, personalIndex);

            StatusController.instance.PersonalInventoryDebugPrint("True! index " + personalIndex);
            return true;
        }
        StatusController.instance.PersonalInventoryDebugPrint("False!");
        return false;
    }
}
