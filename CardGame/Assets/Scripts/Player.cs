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
            Hand[i] = RandomDeck.instance.NextCard();
        }
    }

    public void OnTurnStart()
    {
        stats.Update();
    }

    public void ApplyStats(Stats Effect)
    {
        stats.Add(Effect);
    }
}

[System.Serializable]
public class Stats
{
    public int hp;
    public int Defense;
    public int Workers;
    public int Materials;
    public int Generals;
    public int Soldiers;
    public int Mages;
    public int Magic;

    public int Hp
    {
        get
        {
            return hp;
        }
        private set
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

    public void Set(int Hp, int Defense, int Workers, int Materials, int Generals, int Soldiers, int Mages, int Magic)
    {
        this.Hp = Hp;
        this.Defense = Mathf.Clamp(Defense, 0, 100);
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

        Defense = Mathf.Clamp(Defense, 0, 100);
        if (Workers < 0) Workers = 0;
        if (Materials < 0) Materials = 0;
        if (Generals < 0) Generals = 0;
        if (Soldiers < 0) Soldiers = 0;
        if (Mages < 0) Mages = 0;
        if (Magic < 0) Magic = 0;
    }

    public bool isGreater(Stats other)
    {
        return (hp >= other.hp && Defense >= other.Defense && Workers >= other.Workers && Materials >= other.Materials &&
                Generals >= other.Generals && Soldiers >= other.Soldiers && Mages >= other.Mages && Magic >= other.Magic);
    }
}
