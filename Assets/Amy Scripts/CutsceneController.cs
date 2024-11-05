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
    /* ---- ONE-OFFS: DRAGGED ---- */
    public GameObject oneoffBowl;
    public GameObject oneoffWater;

    /* UI: DRAGGED */
    public UnityEngine.UI.Image backgroundImage;
    public GameObject cutsceneCanvas;

    /* ---- DATA, should be used like a const ---- */
    // const string CUTSCENE_BLANK_URL = "cutscene-blank";
    private Dictionary<string, CutsceneInfo> cutsceneInfoMap;

    /* ---- YARN ---- */
    DialogueRunner cutsceneRunner;
    private void Awake()
    {
        cutsceneRunner = gameObject.GetComponent<DialogueRunner>();

        /* ---- DATA ---- */
        cutsceneInfoMap = new Dictionary<string, CutsceneInfo>();
        cutsceneInfoMap["clerk"] = new CutsceneInfo("clerk", "Clerk", "cutscene/day1-clerk");
        cutsceneInfoMap["wally"] = new CutsceneInfo("wally", "Wally", "cutscene/day1-wally");
        cutsceneInfoMap["eat"] = new CutsceneInfo("eat", "Eat", "cutscene/town1-bg");
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

        // set active + background
        cutsceneCanvas.SetActive(true);
        backgroundImage.sprite = Resources.Load<Sprite>(bUrl);

        // TODO: do this properly later. currently one-off
        if (cutsceneKey == "eat")
        {
            StatusController.instance.TryAddEnergy(StatusController.MAX_ENERGY);
            oneoffBowl.SetActive(false);
            oneoffWater.SetActive(false);
        }

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
        // unset active
        cutsceneCanvas.SetActive(false);
        // backgroundImage.sprite = Resources.Load<Sprite>(CUTSCENE_BLANK_URL);

        StatusController.instance.BigToInWorldScreen();
    }
}
