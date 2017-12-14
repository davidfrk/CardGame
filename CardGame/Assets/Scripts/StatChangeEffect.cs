using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatChangeEffect : MonoBehaviour {

    private void OnEnable()
    {
        Invoke("Hide", GameManager.instance.StatsChangeEffectDuration);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
