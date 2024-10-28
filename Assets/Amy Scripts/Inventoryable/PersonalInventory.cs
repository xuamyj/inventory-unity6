using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PersonalInventory : MonoBehaviour
{
    /* ---- DATA ---- */
    private List<string> realSlots;

    /* ---- UI: DRAGGED ---- */
    public List<GameObject> visibleSlots;


    private void Awake()
    {
        /* ---- DATA ---- */
        realSlots = new List<string>(10);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool TryAddItemToEmptySlot(string itemKey)
    {
        // in theory, can get items from cutscene (different BigStatus) or dialogue (different LittleStatus). so this function always runs, need the calling function to take care of StatusController

        UnityEngine.Debug.Log("Hit TryAddItemToEmptySlot( " + itemKey + " )");

        for (int i = 0; i < realSlots.Count; i++)
        {
            if (realSlots[i] == null)
            {
                realSlots[i] = itemKey;

                ItemInfo item = InventoryConsts.instance.itemInfoMap[itemKey];
                string spriteUrl = item.spriteUrl;
                // visibleSlots[i].GetComponent<Image>().SetSourceImage(spriteUrl);

                return true;
            }
        }
        return false;
    }
}
