using UnityEngine;
using System.Collections;


//for graphics, sound settings etc store in player pref on application quit and load on application start
//no save game or game state data here!
public static class GameSettings
{
    public static bool isGameDataLoaded;

    public static int randomNumberResolution = 10000; //random is (0 to this-1)/this for a floating point 0 to less than 1 

    //level up
    public static int statPointsOnLevelUp = 10;
    public static int hitPointsMinOnLevelUp = 4;
    public static int hitPointsMaxOnLevelUp = 8;
    public static int LevelOneStartingStats_NumberOfLevelsFromZeros = 10;

    //to hit formula
    public static float phyHit_AttributeWeight = 4f;
    public static float phyHit_LevelWeight = 2f;
    public static float phyHit_RangeSpreadConstant = 50f; //higher extends the range

    //formula - Damage Physical Melee
    public static float phyDamage_baseDamage = 10f;
    public static float phyDamage_additionalBaseDamagePerLevel = 1f;
    public static float phyDamage_AttributeWeight = 4f;
    public static float phyDamage_RangeSpreadConstant = 0f; //higher extends the range
    public static float phyDamage_variance = 0.25f;
    public static float phyDamageResist_AttributeWeight = 1f; //vitality resist
    public static float phyDamageResist_LevelWeight = 0.2f;
    public static float elementalStrengthMultiplier = 1.5f;

}
