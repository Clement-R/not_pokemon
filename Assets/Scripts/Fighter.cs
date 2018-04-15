using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Fighter : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool isAI = false;

    public Ability move1;

    public Frame frame;
    public Pilot pilot;

    public Part leftShoulder;
    public Part rightShoulder;
    public Part rightArm;
    public Part leftArm;

    public bool canPlay = true;
    public int health = 100;
    public bool dead = false;

    public bool focused = false;

    public Image healthBar;

    public int player = 1;

    private Button focusSelector;
    private Image _sprite;
    private StatsManager _stats;

    public void Awake()
    {
        _sprite = GetComponent<Image>();
        _stats = GetComponent<StatsManager>();
        focusSelector = GetComponent<Button>();
        focusSelector.enabled = false;
    }

    public int GetDexterity()
    {
        return _stats.ComputeDexterity();
    }

    public int GetStrength()
    {
        return _stats.ComputeStrength();
    }

    public int GetAccuracy()
    {
        return _stats.ComputeAccuracy();
    }

    public IEnumerator Heal(int amount)
    {
        health += amount;
        yield return StartCoroutine(HealthBarAnimation());
    }

    public IEnumerator TakeDamage(int amount)
    {
        health -= amount;

        StartCoroutine(Blink());
        yield return StartCoroutine(HealthBarAnimation());

        if (health <= 0)
        {
            print(name + " is dead");

            healthBar.transform.parent.gameObject.SetActive(false);
            dead = true;
            gameObject.SetActive(false);
        }
    }

    IEnumerator Blink()
    {
        Color baseColor = _sprite.color;
        int numberOfBlink = 2;
        int counter = 0;

        while(counter < numberOfBlink)
        {
            _sprite.color = new Color(0, 0, 0, 0);
            yield return new WaitForSeconds(0.25f);
            _sprite.color = baseColor;
            yield return new WaitForSeconds(0.25f);

            counter++;
        }
    }

    public void ForceSelect()
    {
        _sprite.color = Color.blue;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        _sprite.color = Color.blue;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(focusSelector.enabled)
        {
            _sprite.color = Color.yellow;
        }
        else
        {
            _sprite.color = Color.white;
        }
    }

    public void ChangeFocus(bool canBeFocused)
    {
        focusSelector.enabled = canBeFocused;

        if (canBeFocused)
        {
            _sprite.color = Color.yellow;
        }
        else
        {
            _sprite.color = Color.white;
        }
    }

    IEnumerator HealthBarAnimation()
    {
        float start = healthBar.fillAmount;
        float end = (float)health / 100f;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            healthBar.fillAmount = Mathf.Lerp(start, end, t);
            yield return null;
        }
    }
}
