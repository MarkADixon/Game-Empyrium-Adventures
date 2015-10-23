using UnityEngine;
using System.Collections;


//for graphics, sound settings etc store in player pref on application quit and load on application start
//no save game or game state data here!
public static class GameSettings
{
    public static bool isGameDataLoaded = false;

    public static int randomNumberResolution = 10000; //random is (0 to this-1)/this for a floating point 0 to less than 1 

    //level up
    public static int statPointsOnLevelUp = 10;
    public static int hitPointsMinOnLevelUp = 4;
    public static int hitPointsMaxOnLevelUp = 8;
    public static int LevelOneStartingStats_NumberOfLevelsFromZeros = 5;

    //to hit formula
    public static float phyHit_AttributeWeight = 4f;
    public static float phyHit_LevelWeight = 2f;
    public static float phyHit_RangeSpreadConstant = 50f; //higher extends the range

    //formula - Damage action
    //action damage base on basePower of effect
    public static float DamageAP_variance = 0.20f;
    public static float DamageAP_PowerStatWeight = 4f;
    public static float DamageAP_RangeSpreadConstant = 0f; //higher extends the range
    public static float DamageAP_ResistStatWeight = 0.8f; //vitality resist
    public static float DamageAP_ResistLevelWeight = 0.2f;
    public static float DamageAP_elementalMultiplier = 1.5f;
    public static Color DamageAP_color = new Color(1f,0.6f,0f);


    //formula - Damage health
    public static float DamageHP_baseDamage = 10f;
    public static float DamageHP_addBasePerLevel = 1f;
    public static float DamageHP_variance = 0.20f;
    public static float DamageHP_PowerStatWeight = 4f;
    public static float DamageHP_RangeSpreadConstant = 0f; //higher extends the range
    public static float DamageHP_ResistStatWeight = 0.8f; //vitality resist
    public static float DamageHP_ResistLevelWeight = 0.2f;
    public static float DamageHP_elementalMultiplier = 1.5f;
    public static Color DamageHP_color = Color.white;

    //formula - Damage morale
    //action damage base on basePower of effect
    public static float DamageMP_variance = 0.20f;
    public static float DamageMP_PowerStatWeight = 4f;
    public static float DamageMP_RangeSpreadConstant = 0f; //higher extends the range
    public static float DamageMP_ResistStatWeight = 0.8f; //vitality resist
    public static float DamageMP_ResistLevelWeight = 0.2f;
    public static float DamageMP_elementalMultiplier = 1.5f;
    public static Color DamageMP_color = new Color(1f, 0.4f, 1f);


    //add action
    //base actuon add is base power of ability
    public static float AddAP_variance = 0.20f;
    public static float AddAP_PowerStatWeight = 1f;
    public static float AddAP_RangeSpreadConstant = 0f; //higher extends the range
    public static float AddAP_ScaleLevelWeight = 1f; //inverse to heal amount (scales back down in place of resist)
    public static Color AddAP_color = Color.yellow;

    //add health
    public static float AddHP_baseHeal = 10f;
    public static float AddHP_addBasePerLevel = 1f;
    public static float AddHP_variance = 0.20f;
    public static float AddHP_PowerStatWeight = 4f;
    public static float AddHP_RangeSpreadConstant = 0f; //higher extends the range
    public static float AddHP_ScaleLevelWeight = 1f; //inverse to heal amount (scales back down in place of resist)
    public static Color AddHP_color = new Color(0.4f,1f,0.4f);

    // add morale
    //base morale add is base power of ability
    public static float AddMP_variance = 0.20f;
    public static float AddMP_PowerStatWeight = 1f;
    public static float AddMP_RangeSpreadConstant = 0f; //higher extends the range
    public static float AddMP_ScaleLevelWeight = 1f; //inverse to heal amount (scales back down in place of resist)
    public static Color AddMP_color = new Color (0.4f,0.4f,1f);

    //modify stat
    //base stat mod is base power of ability (range -1 to 1 as percent mod)
    public static float ModifyStat_variance = 0.20f;
    public static float ModifyStat_PowerStatWeight = 1f;
    public static float ModifyStat_RangeSpreadConstant = 0f; //higher extends the range
    public static float ModifyStat_ScaleLevelWeight = 1f; //inverse to amount (keeps scale at all levels)


}
