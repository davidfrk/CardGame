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
    public GameObject changeEffectTransform;
    public Text changeEffectText;

    private int stat = 0;

    public void OnStatChange(int stat) {
        text.text = stat.ToString();
        changeEffectText.text = (stat - this.stat).ToString();
        this.stat = stat;
        if(GameManager.instance.isVisualEffectsActive)
            changeEffectTransform.SetActive(true);
    }
}
