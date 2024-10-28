using System.Collections.Generic;
using UnityEngine;

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
        Null
    }
    public enum InventoryType
    {
        Storage,
        Display,
        Selling
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

        /* ---- GAME START ---- */
        ToGameSavedScreen();
    }

    /* ---- PUBLIC STATUS FUNCTIONS ---- */
    public (BigStatus, LittleStatus) GetStatus()
    {
        return (bigStatus, littleStatus);
    }

    /* ---- UI FUNCTIONS ---- */
    public GameObject gameSavedScreen;
    public GameObject calendarScreen;
    public GameObject inWorldScreen;
    public GameObject storageInventoryUI;
    public GameObject displayInventoryUI;
    public GameObject sellingInventoryUI;

    private void ClearAllUI()
    {
        gameSavedScreen.gameObject.SetActive(false);
        calendarScreen.gameObject.SetActive(false);
        inWorldScreen.gameObject.SetActive(false);
        storageInventoryUI.gameObject.SetActive(false);
        displayInventoryUI.gameObject.SetActive(false);
        sellingInventoryUI.gameObject.SetActive(false);
    }

    public void ToGameSavedScreen()
    {
        // Status
        bigStatus = BigStatus.GameSaved;
        littleStatus = LittleStatus.Null;

        // UI
        ClearAllUI();
        gameSavedScreen.gameObject.SetActive(true);
    }
    public void ToCalendarScreen()
    {
        // TODO: update day to next day! (also involves moving red box, updating weekday/month/date, resetting water and energy)

        // Status
        bigStatus = BigStatus.Calendar;
        littleStatus = LittleStatus.Null;

        // UI
        ClearAllUI();
        calendarScreen.gameObject.SetActive(true);
    }
    public void ToInWorldScreen()
    {
        // Status
        bigStatus = BigStatus.InWorld;
        littleStatus = LittleStatus.Default_InWorld;

        // UI
        ClearAllUI();
        inWorldScreen.gameObject.SetActive(true);
    }

    public void TempButtonCloseInventory() // TODO: use the other function when.. ? check how stardew does it
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
                UnityEngine.Debug.Log("ERROR: StatusController.cs > TempButtonCloseInventory > close inventory > something wrong with InventoryType");
            }
        }
    }
    public void ToggleInventory(InventoryType type)
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
                UnityEngine.Debug.Log("ERROR: StatusController.cs > ToggleInventory > open inventory > something wrong with InventoryType");
            }
        }
        else if (bigStatus == BigStatus.InWorld && littleStatus == LittleStatus.Inventory_InWorld) // close inventory
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
                UnityEngine.Debug.Log("ERROR: StatusController.cs > ToggleInventory > close inventory > something wrong with InventoryType");
            }
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: StatusController.cs > ToggleInventory > something wrong with BigStatus or LittleStatus");
        }
    }

    // next funciton will toggle dialogue

    // Update is called once per frame
    void Update()
    {

    }
}
