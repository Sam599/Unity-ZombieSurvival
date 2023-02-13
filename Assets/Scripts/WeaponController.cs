using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletSpawnReference;
    public bool equipped;
    public float shotsInterval;
    float timeToShootCounter;
    bool readyToShoot = true;
    
    private void Update()
    {
        if(!readyToShoot)
        {
            timeToShootCounter += Time.deltaTime;
            if(timeToShootCounter >= shotsInterval)
            {
                readyToShoot = true;
            }
        }
    }

    // Update is called once per frame
    public bool ShootWeapon()
    {
        if (readyToShoot)
        {
            //Debug.Log(timeToShootCounter);
            Instantiate(bullet, bulletSpawnReference.transform.position, bulletSpawnReference.transform.rotation);
            AudioController.instance.PlayShotSound();
            timeToShootCounter = 0;
            readyToShoot = false;
            return true;
        }
        return false;
    }
}
