using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour, IClickable
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public Transform GetTransform()
    {
        return transform;
    }
    public void SetColor(Color _color)
    {
        GetComponent<Renderer>().material.color = _color;
    }
    public void click()
    {
        Debug.Log("Clicked!");
    }
    public void right_click()
    {
        Debug.Log("Right Clicked!");
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
