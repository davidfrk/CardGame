using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    static public GameManager instance;

    [Header("CardEffects")]
    public float CardMovementSpeed = 2;
    public float CardRotationSpeed = 2;
    public float DrawCardDuration = 3;
    public float FlipCardDuration = 1.8f;

    private void Awake()
    {
        instance = this;
    }
}
