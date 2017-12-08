using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int hp = 25;
    public int defense = 0;
    public Stats stats;
    public Card[] Hand = new Card[8];

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
            }else if(value >= 100){
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
        get
        {
            return defense;
        }
        set
        {
            defense = Mathf.Clamp(value, 0, 100);
        }
    }
    
	void Start () {
        OnGameStart();
	}

    public void OnGameStart()
    {
        hp = 25;
        defense = 0;
        stats.Set(2, 5, 2, 5, 2, 5);
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
    public int Workers;
    public int Materials;
    public int Generals;
    public int Soldiers;
    public int Mages;
    public int Magic;

    public void Set(int Workers, int Materials, int Generals, int Soldiers, int Mages, int Magic)
    {
        this.Workers += Workers;
        this.Materials += Materials;
        this.Generals += Generals;
        this.Soldiers += Soldiers;
        this.Mages += Mages;
        this.Magic += Magic;
    }

    public void Update()
    {
        Materials += Workers;
        Soldiers += Generals;
        Magic += Mages;
    }

    public void Add(Stats stats)
    {
        Workers += stats.Workers;
        Materials += stats.Materials;
        Generals += stats.Generals;
        Soldiers += stats.Soldiers;
        Mages += stats.Mages;
        Magic += stats.Magic;

        if (Workers < 0) Workers = 0;
        if (Materials < 0) Materials = 0;
        if (Generals < 0) Generals = 0;
        if (Soldiers < 0) Soldiers = 0;
        if (Mages < 0) Mages = 0;
        if (Magic < 0) Magic = 0;
    }
}
