using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class NetworkManagerConfig : MonoBehaviour {

    NetworkManager networkManager;
    NetworkManagerHUD networkHUD;
    NetworkMatch networkMatch;

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
                    networkHUD.showGUI = false;
                    networkManager.StartMatchMaker();
                    networkManager.matchMaker.ListMatches(0, 20, "match", false, 0, 0, OnMatchList);
                    break;
                }
            default:
                break;
        }
    }

    private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> responseData)
    {
        networkManager.OnMatchList(success, extendedInfo, responseData);
        if (success)
        {
            if (networkManager.matches.Count > 0)
            {
                networkManager.matchMaker.JoinMatch(networkManager.matches[0].networkId, "", "", "", 0, 0, OnMatchJoined);
            }
            else
            {
                networkManager.matchMaker.CreateMatch("match", 2, true, "", "", "", 0, 0, OnMatchCreate);
            }
        }
        else
        {

        }
    }

    private void OnMatchJoined(bool success, string extendedInfo, MatchInfo responseData)
    {
        networkManager.OnMatchJoined(success, extendedInfo, responseData);
    }

    private void OnMatchCreate(bool success, string extendedInfo, MatchInfo responseData)
    {
        networkManager.OnMatchCreate(success, extendedInfo, responseData);
    }
}
