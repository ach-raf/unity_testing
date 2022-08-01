using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class MouseOperations
{


    public static RaycastHit mouseHit;



    public static RaycastHit CastRay()
    {
        Vector3 mousePosFar = Mouse.current.position.ReadValue();
        mousePosFar.z = Camera.main.farClipPlane;

        Vector3 mousePosNear = Mouse.current.position.ReadValue();
        mousePosNear.z = Camera.main.nearClipPlane;

        Vector3 WorldPosFar = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 WorldPosNear = Camera.main.ScreenToWorldPoint(mousePosNear);

        RaycastHit hit;
        Physics.Raycast(WorldPosNear, WorldPosFar - WorldPosNear, out hit);
        return hit;
    }

    public static IClickable GetClickedObject()
    {
        try
        {
            RaycastHit hit = CastRay();
            if (hit.collider != null)
            {
                IClickable clicked_object = hit.collider.GetComponent<IClickable>();
                if (clicked_object != null)
                {
                    return clicked_object;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        catch (System.NullReferenceException)
        {
            return null;
        }
    }
    public static IPhysics GetPhysicsObject()
    {
        try
        {
            RaycastHit hit = CastRay();
            if (hit.collider != null)
            {
                IPhysics physics_object = hit.collider.GetComponent<IPhysics>();
                if (physics_object != null)
                {
                    return physics_object;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
        catch (System.NullReferenceException)
        {
            return null;
        }
    }
    public static RaycastHit GroundMousePosition(LayerMask ground_mask)
    {
        Vector3 mousePosFar = Mouse.current.position.ReadValue();
        mousePosFar.z = Camera.main.farClipPlane;

        Vector3 mousePosNear = Mouse.current.position.ReadValue();
        mousePosNear.z = Camera.main.nearClipPlane;

        Vector3 WorldPosFar = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 WorldPosNear = Camera.main.ScreenToWorldPoint(mousePosNear);

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit ground_ray_hit, float.MaxValue, ground_mask))
        {

        }
        return ground_ray_hit;
    }



}