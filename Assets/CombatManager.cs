using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public bool combatEnd = false;

    private List<Fighter> _fighters = new List<Fighter>();
    private Fighter fighterToAttack = null;

    private TMPro.TMP_Text _combatLogText;

    void Start ()
    {
        _fighters.Add(P1_Fighter1);
        _fighters.Add(P1_Fighter2);

        _fighters.Add(P2_Fighter1);
        _fighters.Add(P2_Fighter2);

        _combatLogText = combatLog.GetComponent<TMPro.TMP_Text>();

        StartCoroutine(CombatLogic());
    }

    IEnumerator CombatLogic()
    {
        Fighter activeFighter = null;

        while (!combatEnd)
        {
            // TODO : Change for a fighter stats like agility
            activeFighter = _fighters.DefaultIfEmpty(null).FirstOrDefault(e => e.canPlay == true && e.dead == false);

            // Reset all fighters and select first one
            if (activeFighter == null)
            {
                _fighters.Select(c => { c.canPlay = true; return c; }).ToList();
                activeFighter = _fighters.First(e => e.canPlay == true);
            }

            print(activeFighter.gameObject.name);

            waitForPlayerAction = true;
            while (waitForPlayerAction)
            {
                yield return null;
            }

            activeFighter.canPlay = false;
            
            // Choose which enemy to attack
            // Set enemy team to be focusable
            _fighters.FindAll(e => e.player != activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(true); return e; }).ToList();

            // Wait for player choice
            waitForPlayerChoice = true;
            while (waitForPlayerChoice)
            {
                yield return null;
            }

            // TODO : Remove action logic from here
            // Show log
            _combatLogText.text = "";
            playerUI.SetActive(false);
            
            // Play attack
            fighterToAttack.TakeDamage(50);

            yield return RevealText(activeFighter.name + " attack " + fighterToAttack.name);

            // Remove focus on enemy team
            _fighters.FindAll(e => e.player != activeFighter.player && e.dead == false).Select(e => { e.ChangeFocus(false); return e; }).ToList();

            // Check if the other team is dead
            if (_fighters.Count(e => e.player != activeFighter.player && e.dead == false) == 0)
            {
                combatEnd = true;
            }

            EventSystem.current.SetSelectedGameObject(firstFocusedMove);
            
            playerUI.SetActive(true);
        }

        print("Combat end");

        yield return new WaitForSeconds(2f);

        Application.Quit();
    }

    IEnumerator RevealText(string text)
    {
        _combatLogText.text = text;
        _combatLogText.ForceMeshUpdate();

        TMPro.TMP_TextInfo textInfo = _combatLogText.textInfo;

        int totalVisibleCharacters = textInfo.characterCount;
        int visibleCount = 0;

        while (visibleCount < totalVisibleCharacters)
        {
            _combatLogText.maxVisibleCharacters = visibleCount;
            visibleCount += 1;

            yield return null;
        }

        waitForPlayerAction = true;
        while(waitForPlayerAction)
        {
            if(Input.anyKeyDown)
            {
                waitForPlayerAction = false;
            }

            yield return null;
        }

        _combatLogText.text = "";
    }

    public void NotifyPlayerInput()
    {
        waitForPlayerAction = false;
    }

    public void NotifyPlayerEnemyChoice(Fighter fighterToAttack)
    {
        this.fighterToAttack = fighterToAttack;
        waitForPlayerChoice = false;
    }
}
