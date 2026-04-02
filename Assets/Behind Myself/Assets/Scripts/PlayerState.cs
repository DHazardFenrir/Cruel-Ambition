using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerState", menuName = "State", order = 1)]

public class PlayerState: ScriptableObject
{
    public Stats playerStats;
    public Dictionary<Condition, EffectBaseSO> activeEffects = new Dictionary<Condition, EffectBaseSO>();

    public bool HasEffect(Condition effectCondition)
    {
        return activeEffects.ContainsKey(effectCondition);
    }

}


[Serializable]
public class Stats
{
    public int health;
    public int defense;
    public int damage;
    public int endurance;
}