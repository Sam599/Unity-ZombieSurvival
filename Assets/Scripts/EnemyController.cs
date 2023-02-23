using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour, IKillableObjects
{
    [SerializeField] private float wanderInterval;
    [SerializeField] private GameObject medkitPrefab;
    [SerializeField] float itemDropChance;

    GameObject player;
    GameController gameController;
    public GameObject zombieBlood;
    
    float wanderPositionTime;
    float playerDistance;
    bool playerSpotted;
    int attackCounter = 3;

    private AnimationManager animationManager;
    private MovementManager movementManager;
    private CharactersStatus characterStatus;

    public delegate void OnPlayerGrab(bool grab);
    public static event OnPlayerGrab onPlayerGrab;

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
        SetZombiePower();
    }
    void FixedUpdate()
    {
        MoveRotateEnemy(player.transform.position);
    }

    void FallToDeath() //Animation Event
    {
        AudioController.instance.PlayZombieFallSound();
    }

    void PlayerHit() //Animation Event
    {
        player.GetComponent<PlayerController>().TakeHit(characterStatus.hitDamage, transform);
        attackCounter++;
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
        onPlayerGrab?.Invoke(false);
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
                onPlayerGrab?.Invoke(false);
                movementManager.Move(endPosition.normalized, characterStatus.speed);
            }
            else
            {
                movementManager.Rotate(endPosition);
                Attack();
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

    void SetZombiePower()
    {
        switch (gameController.zombiePower)
        {
            case 1:
                characterStatus.speed = 7;
                characterStatus.maxHealthPoints = 2;
                characterStatus.currentHealth = 2;
                break;
            case 2:
                characterStatus.speed = 8;
                characterStatus.maxHealthPoints = 2;
                characterStatus.currentHealth = 2;
                break;
            case 3:
                characterStatus.speed = 9;
                characterStatus.maxHealthPoints = 3;
                characterStatus.currentHealth = 3;
                break;
        }
    }

    void Attack()
    {
        onPlayerGrab?.Invoke(true);
        animationManager.AttackAnim(true);
        if (attackCounter == 3)
        {
            AudioController.instance.PlayZombieAttackSound();
            attackCounter = 0;
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
        Vector3 randomLocation = Random.insideUnitSphere * 5;
        randomLocation.y = position.y;

        return randomLocation;
    }
}
