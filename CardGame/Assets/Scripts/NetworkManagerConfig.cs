using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerConfig : MonoBehaviour {

    NetworkManager networkManager;
    NetworkManagerHUD networkHUD;

    private void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
        networkHUD = networkManager.GetComponent<NetworkManagerHUD>();
    }

    void Start () {
        switch (GameManager.GameMode)
        {
            case GameModeType.PVE:
                {
                    networkHUD.showGUI = false;
                    networkManager.StartHost();
                    break;
                }
            case GameModeType.PVP_Local:
                {
                    networkHUD.showGUI = false;
                    networkManager.StartHost();
                    break;
                }
            case GameModeType.PVP_Online:
                {
                    networkHUD.showGUI = true;
                    break;
                }
            default:
                break;
        }
    }
}
