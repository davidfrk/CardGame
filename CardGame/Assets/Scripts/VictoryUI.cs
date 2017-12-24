using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour {

    public Text victoryText;
    public Image[] RematchMark;

    private void Start()
    {
        GameManager.instance.GameStart.AddListener(HideUI);
        GameManager.instance.VictoryEvent.AddListener(OnVictory);
        GameManager.instance.RematchStatus.AddListener(UpdateRematchStatus);
        gameObject.SetActive(false);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void OnVictory(Player player, VictoryType type)
    {
        victoryText.text = player.gameObject.name + " Won by " + type.ToString()+ "!!";
        gameObject.SetActive(true);
    }

    public void RematchButton()
    {
        Player.LocalPlayer.TryRematch();
        //gameObject.SetActive(false);
        //GameManager.instance.Rematch();
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
        GameManager.BackToMainMenu();
    }

    public void UpdateRematchStatus()
    {
        GameManager gameManager = GameManager.instance;
        if (gameManager.players.Count == 2)
        {
            RematchMark[0].sprite = gameManager.GetCheckBoxSprite(gameManager.players[0].wantRematch);
            RematchMark[1].sprite = gameManager.GetCheckBoxSprite(gameManager.players[1].wantRematch);
        }
    }
}
