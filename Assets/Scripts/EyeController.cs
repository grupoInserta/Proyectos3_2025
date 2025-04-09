using UnityEngine;
using System.Collections.Generic;

public class EyeController : MonoBehaviour
{
    public static bool lEyeClosed;
    public static bool rEyeClosed;

    public string greenPrefix = "(G)";
    public string pinkPrefix = "(P)";

    private List<GameObject> greenEntities = new List<GameObject>();
    private List<GameObject> pinkEntities = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        greenEntities.AddRange(FindAllStartingWith(greenPrefix));
        pinkEntities.AddRange(FindAllStartingWith(pinkPrefix));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            lEyeClosed = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            lEyeClosed = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            rEyeClosed = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            rEyeClosed = false;
        }
        ToggleEntities(greenEntities, lEyeClosed);
        ToggleEntities(pinkEntities, rEyeClosed);

    }
    GameObject[] FindAllStartingWith(string prefix)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        List<GameObject> filtered = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith(prefix) && !IsEditorOnly(obj))
            {
                filtered.Add(obj);
            }
        }

        return filtered.ToArray();
    }
    bool IsEditorOnly(GameObject obj)
    {
        return obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave;
    }
    void ToggleEntities(List<GameObject> entities, bool frozen)
    {
        foreach (GameObject obj in entities)
        {
            if (obj != null && obj.activeSelf == frozen)
                obj.SetActive(!frozen);
        }
    }
}
