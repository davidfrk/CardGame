using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    static public GameManager instance;
    static public GameModeType GameMode = GameModeType.PVP_Online;

    [Header("StatsSymbols")]
    public Sprite[] StatsSymbols = new Sprite[8];

    [Header("UISymbols")]
    public Sprite CheckBoxOpenMark;
    public Sprite CheckBoxClosedMark;

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
    public UnityEvent RematchStatus;

    [Header("Control")]
    public float TimeToDiscard = 0.5f;

    internal bool isPlaying = false;

    [Header("PlayersUI")]
    public List<Transform> playersPos;
    public List<PlayerUI> playersUI;
    public List<Player> players;

    internal GameObject playerPrefab;

    GameManager()
    {
        instance = this;
    }

    private void Start()
    {
        playerPrefab = NetworkManager.singleton.playerPrefab;
        
        switch (GameMode)
        {
            case GameModeType.PVE:
                {
                    GameObject botGO = Instantiate(playerPrefab);
                    RandomBot bot = botGO.GetComponent<RandomBot>();
                    bot.isActive = true;
                    NetworkServer.Spawn(botGO);
                    break;
                }
            case GameModeType.PVP_Local:
                {
                    GameObject playerGO = Instantiate(playerPrefab);
                    NetworkServer.Spawn(playerGO);
                    break;
                }
            case GameModeType.PVP_Online:
                {
                    break;
                }
            default:
                break;
        }
    }

    private void StartGame()
    {
        players[0].Enemy = players[1];
        players[1].Enemy = players[0];
        players[0].isMyTurn = true;
        isPlaying = true;
        GameStart.Invoke();
    }

    private void InitPlayer(int player)
    {
        players[player].transform.position = playersPos[player].position;
        players[player].transform.rotation = playersPos[player].rotation;

        playersUI[player].Subscribe(players[player]);
        players[player].name = "Player " + (player + 1);
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
            InitPlayer(players.Count - 1);
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

    public void ClearGame()
    {
        ClearEvent.Invoke();
    }

    public static void BackToMainMenu()
    {
        if (instance != null)
        {
            instance.ClearGame();
        }
        NetworkManager.Shutdown();
        SceneManager.LoadScene("MainMenuScene");
    }

    [Server]
    public void EvaluateRematch()
    {
        if (GameMode == GameModeType.PVP_Local)
        {
            ServerRematch();
        }else if (players.Count == 2 && players[0].wantRematch && players[1].wantRematch)
        {
            ServerRematch();
        }
    }

    [Server]
    public void ServerRematch()
    {
        ClearGame();
        ServerStartGame();
    }

    public Sprite GetCheckBoxSprite(bool state)
    {
        if (state)
        {
            return CheckBoxClosedMark;
        }
        else
        {
            return CheckBoxOpenMark;
        }
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