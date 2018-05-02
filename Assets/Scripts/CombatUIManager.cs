using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using pkm.EventManager;

public class CombatUIManager : MonoBehaviour {

    public GameObject[] abilities;

    private CanvasGroup _canvas;

    private void Start()
    {
        _canvas = GetComponent<CanvasGroup>();
        EventManager.StartListening(EventList.HIDE_COMBAT_UI.ToString(), OnHideUI);
        EventManager.StartListening(EventList.SHOW_COMBAT_UI.ToString(), OnShowUI);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventList.HIDE_COMBAT_UI.ToString(), OnHideUI);
        EventManager.StopListening(EventList.SHOW_COMBAT_UI.ToString(), OnShowUI);
    }

    private void OnHideUI(dynamic obj)
    {
        _canvas.alpha = 0f;
        _canvas.interactable = false;
    }

    private void OnShowUI(dynamic obj)
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].GetComponent<Text>().text = CombatManager.instance.GetActiveFighter().skillset._abilities[i].abilityName;
        }

        _canvas.alpha = 1f;
        _canvas.interactable = true;
    }
}
