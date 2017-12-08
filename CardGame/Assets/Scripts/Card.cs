using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public Stats Cost;
    public Stats UserEffects;
    public bool hasTarget = true;
    public Stats TargetEffects;

    public bool isPlayable(Player Owner)
    {
        return Owner.stats.isGreater(Cost);
    }

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
