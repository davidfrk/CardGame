using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Player player;
    public StatUI[] Stats = new StatUI[8];
	
	void Start () {
		
	}

    public void Subscribe()
    {

    }
}

[System.Serializable]
public class StatUI
{
    public Text text;
    public Image image;
}
