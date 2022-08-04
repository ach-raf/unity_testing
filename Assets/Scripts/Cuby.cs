using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuby : MonoBehaviour, IClickable
{
    //public Rigidbody rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        //rigidBody = GetComponent<Rigidbody>();
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
        Debug.Log("Cuby Clicked!");
        EventManager.OnCubyClicked(this);
    }
    public void right_click()
    {
        Debug.Log("Cuby Right Clicked!");
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    /*public void UpdateVelocity(Vector3 _velocity)
    {
        rigidBody.velocity = _velocity;
    }
    public void UpdateVelocity(float _x, float _y, float _z)
    {
        rigidBody.velocity = new Vector3(_x, _y, _z);
    }*/
    void OnMouseDown()
    {
        Debug.Log("Mouse Down Cuby");
    }
}
