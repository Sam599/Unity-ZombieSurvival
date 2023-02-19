using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private Rigidbody moveRigidbody;

    void Awake()
    {
        moveRigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 movement, float speed)
    {
        moveRigidbody.velocity = movement * speed;

        if (movement != Vector3.zero)
        {
            moveRigidbody.MoveRotation(Quaternion.LookRotation(movement));
        }
    }

    public void Rotate(Vector3 lookDirection)
    {
        moveRigidbody.MoveRotation(Quaternion.LookRotation(lookDirection));
    }

    public float GetObjectVelocity()
    {
        return moveRigidbody.velocity.magnitude;
    }
}
