using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapLocation : MonoBehaviour {

    private Image _sprite;
    private bool _isFocus = false;

	void Start ()
    {
        _sprite = GetComponent<Image>();
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0.5f);
    }

    public void ToggleFocus()
    {
        _isFocus = !_isFocus;

        if(_isFocus)
        {
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 1f);
        }
        else
        {
            _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 0.5f);
        }
    }
}
