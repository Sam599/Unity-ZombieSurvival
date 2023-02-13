using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerController : MonoBehaviour , IKillableObjects
{
    public float speed = 5;
    private AnimationManager animationManager;
    private GameController gameController;
    private CharactersStatus characterStatus;
    public GameObject equippedWeapon;
    public PlayerMovementManager playerMovement;
    public LayerMask aimingLayer;
    private Vector3 movement;

    void Start()
    {
        animationManager = GetComponent<AnimationManager>();
        playerMovement = GetComponent<PlayerMovementManager>();
        characterStatus = GetComponent<CharactersStatus>();

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.lifeBar.maxValue = characterStatus.maxHealthPoints;
        gameController.UpdateLifeBar(characterStatus.currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        float axisX = Input.GetAxisRaw("Horizontal");
        float axisZ = Input.GetAxisRaw("Vertical");

        movement = new Vector3(axisX, 0, axisZ).normalized;

        animationManager.ShootAnim(false);
        animationManager.MovementAnim(movement.magnitude);

        if (Input.GetButtonDown("Fire1") && Time.timeScale != 0)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        playerMovement.Move(movement,speed);
        playerMovement.PlayerRotate(aimingLayer);
    }

    public void TakeHit(int hitDamage)
    {
        characterStatus.currentHealth -= hitDamage;
        gameController.UpdateLifeBar(characterStatus.currentHealth);
        AudioController.instance.PlayPlayerHitSound();
        //Debug.Log("Player Hit: " + characterStatus.currentHealth + "%");
        if (characterStatus.currentHealth <= 0)
        {
            Killed();
        }
    }

    public void RestoreHealth(int healthRestore)
    {
        characterStatus.currentHealth += healthRestore;
        if (characterStatus.currentHealth > characterStatus.maxHealthPoints) characterStatus.currentHealth = characterStatus.maxHealthPoints;
        gameController.UpdateLifeBar(characterStatus.currentHealth);
    }

    public void Attack()
    {
        if (equippedWeapon.GetComponent<WeaponController>().ShootWeapon()) 
            animationManager.ShootAnim(true);
    }

    public void Killed()
    {
        gameController.PlayerDied();
    }
}
