using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour {

    public Stats stats;
    public Card[] Hand = new Card[8];
    public Player Enemy;
    public LayerMask CardLayer;

    public bool isMyTurn = false;
    private Ray ray;
    private RaycastHit hit;

    private int numberOfCards = 8;
    private bool isFirstTurn = true;
    private int turn = 0;

    void Start () {
        OnGameStart();
	}

    public void OnGameStart()
    {
        stats.Set(30, 5, 2, 5, 2, 5, 2, 5);
        if (isMyTurn)
        {
            isMyTurn = false;
            StartTurn();
        }
    }

    public void DrawCards()
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            DrawCard(i);
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
    }

    public void OnTurnStart()
    {
        isMyTurn = true;
        Debug.Log("Turn " + turn + " to " + this.gameObject);
    }

    public void OnTurnEnd()
    {
        ShowCards(false);
        isMyTurn = false;
        Enemy.StartTurn();
    }

    public void UseCard(int pos, Player Target)
    {
        if (Hand[pos].isPlayable(this))
        {
            isMyTurn = false;
            Hand[pos].Activate(this, Target);
            Hand[pos].Discard();
            DrawCard(pos);
            Invoke("FlipCardsToEndTurn", GameManager.instance.DrawCardDuration);
        }
        else
        {
            //PlaySound
        }
    }

    public void Discard(int pos)
    {
        isMyTurn = false;
        Hand[pos].Discard();
        DrawCard(pos);
        Invoke("FlipCardsToEndTurn", GameManager.instance.DrawCardDuration);
    }

    public void DrawCard(int pos)
    {
        Hand[pos] = RandomDeck.instance.DrawCard();
        Hand[pos].MoveTo(HandManager.instance.CardsTransforms[pos]);
        Hand[pos].PosInHand = pos;
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
        FlipCards(false);
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

    private void Update()
    {
        if (!isMyTurn)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50, CardLayer))
            {
                Card card = hit.collider.GetComponentInParent<Card>();
                UseCard(card.PosInHand, Enemy);
            }
        }else if (Input.GetMouseButtonDown(1))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50, CardLayer))
            {
                Card card = hit.collider.GetComponentInParent<Card>();
                Discard(card.PosInHand);
            }
        }
    }
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