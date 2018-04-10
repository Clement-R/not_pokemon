using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour {

    public Fighter P1_Fighter1;
    public Fighter P1_Fighter2;

    public Fighter P2_Fighter1;
    public Fighter P2_Fighter2;

    public bool waitForPlayerAction = true;

    public bool combatEnd = false;

    private List<Fighter> _fighters = new List<Fighter>();

    void Start ()
    {
        _fighters.Add(P1_Fighter1);
        _fighters.Add(P1_Fighter2);

        _fighters.Add(P2_Fighter1);
        _fighters.Add(P2_Fighter2);

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
            // TODO : Remove action logic from here
            Fighter firstEnemyFighter = _fighters.DefaultIfEmpty(null).FirstOrDefault(e => e.player != activeFighter.player && e.dead == false);

            print(activeFighter.name + " attack " + firstEnemyFighter.name);
            firstEnemyFighter.TakeDamage(50);

            // Check if the other team is dead
            if (_fighters.Count(e => e.player != activeFighter.player && e.dead == false) == 0)
            {
                combatEnd = true;
            }
        }

        print("Combat end");

        yield return new WaitForSeconds(2f);

        Application.Quit();
    }

    public void NotifyPlayerInput()
    {
        waitForPlayerAction = false;        
    }
}
