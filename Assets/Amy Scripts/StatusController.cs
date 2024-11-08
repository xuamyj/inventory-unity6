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
        Inventory_InWorld,
        Dialogue_InWorld,
        Crafting_InWorld,
        Null
    }
    public enum InventoryType
    {
        Storage,
        Display,
        Selling
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
    private InventoryType currInventoryType; // only relevant if LittleStatus = Inventory_InWorld
    private CraftingElemType currCraftingElemType; // these 2 only relevant if LittleStatus = Crafting_InWorld
    private CraftingSizeType currCraftingSizeType;
    private bool craftOrInvMouseCarrying;

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
    /* ---- YARN: DRAGGED ---- */
    public DialogueRunner yarnRunner;
    public GameObject cutsceneObj;

    /* ---- ONE-OFFS: DRAGGED ---- */
    public OneoffChair oneoffChair;

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
        currInventoryType = InventoryType.Storage; // we'll say this one is default for now
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
    public void OpenInventory(InventoryType type)
    {
        if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.Default_InWorld) // open inventory
        {
            // Status
            littleStatus = LittleStatus.Inventory_InWorld;
            currInventoryType = type;

            // UI
            if (currInventoryType == InventoryType.Storage)
            {
                storageInventoryUI.gameObject.SetActive(true);
            }
            else if (currInventoryType == InventoryType.Display)
            {
                displayInventoryUI.gameObject.SetActive(true);
            }
            else if (currInventoryType == InventoryType.Selling)
            {
                sellingInventoryUI.gameObject.SetActive(true);
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
        if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.Inventory_InWorld) // close inventory
        {
            // Status
            littleStatus = LittleStatus.Default_InWorld;

            // UI
            if (currInventoryType == InventoryType.Storage)
            {
                storageInventoryUI.gameObject.SetActive(false);
            }
            else if (currInventoryType == InventoryType.Display)
            {
                displayInventoryUI.gameObject.SetActive(false);
            }
            else if (currInventoryType == InventoryType.Selling)
            {
                sellingInventoryUI.gameObject.SetActive(false);
            }
            else
            {
                UnityEngine.Debug.Log("ERROR: StatusController.cs > CloseInventory > something wrong with InventoryType");
            }
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
    public void SetMouseCarryingBool(bool isCarrying)
    {
        craftOrInvMouseCarrying = isCarrying;
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
            }
            else if (currCraftingSizeType == CraftingSizeType.DecorOnly)
            {
                decorOnlyCraftingUI.gameObject.SetActive(true);
            }
            else if (currCraftingSizeType == CraftingSizeType.UpgradeDecor)
            {
                upgradeDecorCraftingUI.gameObject.SetActive(true);
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
                UnityEngine.Debug.Log("ERROR: StatusController.cs > OpenCrafting > something wrong with CraftingSizeType");
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
