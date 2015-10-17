using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[Serializable]
public class CharacterClass {

    public string name = "";
    public SizeType sizeType = SizeType.NONE;
    public List<Action> forwardActions = new List<Action>();
    public List<Action> rearActions = new List<Action>();
    public List<Action> activatedActions = new List<Action>();
    public string leaderBonus = ""; //change string to bonus datatype later
    public List<string> innateBonuses = new List<string>(); //change string to bonus datatype later
    public float alignment = 0;
    public List<OddsToStat> levelUpStatOdds = new List<OddsToStat>();


    public CharacterClass(Dictionary<string,string> classData)
    {
        OddsToStat newOdds = new OddsToStat(0f,StatType.MOVE);
        char delimiter = char.Parse("_");
        string dataKey;
        string[] dataKeySplit;

        name = classData["Name"];
        sizeType = (SizeType)System.Enum.Parse(typeof(SizeType), classData["SizeType"].ToString());
        leaderBonus = classData["LeaderBonus"];
        alignment = float.Parse(classData["Alignment"]);

        foreach(KeyValuePair<string, string> item in classData)
        {
            dataKey = item.Key;
            dataKeySplit = dataKey.Split(delimiter);
            dataKey = dataKeySplit[0];
            switch(dataKey)
            {
                case "ForwardAction":
                    {
                        forwardActions.Add(DataManager.DM.actionDictionary[item.Value]);
                        break;
                    }
                case "RearAction":
                    {
                        rearActions.Add(DataManager.DM.actionDictionary[item.Value]);
                        break;
                    }
                case "ActivatedAction":
                    {
                        activatedActions.Add(DataManager.DM.actionDictionary[item.Value]);
                        break;
                    }
                case "InnateBonus":
                    {
                        innateBonuses.Add(item.Value);
                        break;
                    }
                case "STR":
                    {
                        newOdds = new OddsToStat(float.Parse(item.Value),StatType.STRENGTH);
                        levelUpStatOdds.Add(newOdds);  
                        break;
                    }
                case "TGH":
                    {
                        newOdds = new OddsToStat(float.Parse(item.Value), StatType.TOUGHNESS);
                        levelUpStatOdds.Add(newOdds);
                        break;
                    }
                case "AGI":
                    {
                        newOdds = new OddsToStat(float.Parse(item.Value), StatType.AGILITY);
                        levelUpStatOdds.Add(newOdds);
                        break;
                    }
                case "MAG":
                    {
                        newOdds = new OddsToStat(float.Parse(item.Value), StatType.MAGIC);
                        levelUpStatOdds.Add(newOdds);
                        break;
                    }
                case "SPI":
                    {
                        newOdds = new OddsToStat(float.Parse(item.Value), StatType.SPIRIT);
                        levelUpStatOdds.Add(newOdds);
                        break;
                    }
                case "MND":
                    {
                        newOdds = new OddsToStat(float.Parse(item.Value), StatType.MIND);
                        levelUpStatOdds.Add(newOdds);
                        break;
                    }
                case "CHA":
                    {
                        newOdds = new OddsToStat(float.Parse(item.Value), StatType.CHARISMA);
                        levelUpStatOdds.Add(newOdds);
                        break;
                    }
                case "RES":
                    {
                        newOdds = new OddsToStat(float.Parse(item.Value), StatType.RESOLVE);
                        levelUpStatOdds.Add(newOdds);
                        break;
                    }
                case "SPD":
                    {
                        newOdds = new OddsToStat(float.Parse(item.Value), StatType.SPEED);
                        levelUpStatOdds.Add(newOdds);
                        break;
                    }
                case "LUK":
                    {
                        newOdds = new OddsToStat(float.Parse(item.Value), StatType.LUCK);
                        levelUpStatOdds.Add(newOdds);
                        break;
                    }
                default:
                    break;
            } //endswitch
        }//ench for each in dictionary

        //initialize odds list
        float total = 0f;
        foreach(OddsToStat odds in levelUpStatOdds)
        {
            total += odds.initialValue;
        }
        float prevRangeMax = 0f;
        
        foreach(OddsToStat odds in levelUpStatOdds)
        {
            odds.InitializeRange(total, prevRangeMax);
            prevRangeMax = odds.range.y;
        }
    }



}

public class OddsToStat
{
    public float initialValue;
    public Vector2 range = new Vector2(0f, 1f);
    public StatType stat = StatType.STRENGTH;
    public OddsToStat(float _initialValue, StatType _stat)
    {
        initialValue = _initialValue;
        stat = _stat;
    }
    public void InitializeRange(float totalValueInTable, float previousRangeMax)
    {
        range = new Vector2(previousRangeMax, initialValue / totalValueInTable + previousRangeMax);
    }
}

