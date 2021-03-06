﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using pkm.EventManager;
using TMPro;

public class CombatManager : MonoBehaviour {
    [Header("Fighters")]
    public Fighter P1_Fighter1;
    public Fighter P1_Fighter2;

    public Fighter P2_Fighter1;
    public Fighter P2_Fighter2;

    public GameObject firstFocusedMove;

    public GameObject playerUI;
    public GameObject combatLog;

    public bool waitForPlayerAction = true;
    public bool waitForPlayerChoice = true;

    public GameObject battlefieldUI;
    
    public TMP_Text debugText;

    public Text[] moves;

    public GameObject damageBubble;

    private bool _combatEnd = false;
    private bool _actionChoosed = false;
    private Ability _choosedAbility = null;
    private bool _targetChoosed = false;

    private List<Fighter> _fighters = new List<Fighter>();
    private Fighter _fighterToAttack = null;

    private TMP_Text _combatLogText;

    private Fighter _activeFighter = null;

    private CombatPhase _actualPhase = CombatPhase.COMBAT_START;
    private bool _coroutineEnd = false;

    public enum CombatPhase
    {
        COMBAT_START,
        TURN_INIT,
        STATUSES,
        WAIT_PLAYER_ACTION_CHOICE,
        WAIT_PLAYER_TARGET_CHOICE,
        WAIT_PLAYER_INPUT,
        AI_ACTION_PHASE,
        ACTION_PHASE,
        COMBAT_END
    }

    public static CombatManager instance
    {
        get
        {
            if (!_combatManager)
            {
                _combatManager = FindObjectOfType(typeof(CombatManager)) as CombatManager;

                if (!_combatManager)
                {
                    Debug.LogError("There needs to be one active CombatManager script on a GameObject in your scene.");
                }
            }

            return _combatManager;
        }
    }
    private static CombatManager _combatManager;

    void Start ()
    {
        _fighters.Add(P1_Fighter1);
        _fighters.Add(P1_Fighter2);

        _fighters.Add(P2_Fighter1);
        _fighters.Add(P2_Fighter2);

        _combatLogText = combatLog.GetComponent<TMP_Text>();
        
        StartCoroutine(Logic());
    }

