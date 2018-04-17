using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour {

    [System.Serializable]
    public class Layer
    {
        public int direction;
        public GameObject layer;
    }

    public List<Layer> layers = new List<Layer>();

    void Start ()
    {
        StartCoroutine(Effect());
	}
    
    IEnumerator Effect()
    {
        while(true)
        {
            float counter = 3f;
            foreach (var layer in layers)
            {
                Vector2 currentPos = layer.layer.transform.localPosition;

                if(currentPos.x <= -3f)
                {
                    layer.direction = 1;
                }
                else if(currentPos.x >= 12.5f)
                {
                    layer.direction = -1;
                }

                if (layer.direction == 1)
                {
                    layer.layer.transform.localPosition = new Vector2(currentPos.x + 0.025f / counter, currentPos.y);
                }
                else
                {
                    layer.layer.transform.localPosition = new Vector2(currentPos.x - 0.025f / counter, currentPos.y);
                }

                counter--;
            }

            yield return null;
        }
    }
}
