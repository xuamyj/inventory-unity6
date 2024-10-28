using System.Collections.Generic;
using UnityEngine;

/* ---- DATA ---- */
public class ItemInfo
{
    public string itemKey; // "lemon"
    public string visibleName; // "Lemon"
    public string spriteUrl; // "Sprite-lemon2.png"
    // other properties added later

    public ItemInfo(string iKey, string vName, string sUrl)
            : base() // calls base first
    {
        itemKey = iKey;
        visibleName = vName;
        spriteUrl = sUrl;
    }
}

public class InventoryConsts : MonoBehaviour
{
    /* ---- CONSTS ---- */
    const string SPRITE_BLANK = "Sprite-blankobj_0.png";

    /* ---- DATA, should be used like a const ---- */
    public Dictionary<string, ItemInfo> itemInfoMap;

    /* ---- STATIC ---- */
    public static InventoryConsts instance { get; private set; }
    private void Awake()
    {
        instance = this;

        /* ---- DATA ---- */
        itemInfoMap = new Dictionary<string, ItemInfo>();
        itemInfoMap["lemon"] = new ItemInfo("lemon", "Lemon", "Crocus.png");
        itemInfoMap["crocus"] = new ItemInfo("crocus", "Crocus", "Sprite-lemon2.png");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
