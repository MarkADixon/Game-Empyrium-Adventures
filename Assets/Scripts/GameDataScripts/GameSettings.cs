using UnityEngine;
using System.Collections;


//for graphics, sound settings etc store in player pref on application quit and load on application start
//no save game or game state data here!
public static class GameSettings
{
    public static bool isGameDataLoaded;

    //formula - To Hit Physical
    public static float PhyHit_baseToHitChance = 0.5f;
    public static float PhyHit_AttributeWeight = 4f;
    public static float PhyHit_LevelWeight = 2f;
    public static float PhyHit_RangeSpreadConstant = 50f; //higher extends the range

    //formula - Damage Physical Melee
    public static float PhyDamage_baseDamage = 5f;
    public static float PhyDamage_AttributeWeight = 4f;
    public static float PhyDamage_LevelWeight = 2f;
    public static float PhyDamage_RangeSpreadConstant = 0f; //higher extends the range

}
