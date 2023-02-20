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
        Debug.Log(other == null);
        switch (other.tag)
        {
            case "Enemy":
                other.GetComponent<EnemyController>().TakeHit(bulletStatus.hitDamage,transform);
                break;
            case "Boss":
                other.GetComponent<BossController>().TakeHit(bulletStatus.hitDamage,transform);
                break;
        }
        Destroy(gameObject);
    }
}
