using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T _instance;

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    /// <summary>
    /// Sets the singleton instance if it's null
    /// If not, the object is destroyed, and the function returns false
    /// </summary>
    /// <param name="inst"></param>
    /// <returns>True if the passed parameter is the singleton and set as such</returns>
    protected bool SetInstanceOK(T inst)
    {
        if (_instance == null)
        {
            _instance = inst;
            return true;
        }
        else
        {
            Destroy(gameObject);
            return false;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            //Debug.Log("Removed singleton");
        }
    }
}
