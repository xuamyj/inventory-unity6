using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class CraftingStation : MonoBehaviour
{
    public StatusController.CraftingElemType craftingElemType;
    public StatusController.CraftingSizeType craftingSizeType;

    /* ---- DATA ---- */
    // private List<string> realSlots;

    /* ---- UI: DRAGGED ---- */
    // public List<GameObject> visibleSlots;
    public GameObject waterEmptyImg;
    public GameObject waterFullImg;
    public GameObject ingredient1Slot;
    public GameObject ingredient2Slot;
    public GameObject simpleResultSlot;
    public GameObject upgradeIngr1Slot; // these + below can be null!
    public GameObject upgradeIngr2Slot;
    public GameObject upgradeResultSlot;
    public GameObject decorIngrSlot;
    public GameObject decorResultSlot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
