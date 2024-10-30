using TMPro;
using UnityEngine;

public class WaterUIHelper : MonoBehaviour
{
    /* ---- WATER UI: DRAGGED ---- */
    public GameObject waterEmptyUI;
    public GameObject waterPartialUI;
    public GameObject waterFullUI;
    public TextMeshProUGUI waterTextUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // private helper function
    private void ClearAllWaterImage()
    {
        waterEmptyUI.SetActive(false);
        waterPartialUI.SetActive(false);
        waterFullUI.SetActive(false);
    }

    public void UpdateWaterUI(int currWater)
    {
        waterTextUI.text = "" + currWater + "/" + StatusController.MAX_WATER + " Water";

        int HALF = StatusController.MAX_WATER / 2;
        ClearAllWaterImage();
        if (currWater <= 0)
        {
            waterEmptyUI.SetActive(true);
        }
        else if (currWater >= 1 && currWater <= HALF) // 1 to 6
        {
            waterPartialUI.SetActive(true);
        }
        else if (currWater >= HALF + 1 && currWater <= StatusController.MAX_WATER) // 7 to 12
        {
            waterFullUI.SetActive(true);
        }
    }
}
