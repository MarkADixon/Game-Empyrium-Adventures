using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


public class Effect
{
    public HitType hitType;
    public ElementalType elementalType;
    public EffectType effectType;
    public float basePower;
    public float baseToHit;
    public int duration;
   
    public float sourcePowerStat = 0f;

    public StatType priPowerStat = StatType.NONE;
    public float priPowerWeight = 0f;
    public StatType secPowerStat = StatType.NONE;
    public float secPowerWeight = 0f;

    public StatType priResistStat = StatType.NONE;
    public float priResistWeight = 0f;
    public StatType secResistStat = StatType.NONE;
    public float secResistWeight = 0f;

    [NonSerialized]
    private Character sourceUnit;
    [NonSerialized]
    private Character targetUnit;

    private float appliedChange; //for effects that need to reverse themselves when removed

    public Effect (ActionComponent actionComponent, Character _sourceUnit, Character _targetUnit)
    {
        hitType = actionComponent.hitType;
        elementalType = actionComponent.elementalType;
        effectType = actionComponent.effectType;
        basePower = actionComponent.basePower;
        baseToHit = actionComponent.baseToHit;
        duration = actionComponent.duration;

        priPowerStat = actionComponent.priPowerStat;
        priPowerWeight = actionComponent.priPowerWeight;
        secPowerStat = actionComponent.secPowerStat;
        secPowerWeight = actionComponent.secPowerWeight;

        priResistStat = actionComponent.priResistStat;
        priResistWeight = actionComponent.priResistWeight;
        secResistStat = actionComponent.secResistStat;
        secResistWeight = actionComponent.secResistWeight;

        sourceUnit = _sourceUnit;
        targetUnit = _targetUnit;

        sourcePowerStat = GetStatPower(_sourceUnit);
    }

