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
        //Vector3 facing = moveRigidbody.transform.forward;
        //float dotProduct = Vector3.Dot(moveRigidbody.velocity.normalized, facing);
        //Debug.Log(dotProduct);
        //
        //if (dotProduct >= -0.1f && dotProduct < 0.8) {
        //    speed = speed / 1.2f;
        //} else if (dotProduct < 0) {
        //    speed = speed / 1.5f;
        //}

        moveRigidbody.velocity = movement * speed;
    }

    public void Rotate(Vector3 lookDirection)
    {
        Quaternion rotation = Quaternion.LookRotation(lookDirection);

        moveRigidbody.MoveRotation(rotation);
    }

    public float GetObjectVelocity()
    {
        return moveRigidbody.velocity.magnitude;
    }
}
