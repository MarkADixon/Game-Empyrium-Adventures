using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[Serializable]
public class Action
{
    public TargetType targetType;
    public Character targetCharacter;

    //action hits on same target
    public int numberOfHitsToTarget;
    bool isMultipleHits;

    //action repeats with new target
    public int numberOfActionRepeats;
    bool isRepeatingAction;

    public List<ActionEffect> actionEffects = new List<ActionEffect>();

}

[Serializable]
public class ActionEffect
{
    public float damageMultiplier;
    public ElementalType element;

    public bool isPhysical_EvadeBlockAbsorb;
    public bool isMagical_EvadeBlockAbsorb;



}

