using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersStatus : MonoBehaviour
{
    public float maxHealthPoints;
    [HideInInspector] 
    public float currentHealth;
    public float speed;
    public int hitDamage;
    public bool isBoss;

    void Awake()
    {
        currentHealth = maxHealthPoints;
    }
}
