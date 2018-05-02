using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartsListSlotUI : MonoBehaviour {

    private Part _part;
    private Image _image;
    private Button _button;
    private TMP_Text _text;
    private bool _isActive = true;

    private void Start()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<TMP_Text>();
    }

    public bool IsActive()
    {
        return _isActive;
    }

    public void UpdatePart (Part newPart)
    {
        _part = newPart;
        RefreshUI();
    }

    public void Enable()
    {
        _button.enabled = true;
        _image.enabled = true;
        _isActive = true;
    }

    public void Disable()
    {
        _text.text = "";
        _button.enabled = false;
        _image.enabled = false;
        _isActive = false;
    }

    void RefreshUI()
    {
        _text.text = _part.partName;
        _image.sprite = _part.sprite;
	}
}
