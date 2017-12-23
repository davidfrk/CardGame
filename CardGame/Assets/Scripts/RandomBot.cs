using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBot : MonoBehaviour {

    private Player player;
    public bool isActive;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        if (GameManager.instance.GameMode != GameModeType.PVE)
        {
            isActive = false;
        }else if (isActive)
        {
            player.TurnStartEvent.AddListener(Play);
            GameManager.instance.VictoryEvent.AddListener(OnVictory);
            player.mustOpenCards = false;
        }
    }

    public void Play()
    {
        for(int i = 0; i < player.numberOfCards; i++)
        {
            if (player.Hand[i].isPlayable(player))
            {
                player.UseCard(i);
                break;
            }
        }
        //Se não conseguiu usar nenhuma carta, descarte
        if (player.isMyTurn)
        {
            player.Discard(Random.Range(0, player.numberOfCards));
        }
    }

    private void OnVictory(Player player, VictoryType type)
    {
        Invoke("TryRematch", 1);
    }

    private void TryRematch()
    {
        player.TryRematch();
    }
}
