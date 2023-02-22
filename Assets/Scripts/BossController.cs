using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour , IKillableObjects
{
    public GameObject medkitPrefab;
    public Slider bossLifeBar;
    public GameObject zombieBlood;
    private NavMeshAgent agent;
    private Transform player;
    private AnimationManager animationManager;
    private GameController gameController;
    private CharactersStatus characterStatus;

    private int bossHitCounter = 3;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        animationManager = GetComponent<AnimationManager>();
        characterStatus = GetComponent<CharactersStatus>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        AudioController.instance.PlayBossSpawnSound();
    }

    private void Update()
    {
        if(characterStatus.currentHealth > 0)
        {
            agent.SetDestination(player.position);
            animationManager.MovementAnim(agent.velocity.magnitude);
            
            if (agent.hasPath && agent.remainingDistance <= agent.stoppingDistance) {
                Debug.Log(agent.remainingDistance);
                animationManager.AttackAnim(true);
                Vector3 playerDirection = player.position - transform.position;
                transform.rotation = Quaternion.LookRotation(playerDirection);
            } else animationManager.AttackAnim(false);
        }
        //Debug.Log("Boss has path: " + agent.hasPath + ", Boss Distance: " + agent.remainingDistance);
    }
    public void TakeHit(int hitDamage, Transform objectHit)
    {
        characterStatus.currentHealth -= hitDamage;
        Instantiate(zombieBlood, objectHit.position, Quaternion.LookRotation(-objectHit.forward));
        PlayHitSound();
        if (characterStatus.currentHealth <= 0)
        {
            Killed();
        }
    }

    void FallToDeath()
    {
        AudioController.instance.PlayZombieFallSound();
    }

    void PlayerHit()
    {
        AudioController.instance.PlayBossAttackSound();
        player.gameObject.GetComponent<PlayerController>().TakeHit(Random.Range(characterStatus.hitDamage - 15, characterStatus.hitDamage), transform);
    }

    void PlayHitSound()
    {
        if(bossHitCounter > 3)
        {
            AudioController.instance.PlayBossHitSound();
            bossHitCounter = 0;
        }
        bossHitCounter++;
    }

    public void Killed()
    {
        agent.ResetPath();
        AudioController.instance.PlayBossDeathSound();
        //Debug.Log("Zombie Killed!");
        Instantiate(medkitPrefab, transform.position, Quaternion.identity);
        gameController.EnemyKilled(this.tag);
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        animationManager.DyingAnim();

        Destroy(gameObject, 2);
    }

}
