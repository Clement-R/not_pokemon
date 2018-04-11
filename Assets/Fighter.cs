using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour {

    public bool canPlay = true;
    public int health = 100;
    public bool dead = false;

    public bool focused = false;

    public Image healthBar;

    public int player = 1;

    private Button focusSelector;

    public void Start()
    {
        focusSelector = GetComponent<Button>();
        focusSelector.enabled = false;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        StartCoroutine(LoseHealth());

        //healthBar.fillAmount = (float)health / 100f;
        //print(healthBar.fillAmount);

        if(health <= 0)
        {
            dead = true;
        }
    }

    public void ChangeFocus(bool canBeFocused)
    {
        focusSelector.enabled = canBeFocused;
    }

    IEnumerator LoseHealth()
    {
        float start = healthBar.fillAmount;
        float end = (float)health / 100f;

        float t = 0;

        while (true)
        {
            t += Time.deltaTime;
            healthBar.fillAmount = Mathf.Lerp(start, end, t);
            yield return null;
        }

        if(health <= 0)
        {
            dead = true;
        }
    }
}
