﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

    static public GameManager instance;

    [Header("StatsSymbols")]
    public Sprite[] StatsSymbols = new Sprite[8];

    [Header("CardEffects")]
    public float CardMovementSpeed = 2;
    public float CardRotationSpeed = 2;
    public float DrawCardDuration = 3;
    public float FlipCardDuration = 1.8f;

    [Header("VisualEffects")]
    public float StatsChangeEffectDuration = 2;
    internal bool isVisualEffectsActive = false;

    [Header("SoundEffects")]
    public AudioSource TurnStartSound;
    public AudioSource VictorySound;
    public AudioSource DiscardSound;

    [Header("Events")]
    public VictoryEvent VictoryEvent;
    internal VictoryType VictoryType;
    internal Player VictoriousPlayer;
    public UnityEvent GameStart;
    public UnityEvent ClearEvent;

    internal bool isPlaying = false;

    [Header("Players")]
    public List<Player> players;

    GameManager()
    {
        instance = this;
    }

    private void Start()
    {
        Invoke("StartGame", 1f);
    }

    public void StartGame()
    {
        players[0].isMyTurn = true;
        isPlaying = true;
        GameStart.Invoke();
    }

    public void PlayerDied(Player player)
    {
        VictoryType = VictoryType.Demolition;
        VictoriousPlayer = player.Enemy;
        isPlaying = false;
        VictoryEvent.Invoke(VictoriousPlayer, VictoryType);
        VictorySound.Play();
    }

    public void PlayerWon(Player player)
    {
        VictoryType = VictoryType.Supremacy;
        VictoriousPlayer = player;
        isPlaying = false;
        VictoryEvent.Invoke(VictoriousPlayer, VictoryType);
        VictorySound.Play();
    }

    private void ClearGame()
    {
        ClearEvent.Invoke();
    }

    public void Exit()
    {
        ClearGame();
        Application.Quit();
    }

    public void Rematch()
    {
        ClearGame();
        Invoke("StartGame", 1f);
    }
}

public enum VictoryType
{
    Supremacy,
    Demolition
}

[System.Serializable]
public class VictoryEvent : UnityEvent<Player, VictoryType>
{
    
}