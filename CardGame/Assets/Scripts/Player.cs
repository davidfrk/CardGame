using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    static public Player LocalPlayer;

    public Stats stats;
    public Card[] Hand = new Card[8];
    internal Player Enemy;
    public LayerMask CardLayer;

    internal bool isMyTurn = false;
    private Bot bot;
    private Ray ray;
    private RaycastHit hit;

    internal int numberOfCards = 8;
    private bool isFirstTurn = true;
    private int turn = 0;
    public bool mustOpenCards = true;

    public UnityEvent TurnStartEvent;

    internal bool wantRematch = false;
    internal float turnStartTime;

    //Touch
    float touchTime = float.MaxValue;
    Card selectedCard = null;
    Vector2 touchOrigin;

    private void Awake()
    {
        bot = GetComponent<Bot>();
    }

    void Start () {
        if (isLocalPlayer)
        {
            LocalPlayer = this;
        }
        stats.Stat[0].StatEvent.AddListener(VictoryCondition);
        GameManager.instance.GameStart.AddListener(OnGameStart);
        GameManager.instance.ClearEvent.AddListener(Clear);
        GameManager.instance.AddPlayer(this);
        InitGameMode();
    }

    public void OnGameStart()
    {
        this.CancelInvoke();
        stats.Set(30, 5, 2, 5, 2, 5, 2, 5);
        isFirstTurn = true;

        wantRematch = false;
        
        if (isMyTurn)
        {
            isMyTurn = false;
            StartTurn();
        }
    }

    private void InitGameMode()
    {
        if (GameManager.GameMode == GameModeType.PVP_Online && !isLocalPlayer)
        {
            mustOpenCards = false;
        }
    }

    public void DrawCards()
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            ServerDrawCard(i);
        }
    }

    public void StartTurn()
    {
        turn++;
        stats.Update();

        if (isFirstTurn)
        {
            isFirstTurn = false;
            DrawCards();
            Invoke("OnTurnStart", GameManager.instance.DrawCardDuration);
        }
        else
        {
            ShowCards(true);
            FlipCardsToStartTurn();
        }
        GameManager.instance.TurnStartSound.Play();
    }

    public void OnTurnStart()
    {
        isMyTurn = true;
        Debug.Log("Turn " + turn + " to " + this.gameObject);
        GameManager.instance.isVisualEffectsActive = true;
        turnStartTime = Time.time;
        TurnStartEvent.Invoke();
    }

    public void OnTurnEnd()
    {
        ShowCards(false);
        isMyTurn = false;
        if (GameManager.instance.isPlaying)
        {
            Enemy.StartTurn();
        }
    }

    [Command]
    public void CmdUseCard(int pos)
    {
        if (isMyTurn)
        {
            RpcUseCard(pos);
            UseCard(pos);
        }
    }

    [ClientRpc]
    private void RpcUseCard(int pos)
    {
        if (isServer) return;
        UseCard(pos);
    }

    public void UseCard(int pos)
    {
        UseCard(pos, Enemy);
    }

    private void UseCard(int pos, Player Target)
    {
        Card card = Hand[pos];
        if (Hand[pos].isPlayable(this))
        {
            //Debug.Log("UseCard " + card.name);

            isMyTurn = false;
            card.Activate(this, Target);
            card.Discard();
            if (isServer)
                ServerDrawCard(pos);
            Invoke("FlipCardsToEndTurn", GameManager.instance.DrawCardDuration);
        }
        else
        {
            //Debug.Log("Cant use card " + card.name);
            //PlaySound
        }
    }

    [Command]
    public void CmdDiscard(int pos)
    {
        if (isMyTurn)
        {
            RpcDiscard(pos);
            Discard(pos);
        }
    }

    [ClientRpc]
    private void RpcDiscard(int pos)
    {
        if (isServer) return;
        Discard(pos);
    }

    public void Discard(int pos)
    {
        isMyTurn = false;
        Hand[pos].Discard();
        if (isServer)
            ServerDrawCard(pos);
        Invoke("FlipCardsToEndTurn", GameManager.instance.DrawCardDuration);
    }

    [ServerCallback]
    private void ServerDrawCard(int pos)
    {
        Card card = Deck.instance.DrawCard();
        InitCard(card, pos);
        NetworkServer.Spawn(card.gameObject);
        RpcDrawCard(card.gameObject, pos);
    }

    private void InitCard(Card card, int pos) {
        Hand[pos] = card;
        card.flipState = !mustOpenCards;
        card.MoveTo(HandManager.instance.CardsTransforms[pos]);
        card.PosInHand = pos;
        card.Owner = this;
    }

    [ClientRpc]
    private void RpcDrawCard(GameObject cardGO, int pos)
    {
        if (isServer) return;
        InitCard(cardGO.GetComponent<Card>(), pos);
    }

    public void ApplyStats(Stats Effect)
    {
        stats.Add(Effect);
    }

    public void ApplyStats(int Amount, StatsType statsType)
    {
        stats.ApplyStats(Amount, statsType);
    }

    public void FlipCardsToStartTurn()
    {
        if (mustOpenCards)
        {
            FlipCards(false);
        }
        Invoke("OnTurnStart", GameManager.instance.FlipCardDuration);
    }

    public void FlipCardsToEndTurn()
    {
        FlipCards(true);
        Invoke("OnTurnEnd", GameManager.instance.FlipCardDuration);
    }

    public void FlipCards(bool state)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            Hand[i].Flip(state);
        }
    }

    public void ShowCards(bool state)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            Hand[i].SetActive(state);
        }
    }

    private void VictoryCondition(int hp)
    {
        if (hp == 100)
        {
            GameManager.instance.PlayerWon(this);
        }else if (hp == 0)
        {
            GameManager.instance.PlayerDied(this);
        }
    }

    public void TryRematch()
    {
        //GameManager.instance.ClearGame();
        CmdRematch();
    }

    [Command]
    private void CmdRematch()
    {
        wantRematch = true;
        GameManager.instance.RematchStatus.Invoke();
        RpcWantToRematch();
        GameManager.instance.EvaluateRematch();
    }

    [ClientRpc]
    private void RpcWantToRematch()
    {
        if (isServer) return;
        wantRematch = true;
        GameManager.instance.RematchStatus.Invoke();
    }

    private void Update()
    {
        if (!isMyTurn || !GameManager.instance.isPlaying || (GameManager.GameMode != GameModeType.PVP_Local && !isLocalPlayer))
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.BackToMainMenu();
            return;
        }

        if (Time.time - turnStartTime > GameManager.instance.TurnDuration)
        {
            bot.Play();
            return;
        }

        //UseMouse
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50, CardLayer))
            {
                Card card = hit.collider.GetComponentInParent<Card>();
                if (!card.isInGraveyard)
                    CmdUseCard(card.PosInHand);
            }
        }else if (Input.GetMouseButtonDown(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50, CardLayer))
            {
                Card card = hit.collider.GetComponentInParent<Card>();
                if (!card.isInGraveyard)
                    CmdDiscard(card.PosInHand);
            }
        }

        //UseTouch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            
            if (touch.phase == TouchPhase.Began)
            {
                touchOrigin = touch.position;
                ray = Camera.main.ScreenPointToRay(touchOrigin);
                if (Physics.Raycast(ray, out hit, 50, CardLayer))
                {
                    selectedCard = hit.collider.GetComponentInParent<Card>();
                    if (selectedCard.isInGraveyard)
                    {
                        selectedCard = null;
                    }
                    else
                    {
                        touchTime = Time.time;
                    }
                }
            }else if (selectedCard != null)
            {
                if(Time.time - touchTime > GameManager.instance.TimeToDiscard)
                {
                    CmdDiscard(selectedCard.PosInHand);
                    selectedCard = null;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    CmdUseCard(selectedCard.PosInHand);
                    selectedCard = null;
                }
            }
        }
    }


    public void Clear()
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            if (Hand[i] != null)
            {
                Destroy(Hand[i].gameObject);
            }
        }
    }
    /*
    private void OnDestroy()
    {
        if (GameManager.exiting == false)
        {
            Debug.Log("A player has disconnected");
            GameManager.BackToMainMenu();
        }
    }
    */
}

