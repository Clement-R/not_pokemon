using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;
using TMPro;

public class CombatLogManager : MonoBehaviour {

    private TMP_Text _combatLog;

    private FadeInText _fadeManager;

	void Start ()
    {
        _combatLog = GetComponent<TMP_Text>();
        _fadeManager = GetComponent<FadeInText>();
        EventManager.StartListening("displayText", OnDisplayText);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("displayText", OnDisplayText);
    }
    
    private void OnDisplayText(dynamic obj)
    {
        EventManager.TriggerEvent("HideCombatUI", new { });
        _combatLog.text = obj.text;
        StartCoroutine(LaunchAnimationAndWaitInput());
    }

    private IEnumerator LaunchAnimationAndWaitInput()
    {
        yield return StartCoroutine(_fadeManager.AnimateVertexColors());
        yield return StartCoroutine(WaitForPlayerInput());
    }

    private IEnumerator WaitForPlayerInput()
    {
        bool waitForPlayerAction = true;
        while (waitForPlayerAction)
        {
            yield return null;

            if (Input.anyKeyDown)
            {
                waitForPlayerAction = false;
            }
        }

        _combatLog.text = "";

        EventManager.TriggerEvent("ShowCombatUI", new { });
    }
}
