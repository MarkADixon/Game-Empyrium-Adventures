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
    public int level = 1;
    public int experience = 0;
    public CharacterType characterType = CharacterType.JUNKER;
    public SizeType size = SizeType.MEDIUM;
    public bool isElite = false;
    public bool isPlayer = false;

    public int squadID = -1;
    public int squadLocX = -1,squadLocY = -1;
    public bool isLeader = false;

    //attributes
    public CharacterStats stats = new CharacterStats();
    public CharacterStats statsMax = new CharacterStats();

    //combat
    public float actGuage = 0f;
    public Action action_Next;
    public ActionType action_FrontRow;
    public ActionType action_BackRow;
    //public Action SpecialAction_A;
    //public Action SpecialAction_B;
    //public Ability Active;
    //public Ability Passive;
    //public List<Action> knownActions = new List<Action>();
}




[Serializable]
public class CharacterStats
{
    public int hitPoints, moralePoints;
    public int strength, toughness, agility;
    public int magic, spirit, mind;
    public int charisma, resolve, wit;
    public int speed, luck;
    public int move, alignment; 
    public CharacterStats()
    {
        hitPoints = 100;
        moralePoints = 100;
        strength = 0;
        toughness = 0;
        agility = 0;
        magic = 0;
        spirit = 0;
        mind = 0;
        charisma = 0;
        resolve = 0;
        wit = 0;
        speed = 0;
        luck = 0;
        move = 0;
        alignment = 0;
    }


}



