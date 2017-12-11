using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        isMyTurn = false;
        Hand[pos].Activate(this, Target);
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
        }
    }
}

[System.Serializable]
public class Stats
{
    private int hp;
    private int defense;
    private int workers;
    private int materials;
    private int generals;
    private int soldiers;
    private int mages;
    private int magic;

    public int Hp
    {
        get { return hp; }
        set
        {
            if (value <= 0)
            {
                hp = 0;
                //Lost
            }
            else if (value >= 100)
            {
                hp = 100;
                //Won
            }
            else
            {
                hp = value;
            }
        }
    }

    public int Defense
    {
        get { return defense; }
        set { defense = Mathf.Clamp(value, 0, 100); }
    }

    public int Workers
    {
        get { return workers; }
        set { workers = Mathf.Clamp(value, 0, 100); }
    }

    public int Materials
    {
        get { return materials; }
        set { materials = Mathf.Clamp(value, 0, 100); }
    }

    public int Generals
    {
        get { return generals; }
        set { generals = Mathf.Clamp(value, 0, 100); }
    }

    public int Soldiers
    {
        get { return soldiers; }
        set { soldiers = Mathf.Clamp(value, 0, 100); }
    }

    public int Mages
    {
        get { return mages; }
        set { mages = Mathf.Clamp(value, 0, 100); }
    }

    public int Magic
    {
        get { return magic; }
        set { defense = Mathf.Clamp(value, 0, 100); }
    }

    public void Set(int Hp, int Defense, int Workers, int Materials, int Generals, int Soldiers, int Mages, int Magic)
    {
        this.Hp = Hp;
        this.Defense = Defense;
        this.Workers = Workers;
        this.Materials = Materials;
        this.Generals = Generals;
        this.Soldiers = Soldiers;
        this.Mages = Mages;
        this.Magic = Magic;
    }

    public void Update()
    {
        Materials += Workers;
        Soldiers += Generals;
        Magic += Mages;
    }

    public void Add(Stats stats)
    {
        Hp += stats.Hp;
        Defense += stats. Defense;
        Workers += stats.Workers;
        Materials += stats.Materials;
        Generals += stats.Generals;
        Soldiers += stats.Soldiers;
        Mages += stats.Mages;
        Magic += stats.Magic;
    }

    public bool isGreater(Stats other)
    {
        return (hp >= other.hp && Defense >= other.Defense && Workers >= other.Workers && Materials >= other.Materials &&
                Generals >= other.Generals && Soldiers >= other.Soldiers && Mages >= other.Mages && Magic >= other.Magic);
    }

    public void ApplyStats(int Amount, StatsType statsType)
    {
        switch (statsType)
        {
            case StatsType.Hp:
                {
                    Hp += Amount;
                    break;
                }
            case StatsType.Defense:
                {
                    Defense += Amount;
                    break;
                }
            case StatsType.Workers:
                {
                    Workers += Amount;
                    break;
                }
            case StatsType.Materials:
                {
                    Materials += Amount;
                    break;
                }
            case StatsType.Generals:
                {
                    Generals += Amount;
                    break;
                }
            case StatsType.Soldiers:
                {
                    Soldiers += Amount;
                    break;
                }
            case StatsType.Mages:
                {
                    Mages += Amount;
                    break;
                }
            case StatsType.Magic:
                {
                    Magic += Amount;
                    break;
                }
            default:
                break;
        }
    }

    public int GetStat(StatsType statsType)
    {
        switch (statsType)
        {
            case StatsType.Hp:
                {
                    return Hp; ;
                }
            case StatsType.Defense:
                {
                    return Defense; ;
                }
            case StatsType.Workers:
                {
                    return Workers;
                }
            case StatsType.Materials:
                {
                    return Materials;
                }
            case StatsType.Generals:
                {
                    return Generals;
                }
            case StatsType.Soldiers:
                {
                    return Soldiers;
                }
            case StatsType.Mages:
                {
                    return Mages;
                }
            case StatsType.Magic:
                {
                    return Magic;
                }
            default:
                return 0;
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
