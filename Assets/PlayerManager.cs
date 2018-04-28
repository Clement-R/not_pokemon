using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager instance;

    private List<Part> _inventory = new List<Part>();

    void Start ()
    {
        if (PlayerManager.instance == null)
        {
            PlayerManager.instance = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
        }
    }
	
	public List<Part> GetParts(PartType type)
    {
        return _inventory.FindAll(e => e.partType == type);
    }
}
