using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    static public GameManager instance;

    [Header("StatsSymbols")]
    public Sprite[] StatsSymbols = new Sprite[8];

    [Header("CardEffects")]
    public float CardMovementSpeed = 2;
    public float CardRotationSpeed = 2;
    public float DrawCardDuration = 3;
    public float FlipCardDuration = 1.8f;

    [Header("SoundEffects")]
    public AudioSource TurnStartSound;
    public AudioSource VictorySound;
    public AudioSource DiscardSound;

    GameManager()
    {
        instance = this;
    }
}
