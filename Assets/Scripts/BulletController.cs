using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    private CharactersStatus bulletStatus;

    // Start is called before the first frame update
    void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletStatus = GetComponent<CharactersStatus>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bulletRigidbody.MovePosition(bulletRigidbody.position + transform.forward * bulletStatus.speed * Time.fixedDeltaTime);

    }
    void OnTriggerEnter(Collider other)
    {
        IKillableObjects killable = other.GetComponent<IKillableObjects>();
        if (killable != null)
        {
            killable.TakeHit(bulletStatus.hitDamage, transform);
        }
        Destroy(gameObject);
    }
    
}

