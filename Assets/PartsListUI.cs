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

	void Start ()
    {
		
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
    }
}
