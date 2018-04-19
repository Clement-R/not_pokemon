using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TransitionPostProcess : MonoBehaviour {

    public Material TransitionMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, TransitionMaterial);
    }

    public void Start()
    {
        TransitionMaterial.SetFloat("_Cutoff", 0);
    }

    public void TransitionEffect()
    {
        StartCoroutine(DoTransitionEffect());
    }

    public IEnumerator TransitionCoroutine()
    {
        yield return StartCoroutine(DoTransitionEffect());
    }

    private IEnumerator DoTransitionEffect()
    {
        float cutoff = 0f;
        while(cutoff < 1f)
        {
            cutoff += 0.05f;
            TransitionMaterial.SetFloat("_Cutoff", cutoff);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
