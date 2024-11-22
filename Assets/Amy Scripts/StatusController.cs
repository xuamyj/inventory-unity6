using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class StatusController : MonoBehaviour
{
    /* ---- STATIC ---- */
    public static StatusController instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }

    /* ---- CONSTANTS ---- */
    enum Weekday
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }
    enum Month
    {
        Summer,
        Fall
    }
    public const int MAX_WATER = 12;
    public const int MAX_ENERGY = 40;

    public enum BigStatus
    {
        GameSaved,
        Calendar,
        InWorld,
        CutScene
    }
    public enum LittleStatus
    {
        Default_InWorld,
        Dialogue_InWorld,
        Crafting_InWorld,
        DisplayCabinet_InWorld,
        StorageChest_InWorld,
        SellingCrate_InWorld,
        PersonalInventory_InWorld,
        Null
    }
    public enum CraftingElemType
    {
        Kiln, // clay + water + heat = pottery
        // Stove, // carrot (or flour) + water + heat = food
        // Infuser, // lemon + garlic + water + magic = vulnerary
        // Papermachine, // wood (pulp) + water + magic = tome 
    }
    public enum CraftingSizeType
    {
        Simple,
        DecorOnly,
        UpgradeDecor,
    }

    /* ---- VARIABLES (FIELDS) ---- */
    [SerializeField] private Weekday weekday;
    [SerializeField] private Month month;
    [SerializeField] private int dateNum;
    private float startTime;
    [SerializeField] private int currSilverCoins;
    [SerializeField] private int currWater;
    [SerializeField] private int currEnergy;
    private Dictionary<string, int> supportPointsMap;

    [SerializeField] private BigStatus bigStatus;
    [SerializeField] private LittleStatus littleStatus; // only relevant if BigStatus == InWorld
    private CraftingElemType currCraftingElemType; // these 2 only relevant if LittleStatus = Crafting_InWorld
    private CraftingSizeType currCraftingSizeType;
    private bool craftOrInvMouseCarrying;
    private string itemKeyMouseCarrying;
    private InventoryLocation origLocMouseCarrying;

    /* ---- UI: DRAGGED ---- */
    public GameObject gameSavedScreen;
    public GameObject calendarScreen;
    public GameObject inWorldScreen;
    public GameObject storageInventoryUI;
    public GameObject displayInventoryUI;
    public GameObject sellingInventoryUI;
    public GameObject personalInventoryUI; // these 6 are part of inWorldScreen
    public GameObject waterUI;
    public TextMeshProUGUI energyTextUI;
    public GameObject simpleCraftingUI;
    public GameObject decorOnlyCraftingUI;
    public GameObject upgradeDecorCraftingUI;
    public UnityEngine.UI.Image mouseCarryingImageUI;
    /* ---- YARN: DRAGGED ---- */
    public DialogueRunner yarnRunner;
    public GameObject cutsceneObj;

    /* ---- ONE-OFFS: DRAGGED ---- */
    public OneoffChair oneoffChair;

    /* ---- CONSTANTS: DRAGGED ---- */
    public UnityEngine.Sprite BLANK_SPRITE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
        supportPointsMap = new Dictionary<string, int>() {
            {"AE", 0}, // Ava - Evander
            {"AI", 0}, // Ava - Isadora
            {"AM", 0}, // Ava - Malcolm
            {"AN", 0}, // Ava - Nadira
            {"EI", 0}, // Evander - Isadora
            {"EM", 0}, // Evander - Malcolm
            {"EN", 0}, // Evander - Nadira
            {"IM", 0}, // Isadora - Malcolm
            {"IN", 0}, // Isadora - Nadira
            {"MN", 0}, // Malcolm - Nadira
        };
        currCraftingElemType = CraftingElemType.Kiln; // we'll say these 2 are default for now
        currCraftingSizeType = CraftingSizeType.Simple;
        craftOrInvMouseCarrying = false;

        /* ---- GAME START ---- */
        currWater = 0;
        currEnergy = 10;
        BigToGameSavedScreen();
    }

    /* ---- PUBLIC STATUS FUNCTIONS ---- */
    public (BigStatus, LittleStatus) GetStatus()
    {
        return (bigStatus, littleStatus);
    }
    public bool IsInWorldAndInventoryStatus()
    {
        if (bigStatus != StatusController.BigStatus.InWorld)
        {
            return false;
        }

        if ((littleStatus == StatusController.LittleStatus.PersonalInventory_InWorld) ||
            (littleStatus == StatusController.LittleStatus.Crafting_InWorld) ||
            (littleStatus == StatusController.LittleStatus.DisplayCabinet_InWorld) ||
            (littleStatus == StatusController.LittleStatus.StorageChest_InWorld) ||
            (littleStatus == StatusController.LittleStatus.SellingCrate_InWorld))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // private helper function
    private void ClearAllUI()
    {
        gameSavedScreen.gameObject.SetActive(false);
        calendarScreen.gameObject.SetActive(false);
        inWorldScreen.gameObject.SetActive(false);
        storageInventoryUI.gameObject.SetActive(false);
        displayInventoryUI.gameObject.SetActive(false);
        sellingInventoryUI.gameObject.SetActive(false);
        simpleCraftingUI.gameObject.SetActive(false);
        decorOnlyCraftingUI.gameObject.SetActive(false);
        upgradeDecorCraftingUI.gameObject.SetActive(false);
    }

    public void BigToGameSavedScreen()
    {
        // Status
        bigStatus = BigStatus.GameSaved;
        littleStatus = LittleStatus.Null;

        // UI
        ClearAllUI();
        gameSavedScreen.gameObject.SetActive(true);
    }
    public void BigToCalendarScreen()
    {
        // TODO: update day to next day! (also involves moving red box, updating weekday/month/date, resetting water and energy)

        // Status
        bigStatus = BigStatus.Calendar;
        littleStatus = LittleStatus.Null;

        // UI
        ClearAllUI();
        calendarScreen.gameObject.SetActive(true);
    }
    public void BigToCutsceneScreen(string cutsceneKey)
    {
        // Status
        bigStatus = BigStatus.CutScene;
        littleStatus = LittleStatus.Null;

        // UI
        ClearAllUI();
        CutsceneController cutsceneC = cutsceneObj.GetComponent<CutsceneController>();
        cutsceneC.SetupAndStartDialogue(cutsceneKey);
    }
    public void BigToInWorldScreen()
    {
        // Status
        bigStatus = BigStatus.InWorld;
        littleStatus = LittleStatus.Default_InWorld;

        // UI
        ClearAllUI();
        inWorldScreen.gameObject.SetActive(true);
        waterUI.GetComponent<WaterUIHelper>().UpdateWaterUI(currWater);
        energyTextUI.text = "" + currEnergy + "/" + StatusController.MAX_ENERGY + " E";
    }

    /* ---- CHEST ---- */
    public void OpenInventory(InventoryType type, int keyDisplayAndStorage)
    {
        if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.Default_InWorld) // open inventory
        {
            if (type == InventoryType.NStorageChest)
            {
                // Status
                littleStatus = LittleStatus.StorageChest_InWorld;

                // UI
                storageInventoryUI.gameObject.SetActive(true);
                NStorageChest nStorageChest = storageInventoryUI.GetComponent<NStorageChest>();
                nStorageChest.SetupStorageChestByKey(keyDisplayAndStorage);
            }
            else if (type == InventoryType.MDisplayCabinet)
            {
                // Status
                littleStatus = LittleStatus.DisplayCabinet_InWorld;

                // UI
                displayInventoryUI.gameObject.SetActive(true);
                // TODO: add this back in when it's created
                // MDisplayCabinet mDisplayCabinet = storageInventoryUI.GetComponent<MDisplayCabinet>();
                // mDisplayCabinet.SetupDisplayCabinetByKey(keyDisplayAndStorage);
            }
            else if (type == InventoryType.NSellingCrate)
            {
                // Status
                littleStatus = LittleStatus.SellingCrate_InWorld;

                // UI
                sellingInventoryUI.gameObject.SetActive(true);
                NSellingCrate nSellingCrate = sellingInventoryUI.GetComponent<NSellingCrate>();
                nSellingCrate.SetupSellingCrate();
            }
            else
            {
                UnityEngine.Debug.Log("ERROR: StatusController.cs > OpenInventory > something wrong with InventoryType");
            }
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: StatusController.cs > OpenInventory > something wrong with BigStatus or LittleStatus");
        }
    }
    public void CloseInventory()
    {
        if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.StorageChest_InWorld)
        {
            // Status
            littleStatus = LittleStatus.Default_InWorld;

            // UI
            storageInventoryUI.gameObject.SetActive(false);
        }
        else if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.DisplayCabinet_InWorld)
        {
            // Status
            littleStatus = LittleStatus.Default_InWorld;

            // UI
            displayInventoryUI.gameObject.SetActive(false);
        }
        else if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.SellingCrate_InWorld)
        {
            // Status
            littleStatus = LittleStatus.Default_InWorld;

            // UI
            sellingInventoryUI.gameObject.SetActive(false);
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: StatusController.cs > CloseInventory > something wrong with BigStatus or LittleStatus");
        }
    }

    /* ---- CRAFTING ---- */ // note to self: type, data, init, get/set
    public (CraftingElemType, CraftingSizeType) GetCraftingType()
    {
        return (currCraftingElemType, currCraftingSizeType);
    }
    public void SetCraftingType(CraftingElemType elemT, CraftingSizeType sizeT)
    {
        currCraftingElemType = elemT;
        currCraftingSizeType = sizeT;
    }

    public bool GetMouseCarryingBool()
    {
        return craftOrInvMouseCarrying;
    }
    public string GetMouseCarryingItemKey()
    {
        return itemKeyMouseCarrying;
    }
    public InventoryLocation GetMouseCarryingOrigLoc()
    {
        return origLocMouseCarrying;
    }
    public void StartMouseCarrying(string itemKey, InventoryLocation origLoc)
    {
        // Data
        craftOrInvMouseCarrying = true;
        itemKeyMouseCarrying = itemKey;
        origLocMouseCarrying = origLoc;

        // UI
        mouseCarryingImageUI.sprite = ItemConsts.instance.GetAndLoadSpriteUrl(itemKey);
    }
    public void StopMouseCarrying()
    {
        // Data
        craftOrInvMouseCarrying = false;
        itemKeyMouseCarrying = "";
        origLocMouseCarrying = InventoryLocation.CreateInvalidLocation();

        // UI
        mouseCarryingImageUI.sprite = BLANK_SPRITE;
    }

    public void OpenCrafting(CraftingElemType elemT, CraftingSizeType sizeT)
    {
        if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.Default_InWorld) // open crafting
        {
            // Status
            littleStatus = LittleStatus.Crafting_InWorld;
            SetCraftingType(elemT, sizeT);

            // UI
            if (currCraftingSizeType == CraftingSizeType.Simple)
            {
                simpleCraftingUI.gameObject.SetActive(true);
                AllInventoryController.instance.mCurrCraftingStation = simpleCraftingUI.GetComponent<MCraftingStation>();
            }
            else if (currCraftingSizeType == CraftingSizeType.DecorOnly)
            {
                decorOnlyCraftingUI.gameObject.SetActive(true);
                AllInventoryController.instance.mCurrCraftingStation = decorOnlyCraftingUI.GetComponent<MCraftingStation>();

            }
            else if (currCraftingSizeType == CraftingSizeType.UpgradeDecor)
            {
                upgradeDecorCraftingUI.gameObject.SetActive(true);
                AllInventoryController.instance.mCurrCraftingStation = upgradeDecorCraftingUI.GetComponent<MCraftingStation>();

            }
            else
            {
                UnityEngine.Debug.Log("ERROR: StatusController.cs > OpenCrafting > something wrong with CraftingSizeType");
            }
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: StatusController.cs > OpenCrafting > something wrong with BigStatus or LittleStatus");
        }
    }
    public void CloseCrafting()
    {
        if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.Crafting_InWorld) // close crafting
        {
            // Status
            littleStatus = LittleStatus.Default_InWorld;

            // TODO: move your ingredients and outputs back to your personalinventory!

            // UI
            if (currCraftingSizeType == CraftingSizeType.Simple)
            {
                simpleCraftingUI.gameObject.SetActive(false);
            }
            else if (currCraftingSizeType == CraftingSizeType.DecorOnly)
            {
                decorOnlyCraftingUI.gameObject.SetActive(false);
            }
            else if (currCraftingSizeType == CraftingSizeType.UpgradeDecor)
            {
                upgradeDecorCraftingUI.gameObject.SetActive(false);
            }
            else
            {
                UnityEngine.Debug.Log("ERROR: StatusController.cs > CloseCrafting > something wrong with CraftingSizeType");
            }
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: StatusController.cs > CloseCrafting > something wrong with BigStatus or LittleStatus");
        }
    }

    /* ---- DIALOGUE ---- */
    public void EnterDialogue(string yarnNodeName)
    {
        if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.Default_InWorld) // enter dialogue
        {
            // Status
            littleStatus = LittleStatus.Dialogue_InWorld;

            // Yarn & UI
            ClearAllUI();
            yarnRunner.StartDialogue(yarnNodeName);
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: StatusController.cs > EnterDialogue > something wrong with BigStatus or LittleStatus");
        }
    }
    public void LeaveDialogue()
    {
        if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.Dialogue_InWorld) // leave dialogue
        {
            // Status
            littleStatus = LittleStatus.Default_InWorld;

            // Yarn & UI
            ClearAllUI();
            inWorldScreen.gameObject.SetActive(true);
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: StatusController.cs > LeaveDialogue > something wrong with BigStatus or LittleStatus");
        }
    }

    /* ---- WATER ---- */
    public void TryGetWater()
    {
        if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.Default_InWorld)
        {
            // Data
            currWater = MAX_WATER;
            currEnergy -= 1;

            // UI
            waterUI.GetComponent<WaterUIHelper>().UpdateWaterUI(currWater);
            energyTextUI.text = "" + currEnergy + "/" + StatusController.MAX_ENERGY + " E";

            // Object - none for this one
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: StatusController.cs > TryGetWater > something wrong with BigStatus or LittleStatus");
        }
    }
    public bool TryWateringThing(Waterable thing)
    {
        if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.Default_InWorld)
        {
            if (currWater <= 0)
            {
                UnityEngine.Debug.Log("Water is empty :(");
                return false;
            }

            // Data
            currWater -= 1;
            currEnergy -= 1;

            // UI
            waterUI.GetComponent<WaterUIHelper>().UpdateWaterUI(currWater);
            energyTextUI.text = "" + currEnergy + "/" + StatusController.MAX_ENERGY + " E";

            // Object
            GameObject toBecome = thing.wateredToBecome;
            thing.gameObject.SetActive(false);
            toBecome.SetActive(true);

            oneoffChair.DoThing(); // one-off
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: StatusController.cs > TryWateringThing > something wrong with BigStatus or LittleStatus");
        }
        return false;
    }

    /* ---- ADD ENERGY ---- */
    public void TryAddEnergy(int energyToAdd)
    {
        // in theory, can gain energy from cutscene or regular food. so this function always runs

        // Data
        currEnergy = Math.Min(currEnergy + energyToAdd, MAX_ENERGY);

        // UI
        energyTextUI.text = "" + currEnergy + "/" + StatusController.MAX_ENERGY + " E";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
