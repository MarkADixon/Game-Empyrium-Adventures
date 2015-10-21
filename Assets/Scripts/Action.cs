using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[Serializable]
public class Action
{
    public string actionName;
    public string displayName;
    public string description;
    public float actionGuageUsed;
    public List<ActionComponent> actionComponents = new List<ActionComponent>();
   
    public float effectTriggerTime;
    public bool isEffectResolved, isTriggerEffect; 
    public float timeSinceActionStart;
    public float timeActionEnds;
    public bool isActionFinished;

    public Character selectedTarget;


    public Action(Dictionary<string, string> actionData, List<Dictionary<string, string>> componentData)
    {
        /*
        foreach(Dictionary<string, string> data in componentData)
        {
            foreach(KeyValuePair<string, string> e in data)
            {
                Debug.Log(e.Key.ToString() + " : " + e.Value.ToString());
            }
        }
        */
      

        actionName = actionData["ActionName"];
        displayName = actionData["DisplayName"];
        description = actionData["Description"];
        actionGuageUsed = float.Parse(actionData["ActionGuageUsed"]);

        ActionComponent newComponent = new ActionComponent();
        foreach(Dictionary<string,string> component in componentData)
        {
            newComponent = new ActionComponent(component);
            actionComponents.Add(newComponent);
        }

        actionComponents.Sort((a, b) => a.executionOrder.CompareTo(b.executionOrder));

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

[Serializable]
public class ActionComponent
{
    public int executionOrder;
    public TargetType targetType;
    public HitType hitType;
    public ElementalType elementalType;
    public EffectType effectType;
    public float basePower;
    public float baseToHit;
    public int duration;

    public StatType priPowerStat = StatType.NONE;
    public float priPowerWeight = 0f;
    public StatType secPowerStat = StatType.NONE;
    public float secPowerWeight = 0f;

    public StatType priResistStat = StatType.NONE;
    public float priResistWeight = 0f;
    public StatType secResistStat = StatType.NONE;
    public float secResistWeight = 0f;

    public ActionComponent()
    { }

    public ActionComponent(Dictionary<string,string> data)
    {
        /*
        foreach(KeyValuePair<string, string> e in data)
        {
            Debug.Log(e.Key.ToString() + " : " + e.Value.ToString());
        }
        */

        executionOrder = int.Parse(data["Component"]);
        targetType = (TargetType)System.Enum.Parse(typeof(TargetType), data["TargetType"].ToString());
        hitType = (HitType)System.Enum.Parse(typeof(HitType), data["HitType"].ToString());
        elementalType = (ElementalType)System.Enum.Parse(typeof(ElementalType), data["ElementalType"].ToString());
        effectType = (EffectType)System.Enum.Parse(typeof(EffectType), data["EffectType"].ToString());
        basePower = float.Parse(data["BasePower"]);
        baseToHit = float.Parse(data["BaseToHit"]);
        duration = int.Parse(data["Duration"]);

        string[] powerStats = data["PowerStats"].Split(char.Parse(","));
        priPowerStat = (StatType)System.Enum.Parse(typeof(StatType), powerStats[0].ToString());
        if(powerStats.Length > 1)
        {
            secPowerStat = (StatType)System.Enum.Parse(typeof(StatType), powerStats[1].ToString());
        }
        priPowerWeight = float.Parse(data["PrimaryPowerStatWeight"]);
        secPowerWeight = 1f - priPowerWeight;

        string[] resistStats = data["DefenseStats"].Split(char.Parse(","));
        priResistStat = (StatType)System.Enum.Parse(typeof(StatType), resistStats[0].ToString());
        if(resistStats.Length > 1)
        {
            secResistStat = (StatType)System.Enum.Parse(typeof(StatType), resistStats[1].ToString());
        }
        priResistWeight = float.Parse(data["PrimaryDefenseStatWeight"]);
        secResistWeight = 1f - priResistWeight;
    }

}


