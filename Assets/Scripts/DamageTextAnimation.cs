using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextAnimation : MonoBehaviour {

    private TMP_Text _text;

	void Start ()
    {
        _text = GetComponent<TMP_Text>();

        print(_text);

        StartCoroutine(Effect());
	}
	
	IEnumerator Effect()
    {
        Color c = _text.color;
        Vector2 pos = transform.position;

        while(c.a > 0f)
        {
            yield return null;
            c.a -= 0.05f;
            _text.color = c;
            transform.position = new Vector2(pos.x, transform.position.y + 0.05f);
        }
    }
}
