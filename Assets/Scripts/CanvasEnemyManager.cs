using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasEnemyManager : MonoBehaviour
{
    public Slider charLifeBar;
    public GameObject character;

    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);        
    }

    public void UpdateLifeBar()
    {

    }
}
