using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatMapEvent : MapEvent
{
    public override void LaunchEvent()
    {
        SceneManager.LoadScene("CombatScene");
    }
}
