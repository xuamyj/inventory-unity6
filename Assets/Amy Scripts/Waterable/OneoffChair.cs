using UnityEngine;

public class OneoffChair : MonoBehaviour
{
    /* ---- DRAGGED ---- */
    public GameObject childCutsceneStarter;

    private int actionCount;
    void Awake()
    {
        actionCount = 2;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoThing()
    {
        if (actionCount > 0)
        {
            actionCount -= 1;
            UnityEngine.Debug.Log("OneoffChair's actionCount = " + actionCount);

            if (actionCount == 0)
            {
                // turn on the CutsceneStarter
                childCutsceneStarter.SetActive(true);
            }
        }
        // actionCount <= 0: don't do anything
    }
}
