using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using pkm.EventManager;

public class PartsListUI : MonoBehaviour {

    public Button upArrow;
    public Button downArrow;

    public GameObject[] parts;

    private int _actualIndex;
    private int _numberOfPartsOnScreen = 4;
    private int _numberOfPartsFound = 0;
    private List<Part> _foundParts = new List<Part>();

    public void ShowPrevious()
    {
        _actualIndex -= _numberOfPartsOnScreen;

        if(_actualIndex < 0)
        {
            _actualIndex = 0;
        }

        // Update shown parts
        RefreshUI();
    }

    public void ShowNext()
    {
        // TODO : Change index to don't show null parts at the end
        _actualIndex += _numberOfPartsOnScreen;

        if(_actualIndex > _numberOfPartsFound)
        {
            _actualIndex = _numberOfPartsFound;
        }

        // Update shown parts
        RefreshUI();
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventList.SHOW_PARTS_LIST.ToString(), UpdateList);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventList.SHOW_PARTS_LIST.ToString(), UpdateList);
    }

    private void RefreshUI()
    {
        Debug.Log(_actualIndex);

        // Disable up arrow if we're at top
        if (_actualIndex < _numberOfPartsOnScreen)
        {
            upArrow.GetComponent<Button>().interactable = false;
        }
        else
        {
            upArrow.GetComponent<Button>().interactable = true;
        }

        // Disable down arrow if we're at the end
        if (_actualIndex >= _numberOfPartsFound - _numberOfPartsOnScreen)
        {
            downArrow.GetComponent<Button>().interactable = false;
        }
        else
        {
            downArrow.GetComponent<Button>().interactable = true;
        }

        for (int i = 0; i < _numberOfPartsOnScreen; i++)
        {
            // Update each UI part if one part is available
            if(_actualIndex + i < _numberOfPartsFound)
            {
                parts[i].GetComponentInChildren<TMP_Text>().text = _foundParts[_actualIndex + i].name;
            }
            else
            {
                // TODO : Disable unused parts
                parts[i].GetComponentInChildren<TMP_Text>().text = "";
            }
        }

        if(EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(parts[0]);
        }
    }

    private void UpdateList(dynamic obj)
    {
        _foundParts = GetParts(obj.slot);

        _actualIndex = 0;
        RefreshUI();
    }

    private List<Part> GetParts(PartType partType)
    {
        // Get all parts in inventory that match the given type
        List<Part> parts = PlayerManager.instance.GetParts(partType);
        Debug.Log(parts.Count.ToString());
        _numberOfPartsFound = parts.Count;
        return parts;
    }
}
