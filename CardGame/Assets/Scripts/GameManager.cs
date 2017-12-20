using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    static public GameManager instance;

    [Header("GameMode")]
    public GameModeType GameMode;

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

    [Header("Control")]
    public float TimeToDiscard = 0.5f;

    internal bool isPlaying = false;

    [Header("PlayersUI")]
    public List<PlayerUI> playersUI;
    public List<Player> players;

    GameManager()
    {
        instance = this;
    }

    private void Start()
    {
        
    }

    private void StartGame()
    {
        InitPlayers();
        players[0].isMyTurn = true;
        isPlaying = true;
        GameStart.Invoke();
    }

    private void InitPlayers()
    {
        players[0].name = "Player1";
        players[1].name = "Player2";
        players[0].Enemy = players[1];
        players[1].Enemy = players[0];

        playersUI[0].Subscribe(players[0]);
        playersUI[1].Subscribe(players[1]);
    }

    public void AddPlayer(Player player)
    {
        if (players.Count >= 2)
        {
            Debug.LogError("Trying to add more than two players in game");
        }
        else
        {
            players.Add(player);
            if (players.Count == 2)
            {
                if (isServer)
                {
                    Invoke("ServerStartGame", 1f);
                }
            }
        }
    }

    [ServerCallback]
    private void ServerStartGame()
    {
        StartGame();
        RpcStartGame();
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        if (isServer) return;

        StartGame();
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
        SceneManager.LoadScene("MainMenuScene");
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

public enum GameModeType
{
    PVE,
    PVP_Local,
    PVP_Online
}