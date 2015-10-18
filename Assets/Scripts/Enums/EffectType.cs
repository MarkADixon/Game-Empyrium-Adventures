using System;

[Serializable]
//list of all the effects of actions, including buffs/debuffs that have duration
public enum EffectType
{
    DAMAGE, //remove health (HP)
    HEAL, //add health (HP)
    DOT, //remove health (HP) each tick
    REGEN, //add health (HP) each tick
    INJURED, //decrease health (HP) gains by %
    VITALITY, //increase health (HP) gains by %
    INVULNERABLE_HEALTH, //decrease health (HP) loss by %
    VULNERABLE_HEALTH, //increase health (HP) loss by %
    FELL, //target health (HP) to 0 
    FALLEN, //while health (HP) is 0

    DEMORALIZE, //remove morale (MP)
    INSPIRE, //add morale (MP)
    , //remove morale (MP) each tick
    , //add morale (MP) each tick
    , //decrease morale (MP) gains by %
    , //increase morale (MP) gains by %
    INVULNERABLE_MORALE, //decrease morale (MP) loss by %
    VULNERABLE_MORALE, //increase morale (MP) loss by %
    BREAK, //target morale (MP) to 0
    INSPIRED, //while morale (MP) is above 100
    DEMORALIZED, //while morale (MP) is below 0

    HINDER, //remove action (AP)
    ASSIST, //add action (AP)
    MIRE, //remove action (AP) each tick
    DRIVE, //add action (AP) each tick
    SLOW, //decrease action (AP) gains by %
    HASTE, //increase action (AP) gains by %
    INTERRUPT, //target AP to 0
    EXHAUSTED, //while action (AP) is below 0

    INVULNERABLE_PHYSICAL, //decrease potency of physical effects by %
    VULNERABLE_PHYSICAL, //increase potency of physical effects by %
    INVULNERABLE_MAGICAL, //decrease potency of magical effects by %
    VULNERABLE_MAGICAL, //increase potency of magical effects by %
    INVULNERABLE_FIRE, //decrease potency of fire elemental effects by %
    VULNERABLE_FIRE, //increase potency of fire elemental effects by %
    INVULNERABLE_WATER, //decrease potency of water elemental effects by %
    VULNERABLE_WATER, //increase potency of water elemental effects by %
    INVULNERABLE_EARTH, //decrease potency of earth elemental effects by %
    VULNERABLE_EARTH, //increase potency of earth elemental effects by %
    INVULNERABLE_AIR, //decrease potency of air elemental effects by %
    VULNERABLE_AIR, //increase potency of air elemental effects by %
    INVULNERABLE_LIGHT, //decrease potency of light elemental effects by %
    VULNERABLE_LIGHT, //increase potency of light elemental effects by %
    INVULNERABLE_SHADOW, //decrease potency of shadow elemental effects by %
    VULNERABLE_SHADOW, //increase potency of shadow elemental effects by %

    INCREASE_STRENGTH, //increase STRENGTH attribute
    DECREASE_STRENGTH, //decrease STRENGTH attribute
    INCREASE_TOUGHNESS, //increase TOUGHNESS attribute
    DECREASE_TOUGHNESS, //decrease TOUGHNESS attribute
    INCREASE_AGILITY, //increase AGILITY attribute
    DECREASE_AGILITY, //decrease AGILITY attribute
    INCREASE_MAGIC, //increase MAGIC attribute
    DECREASE_MAGIC, //decrease MAGIC attribute
    INCREASE_SPIRIT, //increase SPIRIT attribute
    DECREASE_SPIRIT, //decrease SPIRIT attribute
    INCREASE_MIND, //increase MIND attribute
    DECREASE_MIND, //decrease MIND attribute
    INCREASE_CHARISMA, //increase CHARISMA attribute
    DECREASE_CHARISMA, //decrease CHARISMA attribute
    INCREASE_RESOLVE, //increase RESOLVE attribute
    DECREASE_RESOLVE, //decrease RESOLVE attribute
    INCREASE_SPEED, //increase SPEED attribute
    DECREASE_SPEED, //decrease SPEED attribute
    INCREASE_LUCK, //increase LUCK attribute
    DECREASE_LUCK, //decrease LUCK attribute
}

