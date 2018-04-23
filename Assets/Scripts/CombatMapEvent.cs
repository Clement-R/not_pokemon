using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatMapEvent : MapEvent
{
    public override void LaunchEvent()
    {
        Camera.main.GetComponent<TransitionPostProcess>().StartCoroutine(LoadCombatScene());
    }

    private IEnumerator LoadCombatScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("CombatScene");
        op.allowSceneActivation = false;
        yield return Camera.main.GetComponent<TransitionPostProcess>().TransitionCoroutine();
        op.allowSceneActivation = true;
    }
}
