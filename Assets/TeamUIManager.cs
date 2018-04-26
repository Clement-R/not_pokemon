using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TeamUIManager : MonoBehaviour {

    public static TeamUIManager instance
    {
        get
        {
            if (!_teamUIManager)
            {
                _teamUIManager = FindObjectOfType(typeof(TeamUIManager)) as TeamUIManager;

                if (!_teamUIManager)
                {
                    Debug.LogError("There needs to be one active TeamUIManager script on a GameObject in your scene.");
                }
            }

            return _teamUIManager;
        }
    }

    public CanvasGroup skillsetEditor;
    public CanvasGroup teamUI;
    public CanvasGroup membersList;
    public CanvasGroup memberCustomization;
    public CanvasGroup availablePartsList;

    public GameObject firstSelectedPart;

    public GameObject firstSelectedAvailablePart;

    private static TeamUIManager _teamUIManager;
    private bool _playerChoiceMade = false;
    private Coroutine _waitingRoutine = null;

    public void Show()
    {
        // TODO : Set focus on first member
    }

    public void Hide()
    {
        skillsetEditor.alpha = 1;
        skillsetEditor.interactable = true;

        teamUI.alpha = 0;
        teamUI.interactable = false;
    }

    public void SwitchToMemberCustomization()
    {
        memberCustomization.interactable = true;
        membersList.interactable = false;

        EventSystem.current.SetSelectedGameObject(firstSelectedPart);
    }

    public void ShowPartsList()
    {
        availablePartsList.alpha = 1;
        availablePartsList.interactable = true;

        memberCustomization.interactable = false;

        EventSystem.current.SetSelectedGameObject(firstSelectedAvailablePart);

        _waitingRoutine = StartCoroutine(WaitForPlayerChoice());
    }

    public void HidePartsList()
    {
        availablePartsList.alpha = 0;
        availablePartsList.interactable = false;

        memberCustomization.interactable = true;

        EventSystem.current.SetSelectedGameObject(firstSelectedPart);
    }

    private IEnumerator WaitForPlayerChoice()
    {
        while (!_playerChoiceMade)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // TODO

                if(availablePartsList.interactable)
                {
                    HidePartsList();
                }
                
                break;
            }

            yield return null;
        }
    }
}
