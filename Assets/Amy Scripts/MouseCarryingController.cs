using UnityEngine;

public class MouseCarryingController : MonoBehaviour
{
    /* ---- UI: DRAGGED ---- */
    public Canvas canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();

        /* ---- CRAFTING ---- */
        if (StatusController.instance.IsInWorldAndInventoryStatus())
        {
            // Convert mouse position to canvas space
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 mousePositionAmy
            );

            transform.position = canvas.transform.TransformPoint(mousePositionAmy);

            // NOTE: remember to turn off Inspector > Image > Raycast Target!
        }
    }
}
