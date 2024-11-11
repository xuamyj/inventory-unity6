using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PersonalInventory : MonoBehaviour
{
    /* ---- DATA ---- */
    private List<string> realSlots;
    private HashSet<int> lockedIndexes;

    /* ---- UI: DRAGGED ---- */
    public List<GameObject> visibleSlots;


    private void Awake()
    {
        /* ---- DATA ---- */
        realSlots = new List<string>(Enumerable.Repeat("", 10)); lockedIndexes = new HashSet<int>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

    public string GetItemKeyFromIndex(int index)
    {
        return realSlots[index];
    }

    public bool TryAddItemToEmptySlot(string itemKey)
    {
        // in theory, can get items from cutscene (different BigStatus) or dialogue (different LittleStatus). so this function always runs, need the calling function to take care of StatusController

        UnityEngine.Debug.Log("Hit TryAddItemToEmptySlot( " + itemKey + " )");

        for (int i = 0; i < realSlots.Capacity; i++)
        {
            if (realSlots[i] == "")
            {
                /* ---- DATA ---- */
                realSlots[i] = itemKey;

                /* ---- UI ---- */
                ItemInfo item = InventoryConsts.instance.itemInfoMap[itemKey];
                string spriteUrl = item.spriteUrl;
                UnityEngine.UI.Image img = visibleSlots[i].GetComponent<UnityEngine.UI.Image>();
                img.sprite = Resources.Load<Sprite>(spriteUrl);

                UnityEngine.Debug.Log("True! index " + i);
                return true;
            }
        }
        UnityEngine.Debug.Log("False!");
        return false;
    }
}
