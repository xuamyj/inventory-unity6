using UnityEngine;

public class PInteractCollider : MonoBehaviour
{
    /* ---- TRIGGER ---- */
    public GameObject currTriggerObj; // TODO: track a list and get the closest one, don't let the second one clear the first

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /* ---- TRIGGER ---- */
        currTriggerObj = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* ---- TRIGGER ---- */
    void OnTriggerEnter2D(Collider2D other)
    {
        currTriggerObj = other.gameObject;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        currTriggerObj = null;
    }
}
