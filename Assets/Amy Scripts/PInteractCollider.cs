using System.Collections.Generic;
using UnityEngine;

public class PInteractCollider : MonoBehaviour
{
    /* ---- TRIGGER ---- */
    public HashSet<GameObject> currTriggerObjs;
    void Awake()
    {
        currTriggerObjs = new HashSet<GameObject>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /* ---- TRIGGER ---- */
    void OnTriggerEnter2D(Collider2D other)
    {
        currTriggerObjs.Add(other.gameObject);

        /* ---- OUTLINE ---- */
        Material mat = other.GetComponent<Renderer>().material;
        if (mat != null)
        {
            mat.SetFloat("_OutlineAlpha", 1f);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        currTriggerObjs.Remove(other.gameObject);

        /* ---- OUTLINE ---- */
        Material mat = other.GetComponent<Renderer>().material;
        if (mat != null)
        {
            mat.SetFloat("_OutlineAlpha", 0f);
        }
    }
    public GameObject GetNearestTriggerObj()
    {
        float shortestDistance = float.MaxValue;
        GameObject nearestObj = null;
        foreach (GameObject obj in currTriggerObjs)
        {
            float dist = Vector3.Distance(obj.transform.position, transform.position);
            if (dist < shortestDistance)
            {
                nearestObj = obj;
            }
        }
        return nearestObj;
    }
}
