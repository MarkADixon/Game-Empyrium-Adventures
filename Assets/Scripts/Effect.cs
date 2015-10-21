using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


[Serializable]
public class Effect
{
    public HitType hitType;
    public ElementalType elementalType;
    public EffectType effectType;
    public float basePower;
    public float powerStat;
    public float resistStat;
    public float baseToHit;
    public int duration;

    public Character targetUnit;
    public Character sourceUnit;

    public StatType priPowerStat = StatType.NONE;
    public float priPowerWeight = 0f;
    public StatType secPowerStat = StatType.NONE;
    public float secPowerWeight = 0f;

    public StatType priResistStat = StatType.NONE;
    public float priResistWeight = 0f;
    public StatType secResistStat = StatType.NONE;
    public float secResistWeight = 0f;

    public Effect (Character _targetUnit, ActionComponent actionComponent, Character _sourceUnit )
    {
        hitType = actionComponent.hitType;
        elementalType = actionComponent.elementalType;
        effectType = actionComponent.effectType;
        basePower = actionComponent.basePower;
        baseToHit = actionComponent.baseToHit;
        duration = actionComponent.duration;
        targetUnit = _targetUnit;
        sourceUnit = _sourceUnit;

        priPowerStat = actionComponent.priPowerStat;
        priPowerWeight = actionComponent.priPowerWeight;
        secPowerStat = actionComponent.secPowerStat;
        secPowerWeight = actionComponent.secPowerWeight;

        priResistStat = actionComponent.priResistStat;
        priResistWeight = actionComponent.priResistWeight;
        secResistStat = actionComponent.secResistStat;
        secResistWeight = actionComponent.secResistWeight;

        powerStat = GetStatPower(sourceUnit);
        resistStat = GetStatResist(targetUnit);
    }

    public void Tick()
    {

    }

    float GetStatPower(Character unit)
    {
        float power = 0f;
        switch(priPowerStat)
        {
            case StatType.ATTACK:
                {
                    power = unit.sheet.stats.attack * priPowerWeight;
                    break;
                }
            case StatType.DEFENSE:
                {
                    power = unit.sheet.stats.defense * priPowerWeight;
                    break;
                }
            case StatType.AGILITY:
                {
                    power = unit.sheet.stats.agility * priPowerWeight;
                    break;
                }
            case StatType.MAGIC:
                {
                    power = unit.sheet.stats.magic * priPowerWeight;
                    break;
                }
            case StatType.SPIRIT:
                {
                    power = unit.sheet.stats.spirit * priPowerWeight;
                    break;
                }
            case StatType.MIND:
                {
                    power = unit.sheet.stats.mind * priPowerWeight;
                    break;
                }
            case StatType.CHARISMA:
                {
                    power = unit.sheet.stats.charisma * priPowerWeight;
                    break;
                }
            case StatType.RESOLVE:
                {
                    power = unit.sheet.stats.resolve * priPowerWeight;
                    break;
                }
            case StatType.SPEED:
                {
                    power = unit.sheet.stats.speed * priPowerWeight;
                    break;
                }
            case StatType.LUCK:
                {
                    power = unit.sheet.stats.luck * priPowerWeight;
                    break;
                }
            default:
                break;
        }
        switch(secPowerStat)
        {
            case StatType.ATTACK:
                {
                    power += unit.sheet.stats.attack * secPowerWeight;
                    break;
                }
            case StatType.DEFENSE:
                {
                    power += unit.sheet.stats.defense * secPowerWeight;
                    break;
                }
            case StatType.AGILITY:
                {
                    power += unit.sheet.stats.agility * secPowerWeight;
                    break;
                }
            case StatType.MAGIC:
                {
                    power += unit.sheet.stats.magic * secPowerWeight;
                    break;
                }
            case StatType.SPIRIT:
                {
                    power += unit.sheet.stats.spirit * secPowerWeight;
                    break;
                }
            case StatType.MIND:
                {
                    power += unit.sheet.stats.mind * secPowerWeight;
                    break;
                }
            case StatType.CHARISMA:
                {
                    power += unit.sheet.stats.charisma * secPowerWeight;
                    break;
                }
            case StatType.RESOLVE:
                {
                    power += unit.sheet.stats.resolve * secPowerWeight;
                    break;
                }
            case StatType.SPEED:
                {
                    power += unit.sheet.stats.speed * secPowerWeight;
                    break;
                }
            case StatType.LUCK:
                {
                    power += unit.sheet.stats.luck * secPowerWeight;
                    break;
                }
            default:
                break;
        }

        return power;
    }

    float GetStatResist(Character unit)
    {
        float resist = 0f;
        switch(priResistStat)
        {
            case StatType.ATTACK:
                {
                    resist = unit.sheet.stats.attack * priResistWeight;
                    break;
                }
            case StatType.DEFENSE:
                {
                    resist = unit.sheet.stats.defense * priResistWeight;
                    break;
                }
            case StatType.AGILITY:
                {
                    resist = unit.sheet.stats.agility * priResistWeight;
                    break;
                }
            case StatType.MAGIC:
                {
                    resist = unit.sheet.stats.magic * priResistWeight;
                    break;
                }
            case StatType.SPIRIT:
                {
                    resist = unit.sheet.stats.spirit * priResistWeight;
                    break;
                }
            case StatType.MIND:
                {
                    resist = unit.sheet.stats.mind * priResistWeight;
                    break;
                }
            case StatType.CHARISMA:
                {
                    resist = unit.sheet.stats.charisma * priResistWeight;
                    break;
                }
            case StatType.RESOLVE:
                {
                    resist = unit.sheet.stats.resolve * priResistWeight;
                    break;
                }
            case StatType.SPEED:
                {
                    resist = unit.sheet.stats.speed * priResistWeight;
                    break;
                }
            case StatType.LUCK:
                {
                    resist = unit.sheet.stats.luck * priResistWeight;
                    break;
                }
            default:
                break;
        }
        switch(secResistStat)
        {
            case StatType.ATTACK:
                {
                    resist += unit.sheet.stats.attack * secResistWeight;
                    break;
                }
            case StatType.DEFENSE:
                {
                    resist += unit.sheet.stats.defense * secResistWeight;
                    break;
                }
            case StatType.AGILITY:
                {
                    resist += unit.sheet.stats.agility * secResistWeight;
                    break;
                }
            case StatType.MAGIC:
                {
                    resist += unit.sheet.stats.magic * secResistWeight;
                    break;
                }
            case StatType.SPIRIT:
                {
                    resist += unit.sheet.stats.spirit * secResistWeight;
                    break;
                }
            case StatType.MIND:
                {
                    resist += unit.sheet.stats.mind * secResistWeight;
                    break;
                }
            case StatType.CHARISMA:
                {
                    resist += unit.sheet.stats.charisma * secResistWeight;
                    break;
                }
            case StatType.RESOLVE:
                {
                    resist += unit.sheet.stats.resolve * secResistWeight;
                    break;
                }
            case StatType.SPEED:
                {
                    resist += unit.sheet.stats.speed * secResistWeight;
                    break;
                }
            case StatType.LUCK:
                {
                    resist += unit.sheet.stats.luck * secResistWeight;
                    break;
                }
            default:
                break;
        }

        return resist;
    }
}
