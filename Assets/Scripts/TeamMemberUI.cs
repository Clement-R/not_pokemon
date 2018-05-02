using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using pkm.EventManager;

public class TeamMemberUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Fighter fighter;

    private Button _button;

    void Start ()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        // TODO : Set color to unselected except for first one
    }

    private void OnClick()
    {
        TeamUIManager.instance.SwitchToMemberCustomization(fighter);
    }
	
    public void OnDeselect(BaseEventData eventData)
    {
        // TODO : Manage color selection here, because Unity
    }

    public void OnSelect(BaseEventData eventData)
    {
        // Trigger event to update the TeamUIManager with the new focused team member
        EventManager.TriggerEvent(EventList.TEAM_MEMBER_UI_SELECT.ToString(), new { fighter });
    }
}
