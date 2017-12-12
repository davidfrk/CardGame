using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Player player;
    public StatUI[] stats = new StatUI[8];
	
	void Start () {
        Subscribe(player);
	}

    public void Subscribe(Player player)
    {
        for(int i = 0; i < 8; i++)
        {
            player.stats.Stat[i].StatEvent.AddListener(stats[i].OnStatChange);
        }
    }
}

[System.Serializable]
public class StatUI
{
    public Text text;
    public Image image;
    public void OnStatChange(int stat) {
        text.text = stat.ToString();
    }
}
