using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/* ---- DATA ---- */
public class CutsceneInfo
{
    public string cutsceneKey; // "clerk"
    public string yarnNodeName; // "Clerk"
    public string backgroundUrl; // "day1-clerk.png"
    // other properties added later

    public CutsceneInfo(string cKey, string yNName, string bUrl)
            : base() // calls base first
    {
        cutsceneKey = cKey;
        yarnNodeName = yNName;
        backgroundUrl = bUrl;
    }
}

public class CutsceneController : MonoBehaviour
{
    /* UI: DRAGGED */
    public UnityEngine.UI.Image backgroundImage;

    /* ---- DATA, should be used like a const ---- */
    const string CUTSCENE_BLANK_URL = "cutscene-blank";
    private Dictionary<string, CutsceneInfo> cutsceneInfoMap;

    /* ---- YARN ---- */
    DialogueRunner cutsceneRunner;
    private void Awake()
    {
        cutsceneRunner = gameObject.GetComponent<DialogueRunner>();

        /* ---- DATA ---- */
        cutsceneInfoMap = new Dictionary<string, CutsceneInfo>();
        cutsceneInfoMap["clerk"] = new CutsceneInfo("clerk", "Clerk", "day1-clerk");
        cutsceneInfoMap["wally"] = new CutsceneInfo("wally", "Wally", "day1-wally");
        cutsceneInfoMap["eat"] = new CutsceneInfo("eat", "Eat", "town1-bg");
    }

    // const cutscene key - script name - background name

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /* 
        StatusController's BigToCutsceneScreen()
        -> this SetupAndStartDialogue()
        -> Yarn's StartDialogue()
    */
    public void SetupAndStartDialogue(string cutsceneKey)
    {
        string yNName = cutsceneInfoMap[cutsceneKey].yarnNodeName;
        string bUrl = cutsceneInfoMap[cutsceneKey].backgroundUrl;

        // set background
        backgroundImage.sprite = Resources.Load<Sprite>(bUrl);

        // set script
        cutsceneRunner.StartDialogue(yNName);
    }
    /* 
        Yarn's OnDialogueComplete() [upon Stop()]
        -> this StopAndToWorld() 
        -> StatusController's BigToInWorldScreen()
    */
    public void StopAndToInWorld()
    {
        // unset background
        backgroundImage.sprite = Resources.Load<Sprite>(CUTSCENE_BLANK_URL);

        StatusController.instance.BigToInWorldScreen();
    }
}
