using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoBehaviour {

    private Player player;
    public Transform castle;
    public Transform defense;
    public float castleHeight;
    public float defenseHeight;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start () {
        player.stats.Stat[(int)StatsType.Hp].StatEvent.AddListener(CastleUpdate);
        player.stats.Stat[(int)StatsType.Defense].StatEvent.AddListener(DefenseUpdate);
    }

    void CastleUpdate(int value)
    {
        castle.transform.localPosition = new Vector3(0, castleHeight / 100f * value, 0);
    }

    void DefenseUpdate(int value)
    {
        defense.transform.localPosition = new Vector3(0, defenseHeight / 100f * value, defense.transform.localPosition.z);
    }
}
