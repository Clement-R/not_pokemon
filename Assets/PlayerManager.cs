using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance;

    // DEBUG
    public Part[] debugParts;

    private List<Part> _inventory = new List<Part>();

    void Start ()
    {
        if (PlayerManager.instance == null)
        {
            PlayerManager.instance = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
        }

        for (int i = 0; i < 16; i++)
        {
            _inventory.Add(Instantiate(debugParts[0]));
            _inventory.Add(Instantiate(debugParts[1]));
        }
    }
	
	public List<Part> GetParts(PartType type)
    {
        Debug.Log(type);

        foreach (var part in _inventory)
        {
            Debug.Log(part.partType);
        }

        return _inventory.FindAll(e => e.partType == type);
    }
}
