using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


//this class is responsible for statistical representation of a game character where unit class is the graphical/animations
[Serializable]
public class Character
{
    //character values
    public string charName = "";
    public UnitClass unitClass = new UnitClass();
    public bool isElite;

    public int squadID;
    public int squadLocX,squadLocY;
    public bool isLeader;

    //attributes
    public Attributes stats = new Attributes();
    public Attributes statsMax = new Attributes();

    //derived from stats and class
    public float evadePhysical;
    public float evadeMagic;
    public float blockPhysical;
    public float blockMagic;
    public float absorbPhysical;
    public float absorbMagic;


    //combat
    public float actGuage;
    public List<Action> Actions;
    public int frontActionIndex;
    public int rearActionIndex;
    public int triggerActionIndex;


    public Ability Active;
    public Ability Passive;

    public Character()
    {
        unitClass.size = Size.Medium;
    }

    //speed formula: Add to act guage each combat tick
    public void BattleUpdate()
    {
        actGuage += (2f * Mathf.Sqrt(stats.Speed + 25f));
    }

}