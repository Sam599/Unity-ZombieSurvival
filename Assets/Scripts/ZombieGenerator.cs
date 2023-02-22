using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] LayerMask zombieLayerMask;

    GameObject player;
    GameController gameController;
    int zombieGenerateInterval;
    float generateCounter = 0;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        zombieGenerateInterval = Random.Range(gameController.minZombieGenerateInverval, gameController.maxZombieGenerateInverval + 1);
    }

    void Update()
    {
        if (gameController.generatorReady &&
        Vector3.Distance(player.transform.position, transform.position) >= gameController.distanceFromPlayerToSpawn &&
        gameController.numZombieAlive < gameController.zombieLimit && 
        gameController.stopZombieGenerateInLimit && 
        gameController.zombieLimit != gameController.numZombieGenerated)
        {
            generateCounter += Time.deltaTime;

            if (generateCounter >= zombieGenerateInterval)
            {
                GameObject enemyToGenerate;

                if (gameController.spawnZombieBoss) enemyToGenerate = bossPrefab;
                else enemyToGenerate = zombiePrefab;

                GenerateZombie(enemyToGenerate);

                zombieGenerateInterval = Random.Range(gameController.minZombieGenerateInverval, gameController.maxZombieGenerateInverval + 1);
            }
        } else generateCounter = 0;
    }

    void GenerateZombie(GameObject prefabSpawn)
    {
        Collider[] zombieColliders = Physics.OverlapSphere(transform.position, gameController.zombieSpawnRadius, zombieLayerMask);
        if (zombieColliders.Length < 1)
        {
            GameObject spawnedEnemy = Instantiate(prefabSpawn, transform.position, transform.rotation);
            
            if (prefabSpawn.GetComponent<CharactersStatus>().isBoss)
            {
                gameController.BossSpawned();
            }
            else
            {
                gameController.numZombieAlive++;
                gameController.numZombieGenerated++;
            }

        }
        generateCounter = 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3);
    }
}
