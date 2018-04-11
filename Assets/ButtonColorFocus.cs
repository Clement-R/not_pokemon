using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonColorFocus : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public Color normalColor;
    public Color focusColor;

    private Text _buttonText;

    public void OnDeselect(BaseEventData eventData)
    {
        _buttonText.color = normalColor;
    }

    public void OnSelect(BaseEventData eventData)
    {
        _buttonText.color = focusColor;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        _buttonText.color = normalColor;
    }

    public void Focus()
    {
        _buttonText.color = focusColor;
    }

    void Start ()
    {
        _buttonText = GetComponent<Text>();
    }
}
