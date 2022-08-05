using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
// using TMPro;

public class IconScreenShotter : MonoBehaviour
{
    public bool takeScreenShots;
    public string prefabs_path = @"Assets/";
    private List<GameObject> itemsToScreenShot;
    ScreenshotHandler screenshotHandler;

    private void Awake()
    {
        itemsToScreenShot = new List<GameObject>();
    }
    private void Start()
    {
        screenshotHandler = GetComponent<ScreenshotHandler>();
        // #if UNITY_EDITOR
        //         EditorUtility.SetDirty(scriptableObject);
        // #endif
        if (takeScreenShots)
        {
            GetPrefabsIntoScene();
            StartCoroutine(GenerateScreenShots());
        }

        // AssetDatabase.SaveAssets();
    }
    public void DisplayGameObjects(int i)
    {
        foreach (GameObject item in itemsToScreenShot)
            item.SetActive(false);

        itemsToScreenShot[i].SetActive(true);
    }
    public IEnumerator GenerateScreenShots()
    {
        yield return new WaitForSeconds(1f);

        int i = 0;
        while (i < itemsToScreenShot.Count)
        {
            DisplayGameObjects(i);
            yield return new WaitForSeconds(.25f);

            screenshotHandler.TakeScreenshot(Screen.width, Screen.height, itemsToScreenShot[i].name);
            i++;

            yield return new WaitForSeconds(.25f);
        }
    }
    public UnityEngine.Object LoadPrefabFromFile(string filename)
    {
        Debug.Log("Trying to load LevelPrefab from file (" + filename + ")...");
        var loadedObject = Resources.Load(@"Prefabs/" + filename);
        if (loadedObject == null)
        {
            Debug.Log("Prefabs\\" + filename);
            throw new Exception("...no file found - please check the configuration");
        }
        return loadedObject;
    }

    public string[] GetPrefabsFromPath(string prefabPath)
    {
        string[] filePaths = Directory.GetFiles(string.Format(@"{0}", prefabPath), "*.prefab", SearchOption.TopDirectoryOnly);
        return filePaths;
    }
    public void InstantiatePrefab(string prefabName)
    {
        // tried to center object with pivot not on the center, but couldn't make it work
        Vector3 center_postion = new Vector3(Screen.width + gameObject.transform.position.x / 2, gameObject.transform.position.y, Screen.height + gameObject.transform.position.z / 2);


        UnityEngine.Object prefab = LoadPrefabFromFile(prefabName);
        GameObject prefabInstance = Instantiate(prefab, new Vector3(-5, -5, 5), gameObject.transform.rotation, gameObject.transform) as GameObject;
        prefabInstance.name = prefabName;
        itemsToScreenShot.Add(prefabInstance);

    }


    // set path at the inspector
    public void GetPrefabsIntoScene()
    {
        foreach (string item in GetPrefabsFromPath(prefabs_path))
        {
            Debug.Log(Path.GetFileNameWithoutExtension(item));
            InstantiatePrefab(Path.GetFileNameWithoutExtension(item));
        }
    }


}
