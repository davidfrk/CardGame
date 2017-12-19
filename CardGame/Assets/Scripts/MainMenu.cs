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
        SceneManager.LoadScene("PvE_Scene");
    }

    public void PVPButton()
    {
        SceneManager.LoadScene("Pvp_Scene");
    }

    public void MultiplayerButton()
    {
        SceneManager.LoadScene("PrototypeScene");
    }

    public void BackButton()
    {
        GameModeButtons.SetActive(false);
        MainButtons.SetActive(true);
    }

}
