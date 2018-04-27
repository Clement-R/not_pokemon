using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private PartSlot _lastSelectedSlot;

    public void Show()
    {
        // Set focus on first team member
        EventSystem.current.SetSelectedGameObject(firstSelectedMember);
        // Refresh fighter informations
        UpdateMemberPanel(CombatManager.instance.GetActiveFighter());
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

    public void ShowPartsList(PartSlot partSlot)
    {
        _lastSelectedSlot = partSlot;

        // TODO : Update parts list with valid items
        // availablePartsList 

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
