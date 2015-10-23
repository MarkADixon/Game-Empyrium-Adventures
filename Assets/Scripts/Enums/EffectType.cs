using System;

[Serializable]
//list of all the effects of actions, including buffs/debuffs that have duration
public enum EffectType
{
    DAMAGE_HP, //remove health (HP)
    ADD_HP, //add health (HP) (ex: heal)
    DOT_HP, //remove health (HP) each tick
    REGEN_HP, //add health (HP) each tick
    DAMPEN_HP, //decrease health (HP) gains by %
    AMPLIFY_HP, //increase health (HP) gains by %
    INVULNERABLE_HEALTH, //decrease health (HP) loss by %
    VULNERABLE_HEALTH, //increase health (HP) loss by %
    FELL, //target health (HP) to 0 
    FALLEN, //while health (HP) is 0

    DAMAGE_MP, //remove morale (MP) (ex: Demoralize)
    ADD_MP, //add morale (MP) (ex: inspire)
    DOT_MP, //remove morale (MP) each tick
    REGEN_MP, //add morale (MP) each tick
    DAMPEN_MP, //decrease morale (MP) gains by %
    AMPLIFY_MP, //increase morale (MP) gains by %
    INVULNERABLE_MORALE, //decrease morale (MP) loss by %
    VULNERABLE_MORALE, //increase morale (MP) loss by %
    BREAK, //target morale (MP) to 0
    INSPIRED, //while morale (MP) is above 100
    DEMORALIZED, //while morale (MP) is below 0

    DAMAGE_AP, //remove action (AP) (ex:Hinder)
    ADD_AP, //add action (AP) (ex: assist)
    DOT_AP, //remove action (AP) each tick (ex: mire)
    REGEN_AP, //add action (AP) each tick (ex:drive)
    DAMPEN_AP, //decrease action (AP) gains by % (ex: slow)
    AMPLIFY_AP, //increase action (AP) gains by % (ex: haste)
    INVULNERABLE_ACTION, //decrease action (AP) loss by %
    VULNERABLE_ACTION, //increase action (AP) loss by %
    INTERRUPT, //target AP to 0 (ex:interrupt)
    EXHAUSTED, //while action (AP) is below 0

    INVULNERABLE_PHYSICAL, //decrease physical effects by % when targeted
    VULNERABLE_PHYSICAL, //increase physical effects by % when targeted
    INVULNERABLE_MAGICAL, //decrease magical effects by % when targeted
    VULNERABLE_MAGICAL, //increase magical effects by % when targeted
    INVULNERABLE_FIRE, //decrease fire elemental effects by % when targeted
    VULNERABLE_FIRE, //increase fire elemental effects by % when targeted
    INVULNERABLE_WATER, //decrease water elemental effects by % when targeted
    VULNERABLE_WATER, //increase water elemental effects by % when targeted
    INVULNERABLE_EARTH, //decrease earth elemental effects by % when targeted
    VULNERABLE_EARTH, //increase earth elemental effects by % when targeted
    INVULNERABLE_AIR, //decrease air elemental effects by % when targeted
    VULNERABLE_AIR, //increase air elemental effects by % when targeted
    INVULNERABLE_LIGHT, //decrease light elemental effects by % when targeted
    VULNERABLE_LIGHT, //increase light elemental effects by % when targeted
    INVULNERABLE_SHADOW, //decrease shadow elemental effects by % when targeted
    VULNERABLE_SHADOW, //increase shadow elemental effects by % when targeted

    DAMPEN_PHYSICAL, //decrease physical effects by % when acting
    AMPLIFY_PHYSICAL, //increase physical effects by % when acting
    DAMPEN_MAGICAL, //decrease magical effects by % when acting
    AMPLIFY_MAGICAL, //increase magical effects by % when acting
    DAMPEN_FIRE, //decrease fire elemental effects by % when acting
    AMPLIFY_FIRE, //increase fire elemental effects by % when acting
    DAMPEN_WATER, //decrease water elemental effects by % when acting
    AMPLIFY_WATER, //increase water elemental effects by % when acting
    DAMPEN_EARTH, //decrease earth elemental effects by % when acting
    AMPLIFY_EARTH, //increase earth elemental effects by % when acting
    DAMPEN_AIR, //decrease air elemental effects by % when acting
    AMPLIFY_AIR, //increase air elemental effects by % when acting
    DAMPEN_LIGHT, //decrease light elemental effects by % when acting
    AMPLIFY_LIGHT, //increase light elemental effects by % when acting
    DAMPEN_SHADOW, //decrease shadow elemental effects by % when acting
    AMPLIFY_SHADOW, //increase shadow elemental effects by % when acting

    MODIFY_MAXHP,
    MODIFY_ATTACK, 
    MODIFY_DEFENSE, 
    MODIFY_AGILITY,
    MODIFY_SPIRIT, 
    MODIFY_MIND,
    MODIFY_CHARISMA, 
    MODIFY_RESOLVE, 
    MODIFY_SPEED,
    MODIFY_LUCK, 
}

