using UnityEngine;
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


    }

    // Update is called once per frame
    void Update()
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

    void FixedUpdate()
    {
        /* ---- MOVEMENT ---- */
        Vector2 position = rigidbody2d.position; // transform.position;
        position.x = position.x + horizontal * Time.deltaTime;
        position.y = position.y + vertical * Time.deltaTime;
        // transform.position = position;
        rigidbody2d.MovePosition(position);


    }
}
