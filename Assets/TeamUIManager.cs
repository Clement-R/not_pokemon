using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using pkm.EventManager;

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

    public GameObject firstSelectedMember;
    public GameObject firstSelectedPart;
    public GameObject firstSelectedAvailablePart;

    // Fighter panel
    public GameObject leftShoulder;
    public GameObject rightShoulder;
    public GameObject frame;
    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject pilot;

    public TMPro.TMP_Text dex;
    public TMPro.TMP_Text str;
    public TMPro.TMP_Text acc;

    public Sprite emptyPartSprite;
    public Sprite emptyFrameSprite;
    public Sprite emptyPilotSprite;

    private static TeamUIManager _teamUIManager;
    private bool _playerChoiceMade = false;
    private Coroutine _waitingRoutine = null;
    private PartType _lastSelectedSlot;
    private bool _playerCancel = false;

    private void OnEnable()
    {
        // TODO : Create new event to call when the player change the focused team member
        EventManager.StartListening(EventList.TEAM_MEMBER_UI_SELECT.ToString(), OnMemberChange);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventList.TEAM_MEMBER_UI_SELECT.ToString(), OnMemberChange);
    }

    private void OnMemberChange(dynamic obj)
    {
        Fighter fighter = obj.fighter;
        if(fighter != null)
        {
            UpdateMemberPanel(fighter);
        }
    }

    public void Show()
    {
        // Set focus on first team member
        EventSystem.current.SetSelectedGameObject(firstSelectedMember);

        // Refresh fighter informations
        // TODO : Use currently selected fighter and not the active one
        UpdateMemberPanel(firstSelectedMember.GetComponent<TeamMemberUI>().fighter);
    }

    public void Hide()
    {
        skillsetEditor.alpha = 1;
        skillsetEditor.interactable = true;

        teamUI.alpha = 0;
        teamUI.interactable = false;
    }

    public void SwitchToMember()
    {
        memberCustomization.interactable = false;
        membersList.interactable = true;

        EventSystem.current.SetSelectedGameObject(firstSelectedMember);
    }

    public void SwitchToMemberCustomization()
    {
        memberCustomization.interactable = true;
        membersList.interactable = false;

        EventSystem.current.SetSelectedGameObject(firstSelectedPart);
        
        StartCoroutine(WaitForPlayerCancel());
    }

    private IEnumerator WaitForPlayerCancel()
    {
        while (!_playerCancel)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _playerCancel = true;
                SwitchToMember();
                break;
            }

            yield return null;
        }
        _playerCancel = false;
    }

    public void ShowPartsList(GameObject selectedPart)
    {
        _lastSelectedSlot = selectedPart.GetComponent<PartUI>().slot;

        // Trigger evnet to update parts list with valid items
        EventManager.TriggerEvent(EventList.SHOW_PARTS_LIST.ToString(), new { slot = _lastSelectedSlot });

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

    private void UpdateMemberPanel(Fighter fighter)
    {
        // Display sprites of the parts, else the empty sprite
        if(fighter.leftShoulder != null)
        {
            leftShoulder.GetComponent<Image>().sprite = fighter.leftShoulder.sprite;
        }
        else
        {
            leftShoulder.GetComponent<Image>().sprite = emptyPartSprite;
        }
            

        if (fighter.rightShoulder != null)
        {
            rightShoulder.GetComponent<Image>().sprite = fighter.rightShoulder.sprite;
        }   
        else
        {
            rightShoulder.GetComponent<Image>().sprite = emptyPartSprite;
        }

        // TODO
        //if (fighter.frame != null)
        //    frame.GetComponent<Image>().sprite = fighter.frame.sprite;

        if (fighter.leftArm != null)
        {
            leftArm.GetComponent<Image>().sprite = fighter.leftArm.sprite;
        }
        else
        {
            leftArm.GetComponent<Image>().sprite = emptyPartSprite;
        }
        

        if (fighter.rightArm != null)
        {
            rightArm.GetComponent<Image>().sprite = fighter.rightArm.sprite;
        }
        else
        {
            rightArm.GetComponent<Image>().sprite = emptyPartSprite;
        }

        // TODO
        //if (fighter.pilot != null)
        //    pilot.GetComponent<Image>().sprite = fighter.pilot.sprite;

        // Update stats
        dex.text = fighter.GetDexterity().ToString();
        str.text = fighter.GetStrength().ToString();
        acc.text = fighter.GetAccuracy().ToString();
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
