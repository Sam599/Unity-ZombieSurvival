using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IKillableObjects
{
    [SerializeField] GameObject playerSkin;

    private PlayerMovementManager playerMovement;
    private CharactersStatus characterStatus;
    private AnimationManager animationManager;
    private Vector3 movement;
    public GameController gameController;
    public GameObject equippedWeapon;
    public UnityEvent OnPlayerDeath;
    public LayerMask aimingLayer;
    private bool playerAttacked;
    private bool playerGrabbed;
    private float initialSpeed;
    private bool isMale;

    void Start()
    {
        animationManager = GetComponent<AnimationManager>();
        playerMovement = GetComponent<PlayerMovementManager>();
        characterStatus = GetComponent<CharactersStatus>();
        EnemyController.onPlayerGrab += PlayerGrabbed;

        SetPlayerSkin();
        gameController.lifeBar.maxValue = characterStatus.maxHealthPoints;
        gameController.UpdateLifeBar(characterStatus.currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        float axisX = Input.GetAxisRaw("Horizontal");
        float axisZ = Input.GetAxisRaw("Vertical");

        movement = new Vector3(axisX, 0, axisZ).normalized;
        animationManager.MovementAnim(movement.magnitude);

        if (Input.GetButtonDown("Fire1") && Time.timeScale != 0)
        {
            playerAttacked = true;
        }
    }

    private void FixedUpdate()
    {
        animationManager.ShootAnim(false);
        playerMovement.Move(movement, characterStatus.speed);
        Attack();
        PlayerWalking();
        playerMovement.PlayerRotate(aimingLayer);
    }

    public void TakeHit(int hitDamage, Transform objectHit)
    {
        characterStatus.currentHealth -= hitDamage;
        gameController.UpdateLifeBar(characterStatus.currentHealth);
        AudioController.instance.PlayPlayerHitSound(isMale);
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
        if (playerAttacked && equippedWeapon.GetComponent<WeaponController>().ShootWeapon())
        {
            animationManager.ShootAnim(true);
            playerAttacked = false;
        }
    }

    public void Killed()
    {
        AudioController.instance.PlayPlayerDeathSound(isMale);
        OnPlayerDeath?.Invoke();
        gameController.PlayerDied();
    }

    void PlayerWalking()
    {
        if(movement.magnitude > 0.3f)
        {
            AudioController.instance.PlayFootStepSound();
        }
    }

    void PlayerGrabbed(bool grabbed)
    {
        if(grabbed && !playerGrabbed)
        {
            //Debug.Log("Player Grabbed!");
            initialSpeed = characterStatus.speed;
            characterStatus.speed /= initialSpeed;
            playerGrabbed = true;
        }
        else if(!grabbed && playerGrabbed)
        {
            //Debug.Log("Player UNGrabbed!");
            characterStatus.speed *= initialSpeed;
            playerGrabbed = false;
        }
    }

    void SetPlayerSkin()
    {
        int skinIndex = PlayerPrefs.GetInt("SkinSelected");
        playerSkin.transform.GetChild(skinIndex).gameObject.SetActive(true);
        isMale = playerSkin.transform.GetChild(skinIndex).GetComponent<PlayerSkinGender>().isMale;
    }
}
