using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    SACRIFICE,
    BOON,
    DETRIMENT,
    PASSIVE
}

public enum BoonType
{
    SPEED,
    HEALTH,
    ENDURANCE,
    DAMAGE,
    WISDOM
}

public enum Tags
{
    DIALOGUE,
    ANSWER,
    CHOICE
}

public enum TypeSpeaker
{
    SPEAKER,
    PLAYER
}

public enum Condition
{
    NONE,
    HAS_WEAPON,
    HAS_TRANSFORM,
    HAS_TRANSPARENCY,
    IS_MARRIED,
    HAS_STRETCH
}
