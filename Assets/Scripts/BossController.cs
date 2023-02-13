using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour , IKillableObjects
{
    public GameObject medkitPrefab;
    public Slider bossLifeBar;
    private NavMeshAgent agent;
    private Transform player;
    private AnimationManager animationManager;
    private GameController gameController;
    private CharactersStatus characterStatus;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        animationManager = GetComponent<AnimationManager>();
        characterStatus = GetComponent<CharactersStatus>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void Update()
    {
        if(characterStatus.currentHealth > 0)
        {
            agent.SetDestination(player.position);
            animationManager.MovementAnim(agent.velocity.magnitude);

            if (agent.hasPath && agent.remainingDistance <= agent.stoppingDistance) {
                animationManager.AttackAnim(true);
                Vector3 playerDirection = player.position - transform.position;
                transform.rotation = Quaternion.LookRotation(playerDirection);
            } else animationManager.AttackAnim(false);
        }
        //Debug.Log("Boss has path: " + agent.hasPath + ", Boss Distance: " + agent.remainingDistance);
    }
    public void TakeHit(int hitDamage)
    {
        characterStatus.currentHealth -= hitDamage;
        if (characterStatus.currentHealth <= 0)
        {
            Killed();
        }
    }
    void PlayerHit()
    {
        player.gameObject.GetComponent<PlayerController>().TakeHit(Random.Range(characterStatus.hitDamage - 15, characterStatus.hitDamage));
    }

    public void Killed()
    {
        agent.ResetPath();
        AudioController.instance.PlayZombieDeathSound();
        //Debug.Log("Zombie Killed!");
        Instantiate(medkitPrefab, transform.position, Quaternion.identity);
        gameController.EnemyKilled(this.tag);
        gameController.IncreaseKillCounter();
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        animationManager.DyingAnim();

        Destroy(gameObject, 2);
    }

}
