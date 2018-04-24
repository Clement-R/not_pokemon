using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour {

    public static MapManager instance;

    public MapLocation startLocation;

    private MapLocation _lastLocation = null;
    private int _lastLocationId = -1;

    void Start ()
    {
        if(MapManager.instance == null)
        {
            MapManager.instance = FindObjectOfType(typeof(MapManager)) as MapManager;
        }

        int _lastLocationId = PlayerPrefs.GetInt("last_location");
        if(_lastLocationId == -1)
        {
            _lastLocationId = 0;
        }

        print(_lastLocationId);

        if(_lastLocation == null)
        {
            foreach (MapLocation location in GameObject.FindObjectsOfType<MapLocation>())
            {
                if(location.id == _lastLocationId)
                {
                    _lastLocation = location;
                    break;
                }
            } 
        }

        StartCoroutine(SetupMap());
    }

    IEnumerator SetupMap()
    {
        yield return new WaitForSeconds(0.1f);
        SetLocation();
    }

    public void SetLocation()
    {
        EventSystem.current.SetSelectedGameObject(_lastLocation.gameObject);
        _lastLocation.ToggleFocus();
    }

    public void MoveToLocation(MapLocation newLocation)
    {
        // Remove focus on last location
        _lastLocation.ToggleFocus();

        _lastLocation = newLocation;
        
        // Focus on newlocation
        _lastLocation.ToggleFocus();

        // TODO : Save progression
        // Variables to save :
        // Seed (procgen map)
        // Id of the last position (the algorithm should always give the id the same way to ensure that the same ID on the same seed always refer to the same place)
        PlayerPrefs.SetInt("last_location", newLocation.id);
        PlayerPrefs.Save();

        _lastLocation.LaunchEvent();
    }
}
