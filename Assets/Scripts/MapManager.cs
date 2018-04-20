﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public static MapManager instance
    {
        get
        {
            if (!_mapManager)
            {
                _mapManager = FindObjectOfType(typeof(MapManager)) as MapManager;

                if (!_mapManager)
                {
                    Debug.LogError("There needs to be one active OptionsManager script on a GameObject in your scene.");
                }
            }

            return _mapManager;
        }
    }

    public MapLocation startLocation;

    private static MapManager _mapManager;
    private MapLocation _lastLocation = null;

    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        if(_lastLocation == null)
        {
            _lastLocation = startLocation;
        }
        else
        {
            SetLocation();
        }
    }

    public void SetLocation()
    {
        _lastLocation.ToggleFocus();
    }

    public void MoveToLocation(MapLocation newLocation)
    {
        // Remove focus on last location
        _lastLocation.ToggleFocus();

        _lastLocation = newLocation;
        
        // Focus on newlocation
        _lastLocation.ToggleFocus();
        

        _lastLocation.LaunchEvent();

        // TODO : Save progression
    }
}
