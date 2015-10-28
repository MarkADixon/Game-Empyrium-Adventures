using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//this data class is responsible for statistical representation of a game character
[Serializable]
public class CharacterSheet
{
    //character values
    public string characterName = "No Name";
    public string characterClass = "Junker";
    public int level = 1;
    public int experience = 0;

    public SizeType size = SizeType.NORMAL;
    public bool isElite = false;
    public bool isPlayer = false;

    public int squadID = -1;
    public int squadLocX = -1,squadLocY = -1;
    public bool isLeader = false;

    //attributes
    public CharacterStats stats = new CharacterStats();
    public CharacterStats statsRaw = new CharacterStats();

    //combat
    public Action action_Next;
    public List<Action> actions_FrontRow;
    public List<Action> actions_BackRow;
    public List<Action> actions_Activated;

    //public List<Action> knownActions = new List<Action>();
}




[Serializable]
public class CharacterStats
{
    public int health, maxHealth;
    public float armor, maxArmor;
    public float shield, maxShield;
    public float morale, action;
    public int attack, defense, agility;
    public int magic, spirit, mind;
    public int charisma, resolve;
    public int speed,luck;
    public int move;
    public float alignment; 
    public CharacterStats()
    {
        health = 1;
        morale = 100f;
        action = 100f;
        attack = 1;
        defense = 1;
        agility = 1;
        magic = 1;
        spirit = 1;
        mind = 1;
        charisma = 1;
        resolve = 1;
        speed = 1;
        luck = 1;
        move = 1;
        alignment = 0;
    }


}



