using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Card : MonoBehaviour {

    [Header("Cost")]
    public int Cost;
    public StatsType CostType;

    [Header("Effect")]
    public bool hasTarget = true;
    public int EffectStrength = 1;
    public int SecondaryEffectStrength = 0;
    public TargetPlayers targetPlayers;
    public TargetStats targetStats;

    internal bool flipState = false;
    internal Transform targetTransform;
    internal Quaternion targetRotation;

    internal int PosInHand;

    [Header("Design")]
    public Text cardName;
    public Text costText;
    public Image costImage;
    public Text descriptionText;


    public bool isPlayable(Player Owner)
    {
        return (Owner.stats.GetStat(CostType) >= Cost);
    }

    public void Activate(Player Owner, Player Target)
    {
        if (hasTarget && Target == null)
        {
            Debug.LogError("Null Target when activating effect");
            return;
        }
        Owner.ApplyStats(-Cost, CostType);
        
        switch (targetPlayers)
        {
            case TargetPlayers.Owner:
                {
                    ApplyEffect(Owner, 1);
                    break;
                }
            case TargetPlayers.Target:
                {
                    ApplyEffect(Target, 1);
                    break;
                }
            case TargetPlayers.Both:
                {
                    ApplyEffect(Owner, 1);
                    ApplyEffect(Target, 1);
                    break;
                }
            case TargetPlayers.Steal:
                {
                    ApplyEffect(Owner, 1);
                    ApplyEffect(Target, -1);
                    break;
                }
            default:
                break;
        }
    }

    void ApplyEffect(Player Target, int Multiplier)
    {
        switch (targetStats)
        {
            case TargetStats.Hp:
                {
                    Target.stats.Stat[0].Value += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Defense:
                {
                    Target.stats.Stat[1].Value += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Workers:
                {
                    Target.stats.Stat[2].Value += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Materials:
                {
                    Target.stats.Stat[3].Value += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Generals:
                {
                    Target.stats.Stat[4].Value += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Soldiers:
                {
                    Target.stats.Stat[5].Value += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Mages:
                {
                    Target.stats.Stat[6].Value += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Magic:
                {
                    Target.stats.Stat[7].Value += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Stocks:
                {
                    Target.stats.Stat[(int)StatsType.Materials].Value += EffectStrength * Multiplier;
                    Target.stats.Stat[(int)StatsType.Soldiers].Value += EffectStrength * Multiplier;
                    Target.stats.Stat[(int)StatsType.Magic].Value += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Attack:
                {
                    int damage = EffectStrength - Target.stats.Stat[(int)StatsType.Defense].Value;
                    damage = Mathf.Max(0, damage);

                    Target.stats.Stat[(int)StatsType.Defense].Value -= EffectStrength;
                    Target.stats.Stat[(int)StatsType.Hp].Value -= damage;
                    break;
                }
            case TargetStats.All:
                {
                    int Strength = EffectStrength * Multiplier;
                    for (int i = 0; i <8; i++)
                    {
                        Target.stats.Stat[i].Value += Strength;
                    }
                    break;
                }
            default:
                break;
        }
    }

    public void Flip()
    {
        flipState = !flipState;
        UpdateRotation();
    }

    public void Flip(bool state)
    {
        flipState = state;
        UpdateRotation();
    }

    public void SetActive(bool state)
    {
        this.gameObject.SetActive(state);
    }

    public void MoveTo(Transform transform)
    {
        targetTransform = transform;
        UpdateRotation();
    }

    public void UpdateRotation()
    {
        if (flipState == true)
        {
            targetRotation = Quaternion.LookRotation(-targetTransform.forward, targetTransform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(targetTransform.forward, targetTransform.up);
        }
    }

    void Update()
    {
        if (targetTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * GameManager.instance.CardMovementSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * GameManager.instance.CardRotationSpeed);
        }
    }

    public void Discard()
    {
        flipState = true;
        MoveTo(RandomDeck.instance.Graveyard);
        RandomDeck.instance.AddToGraveyard(this);
    }

    public void UpdateDesign()
    {
        costImage.sprite = GameManager.instance.StatsSymbols[(int)CostType];
        costText.text = Cost.ToString();
        descriptionText.text = "Affects " + targetPlayers.ToString() + "\n"
                                + targetStats.ToString() + " by " + EffectStrength;
        cardName.text = gameObject.name;
    }
}

public enum TargetPlayers
{
    Owner,
    Target,
    Both,
    Steal
}

public enum TargetStats
{
    Hp,
    Defense,
    Workers,
    Materials,
    Generals,
    Soldiers,
    Mages,
    Magic,
    Stocks,
    Attack,
    All,
}

#if UNITY_EDITOR
[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Card card = (Card)target;
        if (GUILayout.Button("UpdateDesign"))
        {
            card.UpdateDesign();
        }
    }
}
#endif
