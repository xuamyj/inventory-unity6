using System;
using UnityEngine;

public enum CraftingSlotName
{
    Other,
    Ingredient1Key,
    Ingredient2Key,
    SimpleResultKey,
    DecorIngrKey,
    DecorResultKey,
    UpgradeIngr1Key,
    UpgradeIngr2Key,
    UpgradeResultKey,
}

public enum DisplayCabinetSide
{
    DisplaySide,
    NonDisplaySide,
}

public enum InventoryType
{
    MPersonalInventory,
    MCraftingStation,
    MDisplayCabinet,
    NStorageChest,
    NSellingCrate,
}

[Serializable]
public struct InventoryLocation : IEquatable<InventoryLocation> // postcard
{
    public InventoryType inventoryType;

    // PersonalInventory
    public int personalInventoryIndex;
    // CraftingStation
    public CraftingSlotName craftingSlotName;
    // DisplayCabinet
    public int keyDisplayCabinet;
    public DisplayCabinetSide displayCabinetSide;
    public int displayCabinetIndex;
    // StorageChest
    public int keyStorageChest;
    public int storageChestIndex;
    // SellingCrate
    public int sellingCrateIndex;

    // Factory methods for creating specific locations
    public static InventoryLocation CreatePersonalInventoryLocation(int index) => new InventoryLocation
    {
        inventoryType = InventoryType.MPersonalInventory,
        personalInventoryIndex = index
    };

    public static InventoryLocation CreateCraftingStationLocation(CraftingSlotName slot) => new InventoryLocation
    {
        inventoryType = InventoryType.MCraftingStation,
        craftingSlotName = slot
    };

    public static InventoryLocation CreateDisplayCabinetLocation(int cabinetKey, DisplayCabinetSide side, int index) => new InventoryLocation
    {
        inventoryType = InventoryType.MDisplayCabinet,
        keyDisplayCabinet = cabinetKey,
        displayCabinetSide = side,
        displayCabinetIndex = index
    };

    public static InventoryLocation CreateStorageChestLocation(int chestKey, int index) => new InventoryLocation
    {
        inventoryType = InventoryType.NStorageChest,
        keyStorageChest = chestKey,
        storageChestIndex = index
    };

    public static InventoryLocation CreateSellingCrateLocation(int index) => new InventoryLocation
    {
        inventoryType = InventoryType.NSellingCrate,
        sellingCrateIndex = index
    };

    // Claude wrote these below

    // Override Equals(object) - required for dictionary keys
    public override bool Equals(object obj) =>
        obj is InventoryLocation other && Equals(other);

    // IEquatable<T> implementation - provides type-safe equality comparison
    public bool Equals(InventoryLocation other)
    {
        // First check the inventory type
        if (inventoryType != other.inventoryType)
            return false;

        // Then check the relevant fields based on the inventory type
        return inventoryType switch
        {
            InventoryType.MPersonalInventory =>
                personalInventoryIndex == other.personalInventoryIndex,

            InventoryType.MCraftingStation =>
                craftingSlotName == other.craftingSlotName,

            InventoryType.MDisplayCabinet =>
                keyDisplayCabinet == other.keyDisplayCabinet &&
                displayCabinetSide == other.displayCabinetSide &&
                displayCabinetIndex == other.displayCabinetIndex,

            InventoryType.NStorageChest =>
                keyStorageChest == other.keyStorageChest &&
                storageChestIndex == other.storageChestIndex,

            InventoryType.NSellingCrate =>
                sellingCrateIndex == other.sellingCrateIndex,

            _ => false
        };
    }

    // Define == operator
    public static bool operator ==(InventoryLocation left, InventoryLocation right)
    {
        return left.Equals(right);
    }

    // When you define ==, you must also define != 
    public static bool operator !=(InventoryLocation left, InventoryLocation right)
    {
        return !(left == right);
    }

    // Override GetHashCode - required for dictionary keys
    public override int GetHashCode()
    {
        unchecked // Prevents overflow exceptions
        {
            const int prime = 31;
            int hash = 17;

            // Always include the inventory type
            hash = hash * prime + inventoryType.GetHashCode();

            // Add relevant fields based on inventory type
            switch (inventoryType)
            {
                case InventoryType.MPersonalInventory:
                    hash = hash * prime + personalInventoryIndex.GetHashCode();
                    break;

                case InventoryType.MCraftingStation:
                    hash = hash * prime + craftingSlotName.GetHashCode();
                    break;

                case InventoryType.MDisplayCabinet:
                    hash = hash * prime + keyDisplayCabinet.GetHashCode();
                    hash = hash * prime + displayCabinetSide.GetHashCode();
                    hash = hash * prime + displayCabinetIndex.GetHashCode();
                    break;

                case InventoryType.NStorageChest:
                    hash = hash * prime + keyStorageChest.GetHashCode();
                    hash = hash * prime + storageChestIndex.GetHashCode();
                    break;

                case InventoryType.NSellingCrate:
                    hash = hash * prime + sellingCrateIndex.GetHashCode();
                    break;
            }

            return hash;
        }
    }

    // Override ToString for debugging
    public override string ToString()
    {
        return inventoryType switch
        {
            InventoryType.MPersonalInventory =>
                $"Personal[{personalInventoryIndex}]",

            InventoryType.MCraftingStation =>
                $"Crafting[{craftingSlotName}]",

            InventoryType.MDisplayCabinet =>
                $"Display[{keyDisplayCabinet}:{displayCabinetSide}:{displayCabinetIndex}]",

            InventoryType.NStorageChest =>
                $"Storage[{keyStorageChest}:{storageChestIndex}]",

            InventoryType.NSellingCrate =>
                $"Selling[{sellingCrateIndex}]",

            _ => inventoryType.ToString()
        };
    }
}
