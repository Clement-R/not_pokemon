using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class CombatUIManager : MonoBehaviour {

    private CanvasGroup _canvas;

    private void Start()
    {
        _canvas = GetComponent<CanvasGroup>();
        EventManager.StartListening("HideCombatUI", OnHideUI);
        EventManager.StartListening("ShowCombatUI", OnShowUI);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("HideCombatUI", OnHideUI);
        EventManager.StopListening("ShowCombatUI", OnShowUI);
    }

    private void OnHideUI(dynamic obj)
    {
        _canvas.alpha = 0f;
        _canvas.interactable = false;
    }

    private void OnShowUI(dynamic obj)
    {
        _canvas.alpha = 1f;
        _canvas.interactable = true;
    }
}
