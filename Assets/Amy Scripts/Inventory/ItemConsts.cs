using System.Collections.Generic;
using UnityEngine;

/* ---- DATA ---- */
public class ItemInfo
{
    public string itemKey; // "lemon"
    public string visibleName; // "Lemon"
    public string spriteUrl; // "Sprite-lemon2.png"
    public bool isRaw;
    public bool canBeUpgraded;
    public bool canBeDecorated;

    // other properties added later

    public ItemInfo(string iKey, string vName, string sUrl, bool iRaw = true, bool cbUpgraded = false, bool cbDecorated = false)
            : base() // calls base first
    {
        itemKey = iKey;
        visibleName = vName;
        spriteUrl = sUrl;
        isRaw = iRaw;
        canBeUpgraded = cbUpgraded;
        canBeDecorated = cbDecorated;
    }
}

public class ItemConsts : MonoBehaviour
{
    /* ---- CONSTS ---- */
    public const string BLANK_SPRITE_URL = "Sprite-blankobj";

    /* ---- DATA, should be used like a const ---- */
    public Dictionary<string, ItemInfo> itemInfoMap;

    /* ---- STATIC ---- */
    public static ItemConsts instance { get; private set; }
    private void Awake()
    {
        instance = this;

        /* ---- DATA ---- */
        itemInfoMap = new Dictionary<string, ItemInfo>();
        itemInfoMap["lemon"] = new ItemInfo("lemon", "Lemon", "Sprite-lemon2");
        itemInfoMap["crocus"] = new ItemInfo("crocus", "Crocus", "Crocus");
        itemInfoMap["clay"] = new ItemInfo("clay", "Clay", "clay_rock");
        itemInfoMap["rabbitsfoot"] = new ItemInfo("rabbitsfoot", "Rabbit's Foot", "Rabbits_Foot");
        itemInfoMap["pot"] = new ItemInfo("pot", "Pot", "pot_3", iRaw: false, cbUpgraded: true, cbDecorated: true);
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
