using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    public int inventoryMaxSpace = 8;
    public FighterComponent[] inventory;

    void Start ()
    {
        // TODO : singleton
        DontDestroyOnLoad(gameObject);
	}
	
	void Update ()
    {
		
	}
}
