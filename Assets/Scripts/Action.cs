using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[Serializable]
public class Action
{
    public string actionName;
    public string displayName;  
    public TargetType targetType;
    public ElementalType elementalType;
    public float baseDamageMultiplier;
    public float baseChanceToHit;
    public float guageUsed;
    public AttackType attackType;
    public DamageType damageType;
    public EffectType statusType;
    public int statusDuration;
    public StatType primaryPowerStat,secondaryPowerStat, primaryDefenseStat, secondaryDefenseStat;
    public float primaryPowerStatWeight, secondaryPowerStatWeight,primaryDefenseStatWeight, secondaryDefenseStatWeight;


    public float effectTriggerTime;
    public bool isEffectResolved, isTriggerEffect; 

    public float timeSinceActionStart;
    public float timeActionEnds;
    public bool isActionFinished;

    public Action(Dictionary<string,string> actionData)
    {
        actionName = actionData["ActionName"];
        displayName = actionData["DisplayName"];
        targetType = (TargetType)System.Enum.Parse(typeof(TargetType), actionData["TargetType"].ToString());
        elementalType = (ElementalType)System.Enum.Parse(typeof(ElementalType), actionData["ElementalType"].ToString());
        baseDamageMultiplier = float.Parse(actionData["BaseDamageMultiplier"]);
        baseChanceToHit = float.Parse(actionData["BaseChanceToHit"]);
        guageUsed = float.Parse(actionData["GuageUsed"]);
        attackType = (AttackType)System.Enum.Parse(typeof(AttackType), actionData["AttackType"].ToString());
        damageType = (DamageType)System.Enum.Parse(typeof(DamageType), actionData["DamageType"].ToString());
        statusType = (EffectType)System.Enum.Parse(typeof(EffectType), actionData["EffectType"].ToString());
        statusDuration = int.Parse(actionData["StatusDuration"]);
        primaryPowerStat = (StatType)System.Enum.Parse(typeof(StatType), actionData["PrimaryPowerStat"].ToString());
        primaryPowerStatWeight = float.Parse(actionData["PrimaryPowerStatWeight"]);
        primaryDefenseStat = (StatType)System.Enum.Parse(typeof(StatType), actionData["PrimaryDefenseStat"].ToString());
        primaryDefenseStatWeight = float.Parse(actionData["PrimaryDefenseStatWeight"]);
        secondaryPowerStat = (StatType)System.Enum.Parse(typeof(StatType), actionData["SecondaryPowerStat"].ToString());
        secondaryPowerStatWeight = float.Parse(actionData["SecondaryPowerStatWeight"]);
        secondaryDefenseStat = (StatType)System.Enum.Parse(typeof(StatType), actionData["SecondaryDefenseStat"].ToString());
        secondaryDefenseStatWeight = float.Parse(actionData["SecondaryDefenseStatWeight"]);
    }

    public void StartAction()
    {
        timeSinceActionStart = 0f;
        isActionFinished = false;
        isEffectResolved = false;
        isTriggerEffect = false;
        effectTriggerTime = 0.5f;
        timeActionEnds = 1.0f;
    }

    public void UpdateAction()
    {
        timeSinceActionStart += Time.deltaTime;
        if ((timeSinceActionStart > effectTriggerTime) && !isEffectResolved)
        {
            isTriggerEffect = true;
        }
        if(timeSinceActionStart > timeActionEnds)
            isActionFinished = true;
    }

}


