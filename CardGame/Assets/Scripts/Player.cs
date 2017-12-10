using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Stats stats;
    public Card[] Hand = new Card[8];
    
	void Start () {
        OnGameStart();
	}

    public void OnGameStart()
    {
        stats.Set(30, 5, 2, 5, 2, 5, 2, 5);
        for (int i = 0; i < 8; i++)
        {
            Hand[i] = RandomDeck.instance.DrawCard();
            Hand[i].MoveTo(HandManager.instance.CardsTransforms[i]);
        }
    }

    public void OnTurnStart()
    {
        stats.Update();
    }

    public void OnTurnEnd()
    {

    }

    public void UseCard(int pos)
    {

    }

    public void ApplyStats(Stats Effect)
    {
        stats.Add(Effect);
    }

    public void ApplyStats(int Amount, StatsType statsType)
    {
        stats.ApplyStats(Amount, statsType);
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
