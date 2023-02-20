using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour, IKillableObjects
{
    GameObject player;
    GameController gameController;
    public GameObject medkitPrefab;
    public GameObject zombieBlood;
    public float wanderInterval;
    public float itemDropChance = 0.2f;
    float wanderPositionTime;
    float playerDistance;
    bool playerSpotted;

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
        playerDistance = Vector3.Distance(player.transform.position, transform.position);
        RandomizeZombieSkin();
    }
    void FixedUpdate()
    {
        MoveRotateEnemy(player.transform.position);
    }

    void FallToDeath()
    {
        AudioController.instance.PlayZombieFallSound();
        Debug.Log("Play Fall Sound");
    }

    void PlayerHit() //Animation Event
    {
        player.GetComponent<PlayerController>().TakeHit(characterStatus.hitDamage, transform);
    }

    public void TakeHit(int hitDamage, Transform objectHit)
    {
        characterStatus.currentHealth -= hitDamage;
        Instantiate(zombieBlood, objectHit.position, Quaternion.LookRotation(-objectHit.forward));
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
        if (characterStatus.currentHealth > 0)
        {
            playerDistance = Vector3.Distance(player.transform.position, transform.position);
            Vector3 endPosition = position - transform.position;
            animationManager.MovementAnim(movementManager.GetObjectVelocity());

            if (playerDistance > 15)
            {
                Wander();
            }
            else if (playerDistance >= 2.5)
            {
                animationManager.AttackAnim(false);
                PlayerSpotted();
                movementManager.Move(endPosition.normalized, characterStatus.speed);
            }
            else
            {
                //Debug.Log("Attack! Distance: " + playerDistance);
                movementManager.Rotate(endPosition);
                animationManager.AttackAnim(true);
            }
        }
        else movementManager.Move(transform.forward, 0);
    }

    void Wander()
    {
        wanderPositionTime += Time.deltaTime;

        if (wanderPositionTime >= wanderInterval)
        {
            Vector3 wanderPosition = GenerateRandomLocation(transform.position);
            movementManager.Move(wanderPosition.normalized, characterStatus.speed);
            wanderPositionTime = 0;
        }
    }

    void PlayerSpotted()
    {
        if (!playerSpotted)
        {
            AudioController.instance.PlayZombieSpottedSound();
            playerSpotted = true;
        }
    }

    Vector3 GenerateRandomLocation(Vector3 position)
    {
        Vector3 randomLocation = position - Random.insideUnitSphere * 5;
        randomLocation.y = position.y;

        return randomLocation;
    }
}
