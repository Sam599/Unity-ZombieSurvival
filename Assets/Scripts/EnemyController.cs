using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour, IKillableObjects
{
    GameObject player;
    GameController gameController;
    public GameObject medkitPrefab;
    public float wanderInterval;
    public float itemDropChance = 0.2f;
    float wanderPositionTime;

    private AnimationManager animationManager;
    private MovementManager movementManager;
    private CharactersStatus characterStatus;

    void Start()
    {
        movementManager = GetComponent<MovementManager>();
        animationManager = GetComponent<AnimationManager>();
        characterStatus = GetComponent<CharactersStatus>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        wanderPositionTime = wanderInterval;
        player = GameObject.FindWithTag("Player");
        RandomizeZombieSkin();
    }

    void FixedUpdate()
    {
        float playerDistance = Vector3.Distance(player.transform.position, transform.position);
        animationManager.MovementAnim(movementManager.GetObjectVelocity());

        if(characterStatus.currentHealth <= 0)
        {
            movementManager.Move(transform.position,0);
        }
        else if (playerDistance > 15)
        {
            Wander();
        }
        else if (playerDistance > 2.5)
        {
            MoveRotateEnemy(player.transform.position);
        }
        else
        {
            MoveRotateEnemy(player.transform.position);
            animationManager.AttackAnim(true);
        }
    }

    void PlayerHit() //Animation Event
    {
        player.GetComponent<PlayerController>().TakeHit(characterStatus.hitDamage);
    }

    public void TakeHit(int hitDamage)
    {
        characterStatus.currentHealth -= hitDamage;
        if (characterStatus.currentHealth <= 0)
        {
            Killed();
        }
    }

    public void Killed()
    {
        AudioController.instance.PlayZombieDeathSound();
        //Debug.Log("Zombie Killed!");
        if (Random.value <= itemDropChance)
        {
            Instantiate(medkitPrefab, transform.position, Quaternion.identity);
        }
        gameController.EnemyKilled(this.tag);
        gameController.IncreaseKillCounter();
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        animationManager.DyingAnim();

        Destroy(gameObject, 2);
    }

    void RandomizeZombieSkin()
    {
        int zombieId = Random.Range(1, transform.childCount);
        transform.GetChild(zombieId).gameObject.SetActive(true);
    }

    void MoveRotateEnemy(Vector3 position)
    {
        Vector3 endPosition = position - transform.position;
        movementManager.Rotate(endPosition);

        if (endPosition.magnitude >= 2.5)
        {
            animationManager.AttackAnim(false);
            movementManager.Move(endPosition.normalized, characterStatus.speed);
        }
    }
    void Wander()
    {
        wanderPositionTime += Time.deltaTime;

        if (wanderPositionTime >= wanderInterval)
        {
            Vector3 wanderPosition = GenerateRandomLocation(transform.position);
            MoveRotateEnemy(wanderPosition);
            wanderPositionTime = 0;
        }
    }

    Vector3 GenerateRandomLocation(Vector3 position)
    {
        Vector3 randomLocation = position - Random.insideUnitSphere * 5;
        randomLocation.y = position.y;

        return randomLocation;
    }
}
