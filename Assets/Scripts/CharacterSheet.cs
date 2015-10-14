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
    public CharacterJob job = CharacterJob.None;
    public CharacterSize size = CharacterSize.Medium;
    public bool isElite = false;
    public bool isPlayer = false;

    public int squadID = -1;
    public int squadLocX = -1,squadLocY = -1;
    public bool isLeader = false;

    //attributes
    public CharacterStats stats = new CharacterStats();
    public CharacterStats statsMax = new CharacterStats();

    //derived from stats and class
    public float evadePhysical = 0f;
    public float evadeMagic = 0f;
    public float blockPhysical = 0f;
    public float blockMagic = 0f;
    public float absorbPhysical = 0f;
    public float absorbMagic = 0f;
    public float Movement = 0f;
    public float Alignment = 0f;


    //combat
    public float actGuage = 0f;
    //public Action FrontRowAction;
    //public Action BackRowAction;
    //public Action SpecialAction_A;
    //public Action SpecialAction_B;
    //public Ability Active;
    //public Ability Passive;
    //public List<Action> knownActions = new List<Action>();
}


[Serializable]
public enum CharacterJob
{
    None, Ashigaru, Engineer, Gunner, Junker, Knight, Magi, Pirate, Scout, Valkyrie, Wark, Witch
}

[Serializable]
public enum CharacterSize
{
    None = 0, Small = 1, Medium = 2, Large = 3, Huge = 4
}

[Serializable]
public enum CharacterStat
{
    HitPoints, Moralle, //physical and mental HP
    Strength, Vitality, //physical power and defense "Might"
    Magic, Will, //Mental power and defense "Mind"
    Charisma, Grit, //Moralle power and defense
    Perception, //affects all phy,mag,moralle accruacies based on comparison between characters
    Speed, //speed at which character acts
    Luck, //chance to reroll and keep better result
}

[Serializable]
public class CharacterStats
{
    public int HitPoints, Moralle;
    public int Strength, Vitality;
    public int Magic, Spirit;
    public int Charisma, Grit;
    public int Perception, Speed, Luck; 
    public CharacterStats()
    {
        HitPoints = 100;
        Moralle = 100;
        Strength = 0;
        Vitality = 0;
        Magic = 0;
        Will = 0;
        Charisma = 0;
        Grit = 0;
        Speed = 0;
        Charisma = 0;
        Luck = 0;
    }


    public int GetStat(CharacterStat statWanted)
    {
        int statValue;

        switch(statWanted)
        {
            case (CharacterStat.HitPoints):
                {

                    break;
                }
            case (CharacterStat.Strength):
                {

                    break;
                }
            case (CharacterStat.Vitality):
                {

                    break;
                }
            case (CharacterStat.Agility):
                {

                    break;
                }
            case (CharacterStat.Mind):
                {

                    break;
                }
            case (CharacterStat.Willpower):
                {

                    break;
                }
            case (CharacterStat.Intelligence):
                {

                    break;
                }
            case (CharacterStat.Speed):
                {

                    break;
                }
            case (CharacterStat.Charisma):
                {

                    break;
                }
            case (CharacterStat.Luck):
                {

                    break;
                }
            case (CharacterStat.Movement):
                {

                    break;
                }
            case (CharacterStat.Moralle):
                {

                    break;
                }
            case (CharacterStat.Alignment):
                {

                    break;
                }
            default:
                {
                    break;
                }
        }
        return statValue;
    }
}



