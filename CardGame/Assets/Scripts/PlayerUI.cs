using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    internal Player player;
    public Text timer;
    public StatUI[] stats = new StatUI[8];

    private int remainingTime = 0;

    public void Subscribe(Player player)
    {
        this.player = player;
        for(int i = 0; i < 8; i++)
        {
            player.stats.Stat[i].StatEvent.AddListener(stats[i].OnStatChange);
        }
    }

    private void Update()
    {
        if (player != null && player.isMyTurn)
        {
            int time = (int)(player.turnStartTime + GameManager.instance.TurnDuration - Time.time);
            if (remainingTime != time)
            {
                remainingTime = time;
                timer.text = remainingTime.ToString();
            }
            timer.gameObject.SetActive(true);
        }
        else
        {
            timer.gameObject.SetActive(false);
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