    public void AddEffect()
    {
        if(duration == 0)  //apply immeadiate effects, then expire them (dont add them)
        {
            ApplyEffect();
            targetUnit = null;
            sourceUnit = null;
            return;
        }

        targetUnit.effects.Add(this);

        //stat modifying effects get applied on creadtion/add
        switch(effectType)
        {
            case EffectType.MODIFY_MAXHP:
                {
                    appliedChange = Effect_ModifyStatCalc();
                    if ((appliedChange + targetUnit.sheet.stats.maxHealth) <= 0)
                    {
                        appliedChange = -1f*(targetUnit.sheet.stats.maxHealth - 1);
                    }
                    targetUnit.sheet.stats.maxHealth += (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_ATTACK:
                {
                    appliedChange = Effect_ModifyStatCalc();
                    if((appliedChange + targetUnit.sheet.stats.attack) <= 0)
                    {
                        appliedChange = -1f * (targetUnit.sheet.stats.attack - 1);
                    }
                    targetUnit.sheet.stats.attack += (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_DEFENSE:
                {
                    appliedChange = Effect_ModifyStatCalc();
                    if((appliedChange + targetUnit.sheet.stats.defense) <= 0)
                    {
                        appliedChange = -1f * (targetUnit.sheet.stats.defense - 1);
                    }
                    targetUnit.sheet.stats.defense += (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_AGILITY:
                {
                    appliedChange = Effect_ModifyStatCalc();
                    if((appliedChange + targetUnit.sheet.stats.agility) <= 0)
                    {
                        appliedChange = -1f * (targetUnit.sheet.stats.agility - 1);
                    }
                    targetUnit.sheet.stats.agility += (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_SPIRIT:
                {
                    appliedChange = Effect_ModifyStatCalc();
                    if((appliedChange + targetUnit.sheet.stats.spirit) <= 0)
                    {
                        appliedChange = -1f * (targetUnit.sheet.stats.spirit - 1);
                    }
                    targetUnit.sheet.stats.spirit += (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_MIND:
                {
                    appliedChange = Effect_ModifyStatCalc();
                    if((appliedChange + targetUnit.sheet.stats.mind) <= 0)
                    {
                        appliedChange = -1f * (targetUnit.sheet.stats.mind - 1);
                    }
                    targetUnit.sheet.stats.mind+= (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_CHARISMA:
                {
                    appliedChange = Effect_ModifyStatCalc();
                    if((appliedChange + targetUnit.sheet.stats.charisma) <= 0)
                    {
                        appliedChange = -1f * (targetUnit.sheet.stats.charisma - 1);
                    }
                    targetUnit.sheet.stats.charisma += (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_RESOLVE:
                {
                    appliedChange = Effect_ModifyStatCalc();
                    if((appliedChange + targetUnit.sheet.stats.resolve) <= 0)
                    {
                        appliedChange = -1f * (targetUnit.sheet.stats.resolve - 1);
                    }
                    targetUnit.sheet.stats.resolve += (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_SPEED:
                {
                    appliedChange = Effect_ModifyStatCalc();
                    if((appliedChange + targetUnit.sheet.stats.speed) <= 0)
                    {
                        appliedChange = -1f * (targetUnit.sheet.stats.speed - 1);
                    }
                    targetUnit.sheet.stats.speed += (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_LUCK:
                {
                    appliedChange = Effect_ModifyStatCalc();
                    if((appliedChange + targetUnit.sheet.stats.luck) <= 0)
                    {
                        appliedChange = -1f * (targetUnit.sheet.stats.luck - 1);
                    }
                    targetUnit.sheet.stats.luck += (int)appliedChange;
                    break;
                }
            default:
                break;
        }

    }

    public void RemoveEffect()
    {
        //stat modifying effects reverse
        switch(effectType)
        {
            case EffectType.MODIFY_MAXHP:
                {
                    targetUnit.sheet.stats.maxHealth -= (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_ATTACK:
                {
                    targetUnit.sheet.stats.attack -= (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_DEFENSE:
                {
                    targetUnit.sheet.stats.defense -= (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_AGILITY:
                {
                    targetUnit.sheet.stats.agility -= (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_SPIRIT:
                {
                    targetUnit.sheet.stats.spirit -= (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_MIND:
                {
                    targetUnit.sheet.stats.mind -= (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_CHARISMA:
                {
                    targetUnit.sheet.stats.charisma -= (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_RESOLVE:
                {
                    targetUnit.sheet.stats.resolve -= (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_SPEED:
                {
                    targetUnit.sheet.stats.speed -= (int)appliedChange;
                    break;
                }
            case EffectType.MODIFY_LUCK:
                {
                    targetUnit.sheet.stats.luck -= (int)appliedChange;
                    break;
                }
            default:
                break;
        }

        targetUnit.effects.Remove(this);
        targetUnit = null;
        sourceUnit = null;
    }


    float GetStatPower(Character unit) //source unit
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

    float GetStatResist(Character unit) //target unit
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

    
    public void InactiveTick() //on every other unit's action
    {
        switch(effectType)
        {
            case EffectType.DOT_AP:
            case EffectType.DOT_HP:
            case EffectType.DOT_MP:
            case EffectType.REGEN_AP:
            case EffectType.REGEN_HP:
            case EffectType.REGEN_MP:
                {
                    ApplyEffect();
                    break;
                }

            default:
                break;
        }
    }

    public void ActiveTick () //on target unit acting
    {
        ApplyEffect();
        duration -= 1;
        if(duration <= 0)
        {
            RemoveEffect();
        }
            
    }

    void ApplyEffect()
    {
        switch(effectType)
        {
            case EffectType.DAMAGE_AP:
            case EffectType.DOT_AP:
                {
                    Effect_DamageAP();
                    break;
                }
            case EffectType.DAMAGE_HP:
            case EffectType.DOT_HP:
                {
                    Effect_DamageHP();
                    break;
                }
            case EffectType.DAMAGE_MP:
            case EffectType.DOT_MP:
                {
                    Effect_DamageMP();
                    break;
                }
            case EffectType.ADD_AP:
            case EffectType.REGEN_AP:
                {
                    Effect_AddAP();
                    break;
                }
            case EffectType.ADD_HP:
            case EffectType.REGEN_HP:
                {
                    Effect_AddHP();
                    break;
                }
            case EffectType.ADD_MP:
            case EffectType.REGEN_MP:
                {
                    Effect_AddMP();
                    break;
                }
            case EffectType.FELL:
                {
                    targetUnit.sheet.stats.health = 0;
                    break;
                }
            case EffectType.BREAK:
                {
                    targetUnit.sheet.stats.morale = 0;
                    break;
                }
            case EffectType.INTERRUPT:
                {
                    targetUnit.sheet.stats.action = 0;
                    break;
                }
            default:
                {
                    Debug.Log(effectType.ToString() + " no effect on tick");
                    break;
                }
        }


        targetUnit.piece.UpdateUI();
    }

    float Effect_ModifyStatCalc() //caluclates the amount to modify a stat
    {

        float mod = 0;

        //base added
        mod = basePower*(sourceUnit.sheet.level+GameSettings.statPointsOnLevelUp);

        //roll about the base point
        mod *= UnityEngine.Random.Range(1f - GameSettings.ModifyStat_variance, 1f + GameSettings.ModifyStat_variance);

        //apply stat power
        mod *= (1 + (GameSettings.ModifyStat_PowerStatWeight * (sourcePowerStat + GameSettings.ModifyStat_RangeSpreadConstant) / 100f));

        //reduction for level scaling
        mod *= (1f / (1 + (GameSettings.ModifyStat_ScaleLevelWeight * targetUnit.sheet.level / 100f)));

        return mod;
    }

    void Effect_DamageAP()
    {
        float damage = 0;

        //base damage 
        damage = basePower;

        //damage roll about the base damage point
        damage *= UnityEngine.Random.Range(1f - GameSettings.DamageAP_variance, 1f + GameSettings.DamageAP_variance);

        //apply stat power
        damage *= (1 + (GameSettings.DamageAP_PowerStatWeight * (sourcePowerStat + GameSettings.DamageAP_RangeSpreadConstant) / 100f));

        //create and apply elemental damage multiplier


        //temporary critical hit check
        if(UnityEngine.Random.Range(0f, 1f) < 0.1f)
        {
            Debug.Log("Critical Attack!");
            damage *= 1.5f;
        }

        //resist reduction
        damage *= (1f / (1 + (GameSettings.DamageAP_ResistStatWeight * GetStatResist(targetUnit) / 100f)));
        damage *= (1f / (1 + (GameSettings.DamageAP_ResistLevelWeight * targetUnit.sheet.level / 100f)));

        //temporary anticrital defense check
        if(UnityEngine.Random.Range(0f, 1f) < 0.1f)
        {
            Debug.Log("Critical Defense!");
            damage /= 1.5f;
        }

        Debug.Log("damage taken: " + damage.ToString());

        targetUnit.piece.ShowNumber((int)damage, GameSettings.DamageAP_color);
        targetUnit.sheet.stats.action -= (int)damage;
    }

    void Effect_DamageHP()
    {
        float damage = 0;

        //base damage 
        damage = GameSettings.DamageHP_baseDamage + (GameSettings.DamageHP_addBasePerLevel * sourceUnit.sheet.level);
        if (sourceUnit.sheet.level > 1000)
        {
            damage *= sourceUnit.sheet.level / 1000f;
        }

        //damage roll about the base damage point
        damage *= UnityEngine.Random.Range(1f - GameSettings.DamageHP_variance, 1f + GameSettings.DamageHP_variance);
        Debug.Log("base damage : " + damage.ToString());

        //apply stat power
        damage *= (1 + (GameSettings.DamageHP_PowerStatWeight * (sourcePowerStat + GameSettings.DamageHP_RangeSpreadConstant) / 100f));

        //multiply effect base power 
        damage *= basePower;

        //temporary critical hit check
        if(UnityEngine.Random.Range(0f, 1f) < 0.1f)
        {
            Debug.Log("Critical Attack!");
            damage *= 1.5f;
        }

        Debug.Log("total damage dealt: " + damage.ToString());

        //resist reduction
        damage *= (1f / (1 + (GameSettings.DamageHP_ResistStatWeight * GetStatResist(targetUnit) / 100f)));
        damage *= (1f / (1 + (GameSettings.DamageHP_ResistLevelWeight * targetUnit.sheet.level / 100f)));

        //temporary anticritical defense check
        if(UnityEngine.Random.Range(0f, 1f) < 0.1f)
        {
            Debug.Log("Critical Defense!");
            damage /= 1.5f;
        }

        float shieldDamage =0f, armorDamage=0f, healthDamage = 0f;
        float shieldLeft=0f, armorLeft = 0f;

        shieldLeft = targetUnit.sheet.stats.shield / targetUnit.sheet.stats.maxShield;
        armorLeft = targetUnit.sheet.stats.armor / targetUnit.sheet.stats.maxArmor;

        if(damage > 0)
        {
            if(shieldLeft > 0f)
            {
                shieldDamage = damage * shieldLeft;
            }
            else
            {
                shieldDamage = 0f;
            }
            damage = damage - shieldDamage;
        }

        if(damage > 0)
        {
            if(armorLeft > 0f)
            {
                armorDamage = damage * armorLeft;
            }
            else
            {
                armorDamage = 0f;
            }
            damage = damage - armorDamage;

            armorDamage *= 0.33f;
        }

        if(damage > 0)
        {
            healthDamage = damage;
        }

        //create and apply elemental damage multiplier
        switch(elementalType)
        {
            default:
                break;
        }



        Debug.Log("damage taken HP: " + healthDamage.ToString() + "  Armor: " + armorDamage.ToString() + "  Shield: "+ shieldDamage.ToString());

        targetUnit.piece.ShowNumber((int)(healthDamage+armorDamage+shieldDamage),GameSettings.DamageHP_color);

        targetUnit.sheet.stats.health -= (int)healthDamage;
        targetUnit.sheet.stats.armor -= (int)armorDamage;
        targetUnit.sheet.stats.shield -= (int)shieldDamage;

        if(targetUnit.sheet.stats.health <= 0)
        {
            targetUnit.sheet.stats.health = 0;
            targetUnit.sheet.stats.shield = 0;
            targetUnit.piece.GetComponent<SpriteRenderer>().color *= 0.2f;
        }
    }

    void Effect_DamageMP()
    {
        float damage = 0;

        //base damage 
        damage = basePower;

        //damage roll about the base damage point
        damage *= UnityEngine.Random.Range(1f - GameSettings.DamageMP_variance, 1f + GameSettings.DamageMP_variance);

        //apply stat power
        damage *= (1 + (GameSettings.DamageMP_PowerStatWeight * (sourcePowerStat + GameSettings.DamageMP_RangeSpreadConstant) / 100f));

        //create and apply elemental damage multiplier
      
        //temporary critical hit check
        if(UnityEngine.Random.Range(0f, 1f) < 0.1f)
        {
            Debug.Log("Critical Attack!");
            damage *= 1.5f;
        }

        //resist reduction
        damage *= (1f / (1 + (GameSettings.DamageMP_ResistStatWeight * GetStatResist(targetUnit) / 100f)));
        damage *= (1f / (1 + (GameSettings.DamageMP_ResistLevelWeight * targetUnit.sheet.level / 100f)));

        //temporary anticrital defense check
        if(UnityEngine.Random.Range(0f, 1f) < 0.1f)
        {
            Debug.Log("Critical Defense!");
            damage /= 1.5f;
        }

        Debug.Log("damage taken: " + damage.ToString());

        targetUnit.piece.ShowNumber((int)damage, GameSettings.DamageMP_color);
        targetUnit.sheet.stats.morale -= (int)damage;
    }

    void Effect_AddAP()
    {
        float actionAdded = 0;

        //base ap added
        actionAdded = basePower;

        //roll about the base point
        actionAdded *= UnityEngine.Random.Range(1f - GameSettings.AddAP_variance, 1f + GameSettings.AddAP_variance);

        //apply stat power
        actionAdded *= (1 + (GameSettings.AddAP_PowerStatWeight * (sourcePowerStat + GameSettings.AddAP_RangeSpreadConstant) / 100f));

        //reduction for level scaling
        actionAdded *= (1f / (1 + (GameSettings.AddAP_ScaleLevelWeight * targetUnit.sheet.level / 100f)));

        targetUnit.piece.ShowNumber((int)actionAdded, GameSettings.AddAP_color);
        targetUnit.sheet.stats.action += (int)actionAdded;
    }

    void Effect_AddHP()
    {
        float heal = 0;

        //base heal 
        heal = GameSettings.AddHP_baseHeal+ (GameSettings.AddHP_addBasePerLevel * sourceUnit.sheet.level);

        //heal roll about the base point
        heal *= UnityEngine.Random.Range(1f - GameSettings.AddHP_variance, 1f + GameSettings.AddHP_variance);

        //apply stat power
        heal *= (1 + (GameSettings.AddHP_PowerStatWeight * (sourcePowerStat + GameSettings.AddHP_RangeSpreadConstant) / 100f));

        //multiply effect base power 
        heal *= basePower;

        //heal reduction for level scaling
        heal *= (1f / (1 + (GameSettings.AddHP_ScaleLevelWeight * targetUnit.sheet.level / 100f)));

        targetUnit.piece.ShowNumber((int)heal, GameSettings.AddHP_color);
        targetUnit.sheet.stats.health += (int)heal;
        if(targetUnit.sheet.stats.health > targetUnit.sheet.stats.maxHealth)
        {
            targetUnit.sheet.stats.health = targetUnit.sheet.stats.maxHealth;
        }
    }

    void Effect_AddMP()
    {
        float moraleAdded = 0;

        //base mp added
        moraleAdded = basePower;

        //roll about the base point
        moraleAdded *= UnityEngine.Random.Range(1f - GameSettings.AddMP_variance, 1f + GameSettings.AddMP_variance);

        //apply stat power
        moraleAdded *= (1 + (GameSettings.AddMP_PowerStatWeight * (sourcePowerStat + GameSettings.AddMP_RangeSpreadConstant) / 100f));

        //heal reduction for level scaling
        moraleAdded *= (1f / (1 + (GameSettings.AddMP_ScaleLevelWeight * targetUnit.sheet.level / 100f)));

        targetUnit.piece.ShowNumber((int)moraleAdded, GameSettings.AddMP_color);
        targetUnit.sheet.stats.morale += (int)moraleAdded;
    }

}