    private void Update()
    {
        if (_actionChoosed && !_targetChoosed)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                _actionChoosed = false;
            }
        }
    }

    public Fighter GetActiveFighter()
    {
        return _activeFighter;
    }
    
    private string GetCoroutinePhase(CombatPhase phase)
    {
        string routine = null;
        switch (phase)
        {
            case CombatPhase.COMBAT_START:
                routine = "StartCombat";
                break;

            case CombatPhase.TURN_INIT:
                routine = "TurnInitialization";
                break;

            case CombatPhase.STATUSES:
                routine = "ApplyStatuses";
                break;

            case CombatPhase.WAIT_PLAYER_ACTION_CHOICE:
                routine = "WaitForPlayerActionChoice";
                break;

            case CombatPhase.WAIT_PLAYER_TARGET_CHOICE:
                routine = "WaitForPlayerTargetChoice";
                break;

            case CombatPhase.WAIT_PLAYER_INPUT:
                routine = "";
                break;

            case CombatPhase.ACTION_PHASE:
                routine = "PlayAction";
                break;

            case CombatPhase.AI_ACTION_PHASE:
                routine = "PlayAITurn";
                break;

            case CombatPhase.COMBAT_END:
                routine = "CombatEnd";
                break;
        }

        return routine;
    }


    /*
     * Main coroutine that will end at the end of the combat
     * 
     * It will launch a coroutine following the actual state of the game (CombatPhase)
     * by calling the function GetCoroutinePhase and wait for its end (coroutineEnd)
     * before searching the next step until the end of combat.
     */
    IEnumerator Logic()
    {
        while (!_combatEnd)
        {
            Debug.Log("Entering " + _actualPhase);
            yield return StartCoroutine(GetCoroutinePhase(_actualPhase));
        }
    }

    IEnumerator StartCombat()
    {
        yield return null;

        // Going to turn initialization
        _actualPhase = CombatPhase.TURN_INIT;
    }

    IEnumerator TurnInitialization()
    {
        // Choose next fighter, depending of the dexterity stat
        _activeFighter = _fighters.DefaultIfEmpty(null).OrderByDescending(e => e.GetDexterity()).FirstOrDefault(e => e.canPlay == true && e.dead == false);

        // If no fighter can play, reset all fighters and select first one
        if (_activeFighter == null)
        {
            _fighters.FindAll(e => e.dead == false).Select(c => { c.canPlay = true; return c; }).ToList();
            _activeFighter = _fighters.OrderByDescending(e => e.GetDexterity()).First(e => e.canPlay == true && e.dead == false);
        }

        EventManager.TriggerEvent(EventList.SHOW_COMBAT_UI.ToString(), new { });
        EventManager.TriggerEvent(EventList.FIGHTER_STUFF_UPDATE.ToString(), new { fighter = GetActiveFighter()});

        // Check if one team is KO
        if (_fighters.Count(e => e.player != _activeFighter.player && e.dead == false) == 0)
        {
            _actualPhase = CombatPhase.COMBAT_END;
        }
        else if (_fighters.Count(e => e.player == _activeFighter.player && e.dead == false) == 0)
        {
            _actualPhase = CombatPhase.COMBAT_END;
        }
        else
        {
            debugText.text = _activeFighter.name;
            // Going to turn initialization
            _actualPhase = CombatPhase.STATUSES;
        }

        yield return null;
    }

    IEnumerator ApplyStatuses()
    {
        // Apply buff and debuff
        yield return StartCoroutine(_activeFighter.ApplyStatuses());

        // IF a debuff killed the actual fighter we go back
        // to turn intialization
        if (_activeFighter.dead)
        {
            _actualPhase = CombatPhase.TURN_INIT;
        }
        else
        {
            if(!_activeFighter.isAI)
            {
                _actualPhase = CombatPhase.WAIT_PLAYER_ACTION_CHOICE;
            }
            else
            {
                _actualPhase = CombatPhase.AI_ACTION_PHASE;
            }
        }
    }

    IEnumerator WaitForPlayerActionChoice()
    {
        EventSystem.current.SetSelectedGameObject(firstFocusedMove);
        firstFocusedMove.GetComponent<ButtonColorFocus>().Focus();

        waitForPlayerAction = true;
        // waitForPlayerAction will be updated by the buttons
        // calling NotifyPlayerActionChoosed method
        while (waitForPlayerAction)
        {
            yield return null;
        }

        print(_choosedAbility.abilityName);
        
        _actualPhase = CombatPhase.WAIT_PLAYER_TARGET_CHOICE;
    }

    /*
     * Set the focus on the enemy or the ally team
     * and on a single or multi target
     */
    private void SetFocus(Ability.AbilityTarget target, Ability.AbilityType type)
    {
        // Wait for player enemy target choice
        playerUI.SetActive(false);

        if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
        {
            if (_choosedAbility.type == Ability.AbilityType.ATTACK)
            {
                _fighters.FindAll(e => e.player != _activeFighter.player && e.dead == false).Select(e => { e.ForceSelect(); return e; }).ToList();
            }
            else
            {
                _fighters.FindAll(e => e.player == _activeFighter.player && e.dead == false).Select(e => { e.ForceSelect(); return e; }).ToList();
            }
        }
        else
        {
            if (_choosedAbility.type == Ability.AbilityType.ATTACK)
            {
                // Set enemies focusable
                _fighters.FindAll(e => e.player != _activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(true); return e; }).ToList();
                // Set first ally as focused
                EventSystem.current.SetSelectedGameObject(_fighters.First(e => e.player != _activeFighter.player && e.dead == false).gameObject);
            }
            else
            {
                // Set allies focusable
                _fighters.FindAll(e => e.player == _activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(true); return e; }).ToList();
                // Set first ally as focused
                EventSystem.current.SetSelectedGameObject(_fighters.First(e => e.player == _activeFighter.player && e.dead == false).gameObject);
            }
        }
    }

    private void ClearFocus()
    {
        _fighters.FindAll(e => e.dead == false).Select(e => { e.ChangeFocus(false); return e; }).ToList();

        // Set UI in turn begin state
        EventSystem.current.SetSelectedGameObject(firstFocusedMove);
        firstFocusedMove.GetComponent<ButtonColorFocus>().Focus();
        playerUI.SetActive(true);
    }

    IEnumerator WaitForPlayerTargetChoice()
    {
        // Add some delay with previous action to avoid
        // an unwanted action due to fast input detection
        yield return null;
        yield return null;
        yield return null;

        _targetChoosed = false;
        SetFocus(_choosedAbility.target, _choosedAbility.type);

        bool playerCancel = false;

        // Wait for player action (cancel or validatinon)
        if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
        {
            while (!_targetChoosed && !playerCancel)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _targetChoosed = true;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    playerCancel = true;
                }

                yield return null;
            }
        }
        else
        {
            while (!_targetChoosed && !playerCancel)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    playerCancel = true;
                }

                yield return null;
            }
        }

        ClearFocus();

        if (playerCancel)
        {
            _actualPhase = CombatPhase.WAIT_PLAYER_ACTION_CHOICE;
        }
        else
        {
            _actualPhase = CombatPhase.ACTION_PHASE;
        }
    }

    IEnumerator PlayAITurn()
    {
        yield return null;

        _activeFighter.canPlay = false;
        // DEBUG
        _actualPhase = CombatPhase.TURN_INIT;

        //_actualPhase = CombatPhase.ACTION_PHASE;
    }

    private IEnumerator ExecuteAbility()
    {
        Coroutine effect = null;
        string log = "";

        if (_choosedAbility.type == Ability.AbilityType.ATTACK)
        {
            if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
            {
                foreach (Fighter fighter in _fighters.FindAll(e => e.player != _activeFighter.player && e.dead == false))
                {
                    effect = StartCoroutine(fighter.TakeDamage(_choosedAbility.damage));

                    // TODO : clean that and move to a different component
                    GameObject bubble = Instantiate(damageBubble, new Vector3(fighter.transform.position.x, fighter.transform.position.y, 0f), Quaternion.identity, _activeFighter.transform.parent.parent);
                    bubble.GetComponentInChildren<DamageTextAnimation>().ChangeText(_choosedAbility.damage.ToString());
                }

                log = _activeFighter.name + " attack the enemies with " + _choosedAbility.abilityName;
            }
            else
            {
                effect = StartCoroutine(_fighterToAttack.TakeDamage(_choosedAbility.damage));

                // TODO : clean that and move to a different component
                GameObject bubble = Instantiate(damageBubble, new Vector3(_fighterToAttack.transform.position.x, _fighterToAttack.transform.position.y, 0f), Quaternion.identity, _activeFighter.transform.parent.parent);
                bubble.GetComponentInChildren<DamageTextAnimation>().ChangeText(_choosedAbility.damage.ToString());

                log = _activeFighter.name + " attack " + _fighterToAttack.name + " with " + _choosedAbility.abilityName;
            }
        }
        else
        {
            if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
            {
                foreach (Fighter fighter in _fighters.FindAll(e => e.player == _activeFighter.player && e.dead == false))
                {
                    effect = StartCoroutine(fighter.Heal(_choosedAbility.heal));
                }

                log = _activeFighter.name + " heal all alies with " + _choosedAbility.abilityName;
            }
            else
            {
                effect = StartCoroutine(_fighterToAttack.Heal(_choosedAbility.heal));

                log = _activeFighter.name + " heal " + _fighterToAttack.name + " with " + _choosedAbility.abilityName;
            }
        }

        // Add debuff
        foreach (Status debuff in _choosedAbility.debuffs)
        {
            if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
            {
                foreach (Fighter enemy in _fighters.FindAll(e => e.player != _activeFighter.player && e.dead == false))
                {
                    enemy.AddDebuff(debuff);
                }
            }
            else
            {
                _fighterToAttack.AddDebuff(debuff);
            }
        }

        // Add buff
        foreach (Status buff in _choosedAbility.buffs)
        {
            if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
            {
                foreach (Fighter ally in _fighters.FindAll(e => e.player == _activeFighter.player && e.dead == false))
                {
                    ally.AddBuff(buff);
                }
            }
            else
            {
                _activeFighter.AddBuff(buff);
            }
        }

        // Display combat log
        EventManager.TriggerEvent(EventList.DISPLAY_TEXT.ToString(), new { text = log });

        while (!CombatLogManager.instance.doneDisplaying)
        {
            yield return null;
        }

        yield return effect;
    }

    IEnumerator PlayAction()
    {
        // Play attack
        Camera.main.GetComponent<Screenshake>().ScreenShake();

        // Play ability animation
        GameObject effect = Instantiate(_choosedAbility.effect);
        Destroy(effect, 2f);

        yield return StartCoroutine(ExecuteAbility());

        _activeFighter.canPlay = false;
        _actualPhase = CombatPhase.TURN_INIT;
    }

    IEnumerator CombatEnd()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("Map");
        op.allowSceneActivation = false;
        yield return StartCoroutine(Camera.main.GetComponent<TransitionPostProcess>().TransitionCoroutine());
        _combatEnd = true;
        op.allowSceneActivation = true;
    }

    //IEnumerator CombatLogic()
    //{
    //    while (!_combatEnd)
    //    {

    //        // TURN_INIT

    //        // Choose next fighter, depending of the dexterity stat
    //        _activeFighter = _fighters.DefaultIfEmpty(null).OrderByDescending(e => e.GetDexterity()).FirstOrDefault(e => e.canPlay == true && e.dead == false);

    //        // If no fighter can play, reset all fighters and select first one
    //        if (_activeFighter == null)
    //        {
    //            _fighters.FindAll(e => e.dead == false).Select(c => { c.canPlay = true; return c; }).ToList();
    //            _activeFighter = _fighters.OrderByDescending(e => e.GetDexterity()).First(e => e.canPlay == true && e.dead == false);
    //        }
            
    //        debugText.text = _activeFighter.name;
            
    //        bool turnEnd = false;

    //        // Apply buff and debuff
    //        yield return _activeFighter.ApplyStatuses();

    //        if(_activeFighter.dead)
    //        {
    //            print("Active fighter dead");
    //            turnEnd = true;
    //        }

    //        if (_activeFighter.isAI && !_activeFighter.dead)
    //        {
    //            playerUI.SetActive(false);

    //            // TODO : Take enemy action

    //            // TODO : Select first enemy
    //            // fighterToAttack = _fighters.FindAll(e => e.player != activeFighter.player && e.dead == false).OrderBy(x => Random.Range(0, 10)).First();
                
    //            // Take enemy with max health
    //            _fighterToAttack = _fighters.FindAll(e => e.player != _activeFighter.player && e.dead == false).OrderByDescending(e => e.health).First();

    //            _activeFighter.canPlay = false;

    //            // Apply attack
    //            // Show log
    //            _combatLogText.text = "";

    //            // Play attack
    //            Camera.main.GetComponent<Screenshake>().ScreenShake();

    //            StartCoroutine(_fighterToAttack.TakeDamage(_activeFighter.skillset.GetAbility(0).damage));
    //            _combatLogText.text = _activeFighter.name + " attack " + _fighterToAttack.name;

    //            Coroutine corCol = StartCoroutine(_combatLogText.gameObject.GetComponent<FadeInText>().AnimateVertexColors());

    //            waitForPlayerAction = true;
    //            while (waitForPlayerAction)
    //            {
    //                yield return null;

    //                if (Input.anyKeyDown)
    //                {
    //                    waitForPlayerAction = false;
    //                }
    //            }

    //            StopCoroutine(corCol);
    //            _combatLogText.text = "";

    //            // Check if the other team is dead
    //            if (_fighters.Count(e => e.player != _activeFighter.player && e.dead == false) == 0)
    //            {
    //                _combatEnd = true;
    //            }

    //            EventSystem.current.SetSelectedGameObject(firstFocusedMove);
    //            firstFocusedMove.GetComponent<ButtonColorFocus>().Focus();

    //            playerUI.SetActive(true);

    //            _targetChoosed = false;
    //            _actionChoosed = false;
    //            turnEnd = true;
    //        }
    //        else if(_activeFighter.isAI && _activeFighter.dead)
    //        {
    //            print("Dead AI");
    //            turnEnd = true;
    //        }

    //        while (!turnEnd)
    //        {
    //            if (!_actionChoosed)
    //            {
    //                // Wait for player action choice
    //                waitForPlayerAction = true;
    //                while (waitForPlayerAction)
    //                {
    //                    yield return null;
    //                }
    //            }
    //            else
    //            {
    //                if (!_targetChoosed)
    //                {
    //                    _activeFighter.canPlay = false;

    //                    // Wait for player enemy target choice
    //                    playerUI.SetActive(false);

    //                    if(_choosedAbility.type == Ability.AbilityType.ATTACK)
    //                    {
    //                        if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
    //                        {
    //                            _fighters.FindAll(e => e.player != _activeFighter.player && e.dead == false).Select(e => { e.ForceSelect(); return e; }).ToList();

    //                            waitForPlayerChoice = true;
    //                            while (waitForPlayerChoice && _actionChoosed)
    //                            {
    //                                yield return null;

    //                                if (Input.GetKeyDown(KeyCode.Space))
    //                                {
    //                                    waitForPlayerChoice = false;
    //                                    _targetChoosed = true;
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            // Set enemy team to be focusable
    //                            _fighters.FindAll(e => e.player != _activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(true); return e; }).ToList();

    //                            // Set first enemy as focused
    //                            EventSystem.current.SetSelectedGameObject(_fighters.First(e => e.player != _activeFighter.player && e.dead == false).gameObject);

    //                            waitForPlayerChoice = true;
    //                            while (waitForPlayerChoice && _actionChoosed)
    //                            {
    //                                yield return null;
    //                            }
    //                        }

    //                        if (_targetChoosed)
    //                        {
    //                            // TODO : Remove action logic from here
    //                            // Show log
    //                            _combatLogText.text = "";


    //                            // Play attack
    //                            Camera.main.GetComponent<Screenshake>().ScreenShake();

    //                            if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
    //                            {
    //                                foreach (Fighter fighter in _fighters.FindAll(e => e.player != _activeFighter.player && e.dead == false))
    //                                {
    //                                    StartCoroutine(fighter.TakeDamage(_choosedAbility.damage));
    //                                }
    //                            }
    //                            else
    //                            {
    //                                StartCoroutine(_fighterToAttack.TakeDamage(_choosedAbility.damage));
    //                            }

    //                            // Play ability animation
    //                            GameObject effect = Instantiate(_choosedAbility.effect);
    //                            Destroy(effect, 2f);

    //                            // Add debuff
    //                            foreach (Status debuff in _choosedAbility.debuffs)
    //                            {
    //                                if(_choosedAbility.target == Ability.AbilityTarget.MULTI)
    //                                {
    //                                    foreach (Fighter enemy in _fighters.FindAll(e => e.player != _activeFighter.player && e.dead == false))
    //                                    {
    //                                        enemy.AddDebuff(debuff);
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    _fighterToAttack.AddDebuff(debuff);
    //                                }
    //                            }

    //                            // Add buff
    //                            foreach (Status buff in _choosedAbility.buffs)
    //                            {
    //                                if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
    //                                {
    //                                    foreach (Fighter ally in _fighters.FindAll(e => e.player == _activeFighter.player && e.dead == false))
    //                                    {
    //                                        ally.AddBuff(buff);
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    _activeFighter.AddBuff(buff);
    //                                }
    //                            }

    //                            // Display combat log and wait for the player to press a key
    //                            if (_choosedAbility.target == Ability.AbilityTarget.SINGLE)
    //                            {
    //                                _combatLogText.text = _activeFighter.name + " attack " + _fighterToAttack.name + " with " + _choosedAbility.abilityName;
    //                            }
    //                            else
    //                            {
    //                                _combatLogText.text = _activeFighter.name + " attack the enemies with " + _choosedAbility.abilityName;
    //                            }

    //                            Coroutine corCol = StartCoroutine(_combatLogText.gameObject.GetComponent<FadeInText>().AnimateVertexColors());
    //                            // Coroutine corMov = StartCoroutine(_combatLogText.gameObject.GetComponent<FadeInText>().AnimateVertex());

    //                            waitForPlayerAction = true;
    //                            while (waitForPlayerAction)
    //                            {
    //                                yield return null;

    //                                if (Input.anyKeyDown)
    //                                {
    //                                    waitForPlayerAction = false;
    //                                }
    //                            }

    //                            StopCoroutine(corCol);
    //                            //StopCoroutine(corMov);
    //                            _combatLogText.text = "";

    //                            // Remove focus on enemy team
    //                            _fighters.FindAll(e => e.player != _activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(false); return e; }).ToList();

    //                            // Check if the other team is dead
    //                            if (_fighters.Count(e => e.player != _activeFighter.player && e.dead == false) == 0)
    //                            {
    //                                _combatEnd = true;
    //                            }

    //                            EventSystem.current.SetSelectedGameObject(firstFocusedMove);
    //                            firstFocusedMove.GetComponent<ButtonColorFocus>().Focus();

    //                            playerUI.SetActive(true);

    //                            _targetChoosed = false;
    //                            _actionChoosed = false;
    //                            turnEnd = true;
    //                        }
    //                        else
    //                        {
    //                            // Remove focus on enemy team
    //                            _fighters.FindAll(e => e.player != _activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(false); return e; }).ToList();

    //                            EventSystem.current.SetSelectedGameObject(firstFocusedMove);
    //                            firstFocusedMove.GetComponent<ButtonColorFocus>().Focus();

    //                            playerUI.SetActive(true);
    //                        }
    //                    }
    //                    else if(_choosedAbility.type == Ability.AbilityType.SUPPORT)
    //                    {
    //                        if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
    //                        {
    //                            _fighters.FindAll(e => e.player == _activeFighter.player && e.dead == false).Select(e => { e.ForceSelect(); return e; }).ToList();

    //                            waitForPlayerChoice = true;
    //                            while (waitForPlayerChoice && _actionChoosed)
    //                            {
    //                                yield return null;

    //                                if (Input.GetKeyDown(KeyCode.Space))
    //                                {
    //                                    waitForPlayerChoice = false;
    //                                    _targetChoosed = true;
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            // Set enemy team to be focusable
    //                            _fighters.FindAll(e => e.player == _activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(true); return e; }).ToList();

    //                            // Set first ally as focused
    //                            EventSystem.current.SetSelectedGameObject(_fighters.First(e => e.player == _activeFighter.player && e.dead == false).gameObject);

    //                            waitForPlayerChoice = true;
    //                            while (waitForPlayerChoice && _actionChoosed)
    //                            {
    //                                yield return null;
    //                            }
    //                        }

    //                        if (_targetChoosed)
    //                        {
    //                            // TODO : Remove action logic from here
    //                            // Show log
    //                            _combatLogText.text = "";


    //                            // Play ability
    //                            if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
    //                            {
    //                                foreach (Fighter fighter in _fighters.FindAll(e => e.player == _activeFighter.player && e.dead == false))
    //                                {
    //                                    StartCoroutine(fighter.Heal(_choosedAbility.heal));
    //                                }
    //                            }
    //                            else
    //                            {
    //                                StartCoroutine(_fighterToAttack.Heal(_choosedAbility.heal));
    //                            }

    //                            // Play ability animation
    //                            GameObject effect = Instantiate(_choosedAbility.effect);
    //                            Destroy(effect, 2f);

    //                            // Display combat log and wait for the player to press a key
    //                            if (_choosedAbility.target == Ability.AbilityTarget.SINGLE)
    //                            {
    //                                _combatLogText.text = _activeFighter.name + " heal " + _fighterToAttack.name + " with " + _choosedAbility.abilityName;
    //                            }
    //                            else
    //                            {
    //                                _combatLogText.text = _activeFighter.name + " heal all alies with " + _choosedAbility.abilityName;
    //                            }

    //                            Coroutine corCol = StartCoroutine(_combatLogText.gameObject.GetComponent<FadeInText>().AnimateVertexColors());

    //                            waitForPlayerAction = true;
    //                            while (waitForPlayerAction)
    //                            {
    //                                yield return null;

    //                                if (Input.anyKeyDown)
    //                                {
    //                                    waitForPlayerAction = false;
    //                                }
    //                            }

    //                            StopCoroutine(corCol);
    //                            _combatLogText.text = "";

    //                            // Remove focus on enemy team
    //                            _fighters.FindAll(e => e.player == _activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(false); return e; }).ToList();

    //                            EventSystem.current.SetSelectedGameObject(firstFocusedMove);
    //                            firstFocusedMove.GetComponent<ButtonColorFocus>().Focus();

    //                            playerUI.SetActive(true);

    //                            _targetChoosed = false;
    //                            _actionChoosed = false;
    //                            turnEnd = true;
    //                        }
    //                        else
    //                        {
    //                            // Remove focus on allies
    //                            _fighters.FindAll(e => e.player == _activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(false); return e; }).ToList();

    //                            EventSystem.current.SetSelectedGameObject(firstFocusedMove);
    //                            firstFocusedMove.GetComponent<ButtonColorFocus>().Focus();

    //                            playerUI.SetActive(true);
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        // Check if the other team is dead
    //        if (_fighters.Count(e => e.player != _activeFighter.player && e.dead == false) == 0)
    //        {
    //            _combatEnd = true;
    //        }
    //        else if(_fighters.Count(e => e.player == _activeFighter.player && e.dead == false) == 0)
    //        {
    //            _combatEnd = true;
    //        }
    //    }

    //    Camera.main.GetComponent<TransitionPostProcess>().TransitionEffect();

    //    yield return new WaitForSeconds(2f);
    //}

    public void NotifyPlayerActionChoosed(int abilityIndex)
    {
        waitForPlayerAction = false;
        _choosedAbility = _activeFighter.skillset.GetAbility(abilityIndex);
        _actionChoosed = true;
    }

    public void NotifyPlayerEnemyChoice(Fighter fighterToAttack)
    {
        _fighterToAttack = fighterToAttack;
        waitForPlayerChoice = false;
        _targetChoosed = true;
    }
}
