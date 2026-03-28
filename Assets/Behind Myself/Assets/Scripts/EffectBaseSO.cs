using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "EffectType", order = 1)]
public class EffectBaseSO : ScriptableObject
{
    public int effectID;
    public string displayName = "";
    public EffectType effectType;
    public BoonType boonType;
    public string spriteKey;
}
