using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour
{
    GameObject player;
    GameController gameController;
    public GameObject zombiePrefab;
    public GameObject bossPrefab;
    public LayerMask zombieLayerMask;
    int zombieGenerateInterval;
    float generateCounter = 0;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        zombieGenerateInterval = Random.Range(gameController.minZombieGenerateInverval, gameController.maxZombieGenerateInverval + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.generatorReady &&
        Vector3.Distance(player.transform.position, transform.position) >= gameController.distanceFromPlayerToSpawn &&
        gameController.numZombieAlive < gameController.zombieLimit && 
        gameController.stopZombieGenerateInLimit && 
        gameController.zombieLimit != gameController.numZombieGenerated)
        {
            generateCounter += Time.deltaTime;
            //Debug.Log(zombieGenerateInterval);

            if (generateCounter >= zombieGenerateInterval)
            {
                if (gameController.spawnZombieBoss) GenerateZombie(bossPrefab, true);
                else GenerateZombie(zombiePrefab, false);

                zombieGenerateInterval = Random.Range(gameController.minZombieGenerateInverval, gameController.maxZombieGenerateInverval + 1);
            }
        } else generateCounter = 0;
    }

    void GenerateZombie(GameObject prefabSpawn, bool isBoss)
    {
        Collider[] zombieColliders = Physics.OverlapSphere(transform.position, gameController.zombieSpawnRadius, zombieLayerMask);
        if (zombieColliders.Length < 1)
        {
            Instantiate(prefabSpawn, transform.position, transform.rotation);
            if (isBoss)
            {
                Debug.Log("Boss Generated!");
                gameController.BossSpawned();
            }
            else
            {
                //Debug.Log("Zombie Generated!");
                gameController.numZombieAlive++;
                gameController.numZombieGenerated++;
            }
            generateCounter = 0;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3);
    }
}
