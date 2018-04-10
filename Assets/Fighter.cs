using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour {

    public bool canPlay = true;
    public int health = 100;
    public bool dead = false;

    public Image healthBar;

    public int player = 1;
    
    public void TakeDamage(int amount)
    {
        health -= amount;

        healthBar.fillAmount = (float)health / 100f;
        print(healthBar.fillAmount);

        if(health <= 0)
        {
            dead = true;
        }
    }
}
