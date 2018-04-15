using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CombatManager : MonoBehaviour {

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

    public bool combatEnd = false;
    
    public TMP_Text debugText;

    private bool _actionChoosed = false;
    private Ability _choosedAbility = null;
    private bool _targetChoosed = false;

    private List<Fighter> _fighters = new List<Fighter>();
    private Fighter fighterToAttack = null;

    private TMPro.TMP_Text _combatLogText;

    private float power = 1f;

    void Start ()
    {
        _fighters.Add(P1_Fighter1);
        _fighters.Add(P1_Fighter2);

        _fighters.Add(P2_Fighter1);
        _fighters.Add(P2_Fighter2);

        _combatLogText = combatLog.GetComponent<TMPro.TMP_Text>();

        StartCoroutine(CombatLogic());
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

    IEnumerator WaitForPlayerChoice()
    {
        yield return null;
    }

    IEnumerator WaitForPlayer()
    {
        yield return null;
    }

    IEnumerator CombatLogic()
    {
        Fighter activeFighter = null;

        while (!combatEnd)
        {
            // TODO : Change for a fighter stats like dexterity
            activeFighter = _fighters.DefaultIfEmpty(null).OrderByDescending(e => e.GetDexterity()).FirstOrDefault(e => e.canPlay == true && e.dead == false);
            // activeFighter = _fighters.DefaultIfEmpty(null).FirstOrDefault(e => e.canPlay == true && e.dead == false);

            // Reset all fighters and select first one
            if (activeFighter == null)
            {
                _fighters.FindAll(e => e.dead == false).Select(c => { c.canPlay = true; return c; }).ToList();
                activeFighter = _fighters.First(e => e.canPlay == true && e.dead == false);
            }
            
            debugText.text = activeFighter.name;

            bool turnEnd = false;

            if (activeFighter.isAI)
            {
                playerUI.SetActive(false);

                // TODO : Take enemy action

                // TODO : Select first enemy
                fighterToAttack = _fighters.FindAll(e => e.player != activeFighter.player && e.dead == false).OrderBy(x => Random.Range(0, 10)).First();

                activeFighter.canPlay = false;

                // Apply attack
                // Show log
                _combatLogText.text = "";

                // Play attack
                Camera.main.GetComponent<Screenshake>().ScreenShake();

                // StartCoroutine(fighterToAttack.TakeDamage(activeFighter.move1.damage));
                // yield return RevealText(activeFighter.name + " attack " + fighterToAttack.name);

                StartCoroutine(fighterToAttack.TakeDamage(activeFighter.move1.damage));
                _combatLogText.text = activeFighter.name + " attack " + fighterToAttack.name;

                Coroutine corCol = StartCoroutine(_combatLogText.gameObject.GetComponent<FadeInText>().AnimateVertexColors());

                waitForPlayerAction = true;
                while (waitForPlayerAction)
                {
                    yield return null;

                    if (Input.anyKeyDown)
                    {
                        waitForPlayerAction = false;
                    }
                }

                StopCoroutine(corCol);
                _combatLogText.text = "";

                // Check if the other team is dead
                if (_fighters.Count(e => e.player != activeFighter.player && e.dead == false) == 0)
                {
                    combatEnd = true;
                }

                EventSystem.current.SetSelectedGameObject(firstFocusedMove);
                firstFocusedMove.GetComponent<ButtonColorFocus>().Focus();

                playerUI.SetActive(true);

                _targetChoosed = false;
                _actionChoosed = false;
                turnEnd = true;
            }

            while (!turnEnd)
            {
                if (!_actionChoosed)
                {
                    // Wait for player action choice
                    waitForPlayerAction = true;
                    while (waitForPlayerAction)
                    {
                        yield return null;
                    }
                }
                else
                {
                    if (!_targetChoosed)
                    {
                        activeFighter.canPlay = false;

                        // Wait for player enemy target choice
                        playerUI.SetActive(false);

                        if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
                        {
                            _fighters.FindAll(e => e.player != activeFighter.player && e.dead == false).Select(e => { e.ForceSelect(); return e; }).ToList();

                            waitForPlayerChoice = true;
                            while (waitForPlayerChoice && _actionChoosed)
                            {
                                yield return null;

                                if (Input.GetKeyDown(KeyCode.Space))
                                {
                                    waitForPlayerChoice = false;
                                    _targetChoosed = true;
                                }
                            }
                        }
                        else
                        {
                            // Set enemy team to be focusable
                            _fighters.FindAll(e => e.player != activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(true); return e; }).ToList();

                            // Set first enemy as focused
                            EventSystem.current.SetSelectedGameObject(_fighters.First(e => e.player != activeFighter.player && e.dead == false).gameObject);

                            waitForPlayerChoice = true;
                            while (waitForPlayerChoice && _actionChoosed)
                            {
                                yield return null;
                            }
                        };

                        if (_targetChoosed)
                        {
                            // TODO : Remove action logic from here
                            // Show log
                            _combatLogText.text = "";


                            // Play attack
                            //StartCoroutine(ScreenShake());
                            Camera.main.GetComponent<Screenshake>().ScreenShake();

                            if (_choosedAbility.target == Ability.AbilityTarget.MULTI)
                            {
                                foreach (Fighter fighter in _fighters.FindAll(e => e.player != activeFighter.player && e.dead == false))
                                {
                                    StartCoroutine(fighter.TakeDamage(_choosedAbility.damage));
                                }
                            }
                            else
                            {
                                StartCoroutine(fighterToAttack.TakeDamage(_choosedAbility.damage));
                            }

                            // Play ability animation
                            Instantiate(_choosedAbility.effect);

                            // Display combat log and wait for the player to press a key
                            if(_choosedAbility.target == Ability.AbilityTarget.SINGLE)
                            {
                                _combatLogText.text = activeFighter.name + " attack " + fighterToAttack.name + "with " + _choosedAbility.abilityName;
                            }
                            else
                            {
                                _combatLogText.text = activeFighter.name + " attack the enemies with " + _choosedAbility.abilityName;
                            }
                            
                            Coroutine corCol = StartCoroutine(_combatLogText.gameObject.GetComponent<FadeInText>().AnimateVertexColors());

                            waitForPlayerAction = true;
                            while (waitForPlayerAction)
                            {
                                yield return null;

                                if (Input.anyKeyDown)
                                {
                                    waitForPlayerAction = false;
                                }
                            }

                            StopCoroutine(corCol);
                            _combatLogText.text = "";

                            // Remove focus on enemy team
                            _fighters.FindAll(e => e.player != activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(false); return e; }).ToList();

                            // Check if the other team is dead
                            if (_fighters.Count(e => e.player != activeFighter.player && e.dead == false) == 0)
                            {
                                combatEnd = true;
                            }

                            EventSystem.current.SetSelectedGameObject(firstFocusedMove);
                            firstFocusedMove.GetComponent<ButtonColorFocus>().Focus();

                            playerUI.SetActive(true);

                            _targetChoosed = false;
                            _actionChoosed = false;
                            turnEnd = true;
                        }
                        else
                        {
                            // Remove focus on enemy team
                            _fighters.FindAll(e => e.player != activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(false); return e; }).ToList();

                            EventSystem.current.SetSelectedGameObject(firstFocusedMove);
                            firstFocusedMove.GetComponent<ButtonColorFocus>().Focus();

                            playerUI.SetActive(true);
                        }
                    }
                }
            }
        }

        print("Combat end");

        Camera.main.GetComponent<TransitionPostProcess>().TransitionEffect();

        yield return new WaitForSeconds(2f);

        Application.Quit();
    }

    IEnumerator ScreenShake()
    {
        Vector2 basePosition = battlefieldUI.transform.position;
        Vector2 nextPosition = new Vector2();

        float duration = 1f;
        float counter = 0f;

        float seed = 10f;

        while (counter < duration)
        {
            battlefieldUI.transform.position = basePosition;
            battlefieldUI.transform.rotation = Quaternion.identity;

            nextPosition.x = Mathf.Clamp01(Mathf.PerlinNoise(Time.time * seed, 0f)) - 0.5f; 
            nextPosition.y = Mathf.Clamp01(Mathf.PerlinNoise(0f, Time.time * seed)) - 0.5f;

            battlefieldUI.transform.position = basePosition + (nextPosition * power);

            counter += Time.deltaTime;
            yield return null;
        }

        battlefieldUI.transform.position = basePosition;
        battlefieldUI.transform.rotation = Quaternion.identity;
    }

    public void NotifyPlayerActionChoosed(Ability ability)
    {
        waitForPlayerAction = false;
        _choosedAbility = ability;
        _actionChoosed = true;
    }

    public void NotifyPlayerEnemyChoice(Fighter fighterToAttack)
    {
        this.fighterToAttack = fighterToAttack;
        waitForPlayerChoice = false;
        _targetChoosed = true;
    }
}
