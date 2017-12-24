using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject MainButtons;
    public GameObject GameModeButtons;

    public void PlayButton()
    {
        MainButtons.SetActive(false);
        GameModeButtons.SetActive(true);
    }

    public void InstructionsButton()
    {

    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void PVEButton()
    {
        GameManager.GameMode = GameModeType.PVE;
        SceneManager.LoadScene("GameScene");
    }

    public void PVPButton()
    {
        GameManager.GameMode = GameModeType.PVP_Local;
        SceneManager.LoadScene("GameScene");
    }

    public void MultiplayerButton()
    {
        GameManager.GameMode = GameModeType.PVP_Online;
        SceneManager.LoadScene("GameScene");
    }

    public void BackButton()
    {
        GameModeButtons.SetActive(false);
        MainButtons.SetActive(true);
    }

}
