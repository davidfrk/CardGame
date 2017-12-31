using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour {

	void Start () {
        GameManager.instance.GameStart.AddListener(OnGameStart);
	}

    public void OnGameStart()
    {
        gameObject.SetActive(false);
    }
	
    public void Exit()
    {
        GameManager.BackToMainMenu();
    }
}