[System.Serializable]
public class Stats
{
    public PlayerStat[] Stat = new PlayerStat[8];

    public void Set(int Hp, int Defense, int Workers, int Materials, int Generals, int Soldiers, int Mages, int Magic)
    {
        Stat[0].Value = Hp;
        Stat[1].Value = Defense;
        Stat[2].Value = Workers;
        Stat[3].Value = Materials;
        Stat[4].Value = Generals;
        Stat[5].Value = Soldiers;
        Stat[6].Value = Mages;
        Stat[7].Value = Magic;
    }

    public void Update()
    {
        Stat[(int) StatsType.Materials].Value += Stat[(int)StatsType.Workers].Value;
        Stat[(int)StatsType.Soldiers].Value += Stat[(int)StatsType.Generals].Value;
        Stat[(int)StatsType.Magic].Value += Stat[(int)StatsType.Mages].Value;
    }

    public void Add(Stats stats)
    {
        Stat[0].Value = stats.Stat[0].Value;
        Stat[1].Value = stats.Stat[1].Value;
        Stat[2].Value = stats.Stat[2].Value;
        Stat[3].Value = stats.Stat[3].Value;
        Stat[4].Value = stats.Stat[4].Value;
        Stat[5].Value = stats.Stat[5].Value;
        Stat[6].Value = stats.Stat[6].Value;
        Stat[7].Value = stats.Stat[7].Value;
    }

    public void ApplyStats(int Amount, StatsType statsType)
    {
        Stat[(int)statsType].Value += Amount;
    }

    public int GetStat(StatsType statsType)
    {
        return Stat[(int)statsType].Value;
    }
}

[System.Serializable]
public class PlayerStat
{
    [SerializeField]
    private int value;
    public IntEvent StatEvent;
    public int Value
    {
        get { return value; }
        set
        {
            int newValue = Mathf.Clamp(value, 0, 100);
            if (newValue != this.value) { this.value = newValue; StatEvent.Invoke(newValue); }
            else { this.value = newValue; }
        }
    }
}


public enum StatsType
{
    Hp,
    Defense,
    Workers,
    Materials,
    Generals,
    Soldiers,
    Mages,
    Magic
}

[System.Serializable]
public class IntEvent : UnityEvent<int>
{

}