using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using pkm.EventManager;
using UnityEditor;

public class Fighter : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public bool isAI = false;

    public Frame frame;
    public Pilot pilot;

    public Part leftShoulder;
    public Part rightShoulder;
    public Part rightArm;
    public Part leftArm;

    public bool canPlay = true;
    public int health = 100;
    public int maxHealth = 100;
    public bool dead = false;

    public bool focused = false;

    public Image healthBar;

    public int player = 1;

    public Skillset skillset;

    public List<Status> buffs = new List<Status>();
    public List<Status> debuffs = new List<Status>();

    public List<Ability> availableAbilities = new List<Ability>();

    private Button focusSelector;
    private Image _sprite;
    private StatsManager _stats;

    private void Awake()
    {
        _sprite = GetComponent<Image>();
        _stats = GetComponent<StatsManager>();
        focusSelector = GetComponent<Button>();
        focusSelector.enabled = false;

        if(skillset == null)
        {
            skillset = ScriptableObject.CreateInstance<Skillset>();
        }
    }

    private void Start()
    {
        UpdateAvailableAbilities();


        /// DEBUG ///
        // skillset.DebugAbilities();
        for (int ii = 0; ii < availableAbilities.Count; ii++)
        {
            Debug.Log(availableAbilities[ii].abilityName);
        }
        EventManager.TriggerEvent(EventList.FIGHTER_STUFF_UPDATE.ToString(), new { fighter = this });
        Debug.Log("-----------------------------------");

        skillset._abilities[0] = availableAbilities[0];
        skillset._abilities[1] = availableAbilities[1];
        skillset._abilities[2] = availableAbilities[4];
        skillset._abilities[3] = availableAbilities[5];
    }

    public void ChangeAbility(int abilityIndex, Ability ability)
    {
        if(skillset._abilities[abilityIndex] != ability)
        {
            skillset._abilities[abilityIndex] = ability;
        }
    }

    public void ChangePart(PartSlot slot, Part newPart)
    {
        // TODO : Put old part to inventory

        
        // Set new part and refresh fighter abilties and stats
        Part changedPart = null;
        switch (slot)
        {
            case PartSlot.LEFT_ARM:
                changedPart = leftArm;
                leftArm = newPart;
                break;
            case PartSlot.RIGHT_ARM:
                changedPart = rightArm;
                rightArm = newPart;
                break;
            case PartSlot.LEFT_SHOULDER:
                changedPart = leftShoulder;
                leftShoulder = newPart;
                break;
            case PartSlot.RIGHT_SHOULDER:
                changedPart = rightShoulder;
                rightShoulder = newPart;
                break;
        }

        // Remove old abilities from skillset if used
        int abilityIndex = -1;
        for (int ii = 0; ii < 2; ii++)
        {
            abilityIndex = ArrayUtility.IndexOf(skillset._abilities, changedPart.abilities[ii]);
            if (abilityIndex != -1)
            {
                skillset._abilities[abilityIndex] = newPart.abilities[ii];
            }
        }

        // TODO : Handle this trigger somewhere in UI
        EventManager.TriggerEvent(EventList.FIGHTER_STUFF_UPDATE.ToString(), new { fighter = this });
        UpdateAvailableAbilities();

        // TODO : Remove new part of the inventory
    }

    public void AddBuff(Status buff)
    {
        buffs.Add(Instantiate(buff));
    }

    public void AddDebuff(Status debuff)
    {
        debuffs.Add(Instantiate(debuff));
    }

    public IEnumerator ApplyStatuses()
    {
        // Manage buff
        if (buffs.Count > 0)
        {
            foreach (Status buff in buffs)
            {
                yield return StartCoroutine(buff.Apply(this));
            }
        }

        // Remove all done buff
        buffs.RemoveAll(e => e.isDone == true);

        // Manage debuff
        if (debuffs.Count > 0)
        {
            foreach (Status debuff in debuffs)
            {
                yield return StartCoroutine(debuff.Apply(this));

                if (dead)
                {
                    break;
                }
            }
        }

        // Remove all done debuff
        debuffs.RemoveAll(e => e.isDone == true);
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

        health = Mathf.Clamp(health, 0, maxHealth);
        yield return StartCoroutine(HealthBarAnimation());
    }

    public IEnumerator TakeDamage(int amount)
    {
        health -= amount;

        StartCoroutine(Blink());
        yield return StartCoroutine(HealthBarAnimation());

        if (health <= 0)
        {
            healthBar.transform.parent.gameObject.SetActive(false);
            dead = true;

            _sprite.color = new Color(0, 0, 0, 0);
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

    private void UpdateAvailableAbilities()
    {
        // TODO : Maybe this size will change, find a better way to handle this or fix game design to be sure
        List<Ability> abilities = new List<Ability>();

        if (leftShoulder != null)
            abilities.AddRange(leftShoulder.abilities);

        if (rightShoulder != null)
            abilities.AddRange(rightShoulder.abilities);

        if (leftArm != null)
            abilities.AddRange(leftArm.abilities);

        if (rightArm != null)
            abilities.AddRange(rightArm.abilities);

        availableAbilities = abilities;
    }

    private IEnumerator HealthBarAnimation()
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
