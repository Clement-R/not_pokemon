using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using pkm.EventManager;

public class PartsListUI : MonoBehaviour {

    public Button upArrow;
    public Button downArrow;

    public GameObject[] parts;

    private int _actualIndex;
    private int _numberOfPartsOnScreen = 4;
    private int _numberOfPartsFound = 0;

	public void ShowPrevious()
    {
        _actualIndex -= _numberOfPartsOnScreen;
        if(_actualIndex < 0)
        {
            _actualIndex = 0;
        }

        // TODO : Update shown parts
    }

    public void ShowNext()
    {
        _actualIndex += _numberOfPartsOnScreen;
        if(_actualIndex > _numberOfPartsFound)
        {
            _actualIndex = _numberOfPartsFound;
        }

        // TODO : Update shown parts
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventList.SHOW_PARTS_LIST.ToString(), UpdateList);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventList.SHOW_PARTS_LIST.ToString(), UpdateList);
    }

    private void UpdateList(dynamic obj)
    {
        Debug.Log(obj.slot);
        GetParts(obj.slot);
        // TODO : Display the parts
    }

    private void GetParts(PartType partType)
    {
        // Get all parts in inventory that match the given type
        List<Part> parts = PlayerManager.instance.GetParts(partType);
        _numberOfPartsFound = parts.Count;
    }
}
