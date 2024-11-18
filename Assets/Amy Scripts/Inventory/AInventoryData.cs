using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class StorageChestData
{
    public List<string> realSlots;
    public int keyStorage;

    public StorageChestData(int key)
            : base() // calls base first
    {
        realSlots = new List<string>(Enumerable.Repeat("", 24));
        keyStorage = key;
    }

    public string GetItemKeyFromIndex(int index)
    {
        return realSlots[index];
    }
}

public class DisplayCabinetData
{
    public List<string> displaySlots;
    public List<string> extraSlots;
    public int keyDisplay;

    public DisplayCabinetData(int key)
            : base() // calls base first
    {
        displaySlots = new List<string>(Enumerable.Repeat("", 4));
        extraSlots = new List<string>(Enumerable.Repeat("", 8));
        keyDisplay = key;
    }
}

public class AInventoryData : MonoBehaviour
{
    /* ---- DATA ---- */
    // currently only has it for StorageChest and DisplayCabinet
    public List<StorageChestData> storageChests;
    int nextStorageChestKey;
    public List<DisplayCabinetData> displayCabinets;
    int nextDisplayCabinetKey;

    /* ---- STATIC ---- */
    public static AInventoryData instance { get; private set; }
    private void Awake()
    {
        instance = this;
        storageChests = new List<StorageChestData>();
        storageChests.Add(new StorageChestData(0));
        storageChests.Add(new StorageChestData(1));
        nextStorageChestKey = 2;

        displayCabinets = new List<DisplayCabinetData>();
        displayCabinets.Add(new DisplayCabinetData(0));
        displayCabinets.Add(new DisplayCabinetData(1));
        nextDisplayCabinetKey = 2;
    }

    public void MakeNewStorageChest()
    {
        storageChests.Add(new StorageChestData(nextStorageChestKey));
        nextStorageChestKey++;
    }

    public void MakeNewDisplayCabinet()
    {
        displayCabinets.Add(new DisplayCabinetData(nextDisplayCabinetKey));
        nextDisplayCabinetKey++;
    }

    public StorageChestData GetStorageChestByKey(int key)
    {
        return storageChests[key];
    }

    public DisplayCabinetData GetDisplayCabinetByKey(int key)
    {
        return displayCabinets[key];
    }
}