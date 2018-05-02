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

        for (int i = 0; i < 15; i++)
        {
            Part newPart = Instantiate(debugParts[0]);
            newPart.partName = "Part " + newPart.partName + " | " + i;
            _inventory.Add(newPart);

            newPart = Instantiate(debugParts[1]);
            newPart.partName = "Part " + newPart.partName + " | " + i;
            _inventory.Add(newPart);
        }
    }
	
	public List<Part> GetParts(PartType type)
    {
        return _inventory.FindAll(e => e.partType == type);
    }
}
