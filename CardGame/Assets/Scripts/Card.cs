using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private float rotationSpeed = 1;
    internal bool flipState = false;
    internal Transform targetTransform;
    internal Quaternion targetRotation;

    internal int PosInHand;


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
                    Target.stats.Hp += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Defense:
                {
                    Target.stats.Defense += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Workers:
                {
                    Target.stats.Workers += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Materials:
                {
                    Target.stats.Materials += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Generals:
                {
                    Target.stats.Generals += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Soldiers:
                {
                    Target.stats.Soldiers += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Mages:
                {
                    Target.stats.Mages += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Magic:
                {
                    Target.stats.Magic += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Stocks:
                {
                    Target.stats.Materials += EffectStrength * Multiplier;
                    Target.stats.Soldiers += EffectStrength * Multiplier;
                    Target.stats.Magic += EffectStrength * Multiplier;
                    break;
                }
            case TargetStats.Attack:
                {
                    int damage = EffectStrength - Target.stats.Defense;
                    damage = Mathf.Max(0, damage);

                    Target.stats.Defense -= EffectStrength;
                    Target.stats.Hp -= damage;
                    break;
                }
            case TargetStats.All:
                {
                    Target.stats.Hp += EffectStrength * Multiplier;
                    Target.stats.Defense += EffectStrength * Multiplier;
                    Target.stats.Workers += EffectStrength * Multiplier;
                    Target.stats.Materials += EffectStrength * Multiplier;
                    Target.stats.Generals += EffectStrength * Multiplier;
                    Target.stats.Soldiers += EffectStrength * Multiplier;
                    Target.stats.Mages += EffectStrength * Multiplier;
                    Target.stats.Magic += EffectStrength * Multiplier;
                    break;
                }
            default:
                break;
        }
    }

    public void Flip()
    {
        flipState = !flipState;
    }

    public void Flip(bool state)
    {
        flipState = state;
    }

    public void MoveTo(Transform transform)
    {
        targetTransform = transform;
        if (flipState == true)
        {
            targetRotation = Quaternion.LookRotation(-transform.forward, transform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(transform.forward, transform.up);
        }
    }

    void Update()
    {
        if (targetTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * movementSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void Discard()
    {
        Destroy(this.gameObject);
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
