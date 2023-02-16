using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersStatus : MonoBehaviour
{
    public int maxHealthPoints;
    [HideInInspector] 
    public int currentHealth;
    public float speed;
    public int hitDamage;

    void Awake()
    {
        currentHealth = maxHealthPoints;
    }
}
