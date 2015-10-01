using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public enum CharacterClass
{
    None, Ashigaru, Engineer, Gunner, Junker, Knight, Magi, Pirate, Scout, Valkyrie, Wark, Witch
}

[Serializable]
public enum Size
{
    None = 0, Small = 1, Medium = 2, Large = 3, Huge = 4
}

[Serializable]
public class Attributes
{
    public int HitPoints, Strength, Vitality, Agility, Intelligence, Willpower, Mind, Speed, Charisma, Luck, Movement, Moralle, Alignment;
    public Attributes()
    {
        HitPoints = 0; Strength = 0; Vitality = 0; Agility = 0;
        Intelligence = 0; Willpower = 0; Mind = 0; Speed = 0;
        Charisma = 0; Luck = 0;
        Movement = 0; Moralle = 0; Alignment = 0;
    }
}


//this class is responsible for statistical representation of a game character where unit class is the graphical/animations
[Serializable]
public class Character
{
    //character values
    public string characterName = "No Name";
    public CharacterClass characterClass = CharacterClass.None;
    public Size size = Size.Medium;
    public bool isElite = false;
    public bool isPlayer = false;

    public int squadID = -1;
    public int squadLocX = -1,squadLocY = -1;
    public bool isLeader = false;

    //attributes
    public Attributes stats = new Attributes();
    public Attributes statsMax = new Attributes();

    //derived from stats and class
    public float evadePhysical = 0f;
    public float evadeMagic = 0f;
    public float blockPhysical = 0f;
    public float blockMagic = 0f;
    public float absorbPhysical = 0f;
    public float absorbMagic = 0f;


    //combat
    public float actGuage = 0f;
    //public Action FrontRowAction;
    //public Action BackRowAction;
    //public Action SpecialAction_A;
    //public Action SpecialAction_B;
    //public Ability Active;
    //public Ability Passive;
    //public List<Action> knownActions = new List<Action>();

    public Character()
    {
        
    }

    

}