using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /* ---- MOVEMENT ---- */
    public InputAction UpAction;
    public InputAction LeftAction;
    public InputAction DownAction;
    public InputAction RightAction;

    public float speed;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    /* ---- RIGHT CLICK ---- */
    public InputAction MouseRightAction;
    public GameObject interactColliderObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /* ---- MOVEMENT ---- */

        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 10;

        UpAction.Enable();
        LeftAction.Enable();
        DownAction.Enable();
        RightAction.Enable();

        rigidbody2d = GetComponent<Rigidbody2D>();

        /* ---- RIGHT CLICK ---- */
        MouseRightAction.Enable();
        MouseRightAction.performed += RightClickedMouse;
    }

    // Update is called once per frame
    void Update()
    {
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();

        if (bigStatus == StatusController.BigStatus.GameSaved)
        {

        }
        else if (bigStatus == StatusController.BigStatus.Calendar)
        {

        }
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Default_InWorld)
        {
            /* ---- MOVEMENT ---- */
            horizontal = 0.0f;
            if (LeftAction.IsPressed())
            {
                horizontal = -speed;
            }
            else if (RightAction.IsPressed())
            {
                horizontal = speed;
            }
            // UnityEngine.Debug.Log("keyboard horizontal: " + horizontal);

            vertical = 0.0f;
            if (DownAction.IsPressed())
            {
                vertical = -speed;
            }
            else if (UpAction.IsPressed())
            {
                vertical = speed;
            }
            // UnityEngine.Debug.Log("keyboard vertical: " + vertical);
        }
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Inventory_InWorld)
        {

        }
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Dialogue_InWorld)
        {

        }
    }

    void FixedUpdate()
    {
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();

        if (bigStatus == StatusController.BigStatus.GameSaved)
        {

        }
        else if (bigStatus == StatusController.BigStatus.Calendar)
        {

        }
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Default_InWorld)
        {
            /* ---- MOVEMENT ---- */
            Vector2 position = rigidbody2d.position; // transform.position;
            position.x = position.x + horizontal * Time.deltaTime;
            position.y = position.y + vertical * Time.deltaTime;
            // transform.position = position;
            rigidbody2d.MovePosition(position);

            // Remember to (Hierarchy) Player > Rigidbody 2D > Interpolate set to "Interpolate" instead of "None" to fix judder
        }
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Inventory_InWorld)
        {

        }
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Dialogue_InWorld)
        {

        }
    }

    /* ---- RIGHT CLICK ---- */
    void RightClickedMouse(InputAction.CallbackContext context)
    {
        (StatusController.BigStatus bigStatus, StatusController.LittleStatus littleStatus) = StatusController.instance.GetStatus();

        if (bigStatus == StatusController.BigStatus.GameSaved)
        {

        }
        else if (bigStatus == StatusController.BigStatus.Calendar)
        {

        }
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Default_InWorld)
        {
            PInteractCollider interactCollider = interactColliderObj.GetComponent<PInteractCollider>();
            GameObject obj = interactCollider.currTriggerObj;
            UnityEngine.Debug.Log("Hi " + obj);

            /* ---- CHEST ---- */
            if (obj && obj.CompareTag("Chest"))
            {
                ChestController chest = obj.GetComponent<ChestController>();
                StatusController.instance.OpenInventory(chest.inventoryType);
            }

            /* ---- DIALOGUE ---- */
            if (obj && obj.CompareTag("Friend"))
            {
                FriendController friend = obj.GetComponent<FriendController>();
                int index = friend.yarnNodeIndex;
                int len = friend.yarnNodeNames.Capacity;
                if (index >= 0 && index < len)
                {
                    string currYarnNodeName = friend.yarnNodeNames[index];
                    StatusController.instance.EnterDialogue(currYarnNodeName);
                }
                else
                {
                    UnityEngine.Debug.Log("No dialogues to run. index = " + index + ", list length = " + len);
                }
            }

            /* ---- OBJECT ---- */
            if (obj && obj.CompareTag("Inventoryable"))
            {
                Inventoryable item = obj.GetComponent<Inventoryable>();
                string itemKey = item.itemKey;

                PersonalInventory personalI = StatusController.instance.personalInventoryUI.GetComponent<PersonalInventory>();

                Destroy(obj);
                personalI.TryAddItemToEmptySlot(itemKey);
            }
        }
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Inventory_InWorld)
        {

        }
        else if (bigStatus == StatusController.BigStatus.InWorld && littleStatus == StatusController.LittleStatus.Dialogue_InWorld)
        {

        }

    }
}
