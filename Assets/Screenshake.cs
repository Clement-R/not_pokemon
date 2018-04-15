using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour {

    public GameObject battlefieldUI;

    [Range(0, 1)]
    public float power = 1f;

    public void ScreenShake()
    {
        StartCoroutine(DoScreenShake());
    }

    IEnumerator DoScreenShake()
    {
        Vector2 basePosition = battlefieldUI.transform.position;
        Vector2 nextPosition = new Vector2();

        float duration = 1f;
        float counter = 0f;

        while (counter < duration)
        {
            battlefieldUI.transform.position = basePosition;
            battlefieldUI.transform.rotation = Quaternion.identity;

            nextPosition.x = Mathf.Clamp01(Mathf.PerlinNoise(Time.time, 0f)) - 0.5f;
            nextPosition.y = Mathf.Clamp01(Mathf.PerlinNoise(0f, Time.time)) - 0.5f;

            battlefieldUI.transform.position = basePosition + (nextPosition * power);

            counter += Time.deltaTime;
            yield return null;
        }

        battlefieldUI.transform.position = basePosition;
        battlefieldUI.transform.rotation = Quaternion.identity;
    }
}
