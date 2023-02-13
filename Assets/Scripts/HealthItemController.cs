using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItemController : MonoBehaviour
{
    public int medKitRestorePoints;
    public float secondsToDestroy;

    private void Start()
    {
        Destroy(gameObject, secondsToDestroy);    
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerController>().RestoreHealth(medKitRestorePoints);
            Destroy(gameObject);
        }
    }
}
