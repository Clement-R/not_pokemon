using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextAnimation : MonoBehaviour {

    private TMP_Text _text;

	void Awake ()
    {
        _text = GetComponent<TMP_Text>();
        print(_text);
        StartCoroutine(Effect());
	}

    public void ChangeText(string text)
    {
        _text.text = text;
    }
	
	IEnumerator Effect()
    {
        Color c = _text.color;
        Vector3 pos = transform.localPosition;

        while(c.a > 0f)
        {
            yield return null;
            c.a -= 0.05f;
            _text.color = c;
            transform.localPosition = new Vector3(pos.x, transform.localPosition.y + 5f, 0f);
        }
    }
}
