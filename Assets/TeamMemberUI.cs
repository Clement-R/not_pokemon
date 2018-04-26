using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamMemberUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Button _button;

    void Start ()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
        // TODO : Set color to unselected except for first one
    }

    private void OnClick()
    {
        TeamUIManager.instance.SwitchToMemberCustomization();
    }
	

    public void OnDeselect(BaseEventData eventData)
    {
        // TODO : Manage color selection here, because Unity    
    }

    public void OnSelect(BaseEventData eventData)
    {
        // TODO : Update team member info panel
    }
}
