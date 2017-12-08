using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public int Cost = 2;
    public bool hasTarget = true;

    public Stats UserEffects;
    public Stats TargetEffects;

    public void Activate(Player Owner, Player Target)
    {
        if (hasTarget && Target == null)
        {
            Debug.LogError("Null Target when activating effect");
            return;
        }

        Owner.ApplyStats(UserEffects);
        if (Target != null)
        {
            Target.ApplyStats(TargetEffects);
        }
    }
}
