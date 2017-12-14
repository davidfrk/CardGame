using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour {

    public Text victoryText;

    private void Start()
    {
        GameManager.instance.VictoryEvent.AddListener(OnVictory);
        gameObject.SetActive(false);
    }

    public void OnVictory(Player player, VictoryType type)
    {
        victoryText.text = player.gameObject.name + " Won by " + type.ToString()+ "!!";
        gameObject.SetActive(true);
    }

    public void RematchButton()
    {
        gameObject.SetActive(false);
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
