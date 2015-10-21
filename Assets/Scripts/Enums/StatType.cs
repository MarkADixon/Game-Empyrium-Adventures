using System;

[Serializable]
public enum StatType
{
    NONE,
    HEALTH, MORALE, ACTION,
    ATTACK, DEFENSE, AGILITY, //Strength,Toughness,Agility
    MAGIC, SPIRIT, MIND,         //Magic,Spirit,Mind
    CHARISMA, RESOLVE,       //Charisma,Will
    SPEED, LUCK,
    MOVE, ALIGNMENT
}
