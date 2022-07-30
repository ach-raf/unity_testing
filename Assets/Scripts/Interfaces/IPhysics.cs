using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhysics
{
    void UpdateVelocity(Vector3 _velocity);
    void UpdateVelocity(float _x, float _y, float _z);
}
