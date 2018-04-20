using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;
using TMPro;

public class CombatLogManager : MonoBehaviour {

    public static CombatLogManager instance
    {
        get
        {
            if (!logManager)
            {
                logManager = FindObjectOfType(typeof(CombatLogManager)) as CombatLogManager;

                if (!logManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
            }

            return logManager;
        }
    }

    public bool doneDisplaying = false;

    private static CombatLogManager logManager;
    private TMP_Text _combatLog;
    private FadeInText _fadeManager;

	void Start ()
    {
        _combatLog = GetComponent<TMP_Text>();
        _fadeManager = GetComponent<FadeInText>();
        EventManager.StartListening("DisplayText", OnDisplayText);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("DisplayText", OnDisplayText);
    }
    
    private void OnDisplayText(dynamic obj)
    {
        EventManager.TriggerEvent(EventList.HIDE_COMBAT_UI.ToString(), new { });
        _combatLog.text = obj.text;
        StartCoroutine(LaunchAnimationAndWaitInput());
    }

    private IEnumerator LaunchAnimationAndWaitInput()
    {
        doneDisplaying = false;
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

        doneDisplaying = true;

        _combatLog.text = "";

        EventManager.TriggerEvent(EventList.SHOW_COMBAT_UI.ToString(), new { });
    }
}
