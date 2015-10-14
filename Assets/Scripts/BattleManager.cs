using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BattleManager : MonoBehaviour {

    public AudioClip battleStart;
    float ellapsedTime = 0f;

    //incoming data to instantate battle from
    Squad playerSquad;
    Squad enemySquad;

    //for location calculations on screen placement
    Vector3 playerSquadOffset = new Vector3(3f,-3.75f,0);
    Vector3 sizeOffset, spawnLocation;
    float scaleDistBetweenUnits = 0.75f;
 
    List<Character> unitsInBattle = new List<Character>();
    List<Character> unitActOrder = new List<Character>();

    bool isUnitActing = false;
    Character actingUnit;
    List<Character> targetedUnits = new List<Character>();
    List<Character> selectUnits = new List<Character>();

    //game object containers for squad graphics objects
    GameObject playerBattleGroup;
    GameObject enemyBattleGroup;

    //for battle calculations
    float unitsLeftInBattle = 10f; //experiement with scaling up speed as units die
    float maxAct = 0f;
    int index = 0;
    float damage = 0;
    float roll = 0;
    float toHitChance = 0;
    TargetType targetType = TargetType.EnemySingleHighestHP;

    //make BM a singleton
    public static BattleManager instance = null;
    
    //initialize here
    void Awake()
    {
        //make BM a singleton
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        playerBattleGroup = new GameObject();
        enemyBattleGroup = new GameObject();
        
    }

    void Start()
    {
        
    }

    public void StartNewBattle(Squad _playerSquad, Squad _enemySquad)
    {
        playerSquad = _playerSquad;
        enemySquad = _enemySquad;

        ellapsedTime = 0f;

        SoundManager.SM.PlaySfx(battleStart);

        CreateBattleGroup(playerSquad,playerBattleGroup);
        CreateBattleGroup(enemySquad,enemyBattleGroup);

        RollInitative();

        CalculateActOrder();
    }



    // Update is called once per frame
    void Update ()
    {            
        ellapsedTime += Time.deltaTime;
 
        AdvanceACTGuages();

        SetActingUnit();

        if(isUnitActing)
        {
            
        
            DetermineTargets();

            Debug.Log(actingUnit.sheet.characterName + " is attacking " +targetedUnits[0].sheet.characterName);

            RollToHit();

            if(roll < toHitChance)
            {
                RollDamage();


                ApplyDamage();
            }
            
            CalculateActOrder();

        }

    }


    public void CreateBattleGroup(Squad _squad, GameObject battleGroup)
    {
        battleGroup.name = _squad.isPlayer ? "playerSquad" : "enemySquad";
        battleGroup.transform.position = Vector3.zero;

        for(int i = 0; i < _squad.members.Count; i++)
        {
            //parent the squad members to battlegroup
            _squad.members[i].piece.transform.parent = battleGroup.transform;

            //calculate position
            sizeOffset = ((int)_squad.members[i].sheet.size - 1) * Vector3.one *0.5f * scaleDistBetweenUnits;
            spawnLocation = new Vector3(_squad.members[i].sheet.squadLocX, _squad.members[i].sheet.squadLocY, _squad.members[i].sheet.squadLocY) * scaleDistBetweenUnits;
            _squad.members[i].piece.transform.localPosition = spawnLocation + playerSquadOffset + sizeOffset;
            
            //add reference of all units ot battle manager
            unitsInBattle.Add(_squad.members[i]);        
        }

        // invert enemy squad facing
        if(!_squad.isPlayer)
            battleGroup.transform.localScale = new Vector3(battleGroup.transform.localScale.x * -1, battleGroup.transform.localScale.y, battleGroup.transform.localScale.z);
    }

    //initial actguage values from 0 to 75 (100 to act)
    void RollInitative()
    {
        for(int i = 0; i < unitsInBattle.Count; i++)
        {
            unitsInBattle[i].sheet.actGuage = Random.Range(0f, 75f);
        }
    }

    //speed formula: Add to act guage each combat tick
    void AdvanceACTGuages()
    {
        unitsLeftInBattle = 0;
        for(int i = 0; i < unitsInBattle.Count; i++)
        {
            //conditions: unit hp>0, 
            if(unitsInBattle[i].sheet.stats.HitPoints > 0)
            {

                unitsLeftInBattle += 1;
            }
        }
        

        for(int i = 0; i < unitsInBattle.Count; i++)
        {
            //conditions: unit hp>0, 
            if(unitsInBattle[i].sheet.stats.HitPoints > 0)
            {
                
                unitsInBattle[i].sheet.actGuage += (10f/unitsLeftInBattle) * Time.deltaTime * (2.5f * Mathf.Sqrt(unitsInBattle[i].sheet.stats.Speed + 25f));
            }
        }
    }

    //determine which unit will be acting this frame (max ACT) if over 100 ACT and flag the frame for a unit acting and set reference to acting unit
    void SetActingUnit()
    {
        isUnitActing = false;

        //get max value and index
        maxAct = 0;
        index = 0;
        for(int i = 0; i < unitsInBattle.Count; i++)
        {
            if(unitsInBattle[i].sheet.actGuage > maxAct)
            {
                maxAct = unitsInBattle[i].sheet.actGuage;
                index = i;
            }
        }

        //if the max is over the act value of 100, subtract 100 and flag to act
        if(maxAct > 100f)
        {
            isUnitActing = true;
            actingUnit = unitsInBattle[index];
            unitsInBattle[index].sheet.actGuage -= 100;
            unitsInBattle[index].piece.Animate_Attack();
        }
    }

    //dertemine targets of the action and store in targetedUnits[]
    void DetermineTargets()
    {
        //TODO check action targeting 

        //TODO check tactics

        //reset targetunits array 
        targetedUnits = new List<Character>(unitsInBattle);
    
        switch (targetType)
        {
            case TargetType.Self:
                {
                    targetedUnits.Clear();
                    targetedUnits.Add(actingUnit);
                    break;
                }
            case TargetType.EnemySingleMelee:  //taget nearest with preference to same horizontal
                {
                    Target_RemoveAllies();
                    Target_RemoveFallen();
                    Target_NearestWithLinePreference();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.EnemySingleRanged: //target nearest in backmost row
                {
                    Target_BackRow(); //no need to remove allies/fallen as it polls squad directly
                    Target_Nearest();
                    break;
                }
            case TargetType.EnemySingleRandom:
                {
                    Target_RemoveAllies();
                    Target_RemoveFallen();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.EnemySingleLowestHP:
                {
                    Target_RemoveAllies();
                    Target_RemoveFallen();
                    Target_LowestHP();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.EnemySingleHighestHP:
                {
                    Target_RemoveAllies();
                    Target_RemoveFallen();
                    Target_HighestHP();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.AllySingleFallen:
                {
                    Target_RemoveEnemies();
                    Target_RemoveUnfallen();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.AllySingleLowestHP:
                {
                    Target_RemoveEnemies();
                    Target_RemoveFallen();
                    Target_LowestHP();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.AllySingleHighestHP:
                {
                    Target_RemoveEnemies();
                    Target_RemoveFallen();
                    Target_HighestHP();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.AllySingleRandom:
                {
                    Target_RemoveEnemies();
                    Target_RemoveFallen();
                    Target_RandomSelectOne();
                    break;
                }
            default:
                {                   
                    break;
                }
        }
    }

    void Target_NearestWithLinePreference()
    {
        float nearest = float.MaxValue;
        float distance;
        float dist_x;
        float dist_y;
        selectUnits.Clear();
        //get closest
        for(int i = 0; i < targetedUnits.Count; i++)
        {
            dist_x = (actingUnit.piece.transform.position.x - targetedUnits[i].piece.transform.position.x);
            dist_y = (actingUnit.piece.transform.position.y - targetedUnits[i].piece.transform.position.y)*6f; //the times six makes the line preference

            distance = Mathf.Sqrt(Mathf.Pow(dist_x,2f)+Mathf.Pow(dist_y,2f));
            if(Mathf.Approximately(distance, nearest))
            {
                selectUnits.Add(targetedUnits[i]);
            }
            else if(distance < nearest)
            {
                selectUnits.Clear();
                nearest = distance;
                selectUnits.Add(targetedUnits[i]);
            }
        }
        //make nearest targeted
        targetedUnits = new List<Character>(selectUnits);
    }

    void Target_Nearest()
    {
        float nearest = float.MaxValue;
        float distance;
        selectUnits.Clear();
        //get closest
        for(int i = 0; i < targetedUnits.Count; i++)
        {
            distance = Vector2.Distance(actingUnit.piece.transform.position, targetedUnits[i].piece.transform.position);
            if(Mathf.Approximately(distance, nearest))
            {
                selectUnits.Add(targetedUnits[i]);
            }
            else if(distance < nearest)
            {
                selectUnits.Clear();
                nearest = distance;
                selectUnits.Add(targetedUnits[i]);
            }
        }
        //make nearest targeted
        targetedUnits = new List<Character>(selectUnits);
    }

    void Target_RandomSelectOne()
    {
        //select one at random of remaining
        if(targetedUnits.Count > 1)
        {
            Character temp = targetedUnits[Random.Range(0, targetedUnits.Count)];
            targetedUnits.Clear();
            targetedUnits.Add(temp);
        }
    }

    void Target_BackRow()
    {
        //find backmost row in opposite formation
        int rowWithUnits = -1;
        for(int row = 5; row >= 0; row--)
        {
            for (int place = 0; place<6;place++)
            {
                if (actingUnit.sheet.isPlayer && rowWithUnits == -1)
                {
                    if(enemySquad.formation[row,place] >= 0) rowWithUnits = row;
                }
                else if(!actingUnit.sheet.isPlayer && rowWithUnits == -1)
                {
                    if(playerSquad.formation[row, place] >= 0) rowWithUnits = row;
                }
            }
        }

        Target_SelectedRow(rowWithUnits);
    }

    void Target_SelectedRow(int row)
    {
        //filter for selected row
        selectUnits.Clear();

        if(actingUnit.sheet.isPlayer)
        {
            selectUnits = enemySquad.GetCharactersInRow(row);
        }
        else if(!actingUnit.sheet.isPlayer)
        {
            selectUnits = playerSquad.GetCharactersInRow(row);
        }
        
        //make targeted
        targetedUnits = new List<Character>(selectUnits);
    }

    void Target_RemoveAllies()
    {
        //remove allies
        for(int i = targetedUnits.Count - 1; i >= 0; i--)
        {
            if(targetedUnits[i].sheet.isPlayer == actingUnit.sheet.isPlayer)
            {
                targetedUnits.RemoveAt(i);
            }
        }
    }

    void Target_RemoveEnemies()
    {
        //remove enemies
        for(int i = targetedUnits.Count - 1; i >= 0; i--)
        {
            if(targetedUnits[i].sheet.isPlayer != actingUnit.sheet.isPlayer)
            {
                targetedUnits.RemoveAt(i);
            }
        }
    }

    void Target_RemoveFallen()
    {
        //remove fallen
        for(int i = targetedUnits.Count - 1; i >= 0; i--)
        {
            if(targetedUnits[i].sheet.stats.HitPoints <= 0)
            {
                targetedUnits.RemoveAt(i);
            }
        }
    }

    void Target_RemoveUnfallen()
    {
        //remove unfallen
        for(int i = targetedUnits.Count - 1; i >= 0; i--)
        {
            if(targetedUnits[i].sheet.stats.HitPoints > 0)
            {
                targetedUnits.RemoveAt(i);
            }
        }
    }

    void Target_LowestHP()
    {
        int lowest = int.MaxValue;
        selectUnits.Clear();
        //get closest
        for(int i = 0; i < targetedUnits.Count; i++)
        {   
            if(targetedUnits[i].sheet.stats.HitPoints == lowest)
            {
                selectUnits.Add(targetedUnits[i]);
            }
            else if(targetedUnits[i].sheet.stats.HitPoints < lowest)
            {
                selectUnits.Clear();
                lowest = targetedUnits[i].sheet.stats.HitPoints;
                selectUnits.Add(targetedUnits[i]);
            }
        }
        //make lowest HP targeted
        targetedUnits = new List<Character>(selectUnits);

    }

    void Target_HighestHP()
    {
        int highest = 0;
        selectUnits.Clear();
        //get closest
        for(int i = 0; i < targetedUnits.Count; i++)
        {
            if(targetedUnits[i].sheet.stats.HitPoints == highest)
            {
                selectUnits.Add(targetedUnits[i]);
            }
            else if(targetedUnits[i].sheet.stats.HitPoints > highest)
            {
                selectUnits.Clear();
                highest = targetedUnits[i].sheet.stats.HitPoints;
                selectUnits.Add(targetedUnits[i]);
            }
        }
        //make highest HP targeted
        targetedUnits = new List<Character>(selectUnits);
    }

   
    void RollToHit()
    {
        toHitChance = GameSettings.phyHit_baseToHitChance;
        toHitChance = toHitChance * (1 + (GameSettings.phyHit_AttributeWeight * (actingUnit.sheet.stats.Agility + GameSettings.phyHit_RangeSpreadConstant) / 100f));
        toHitChance = toHitChance * (1 + (GameSettings.phyHit_LevelWeight * (actingUnit.sheet.level + GameSettings.phyHit_RangeSpreadConstant) / 100f));
        toHitChance = toHitChance * (1f / (1 + (GameSettings.phyHit_AttributeWeight * (targetedUnits[0].sheet.stats.Agility + GameSettings.phyHit_RangeSpreadConstant) / 100f)));
        toHitChance = toHitChance * (1f / (1 + (GameSettings.phyHit_LevelWeight * (targetedUnits[0].sheet.level + GameSettings.phyHit_RangeSpreadConstant) / 100f)));
        roll = Random.Range(0f, 1f);
        Debug.Log("Chance to Hit : " + toHitChance.ToString() + "   Rolled : " + roll.ToString());
    }

    void RollDamage()
    {
        damage = GameSettings.phyDamage_baseDamage;
        damage = damage * (1 + (GameSettings.phyDamage_AttributeWeight * (actingUnit.sheet.stats.Strength + GameSettings.phyDamage_RangeSpreadConstant) / 100f));
        damage = damage * (1 + (GameSettings.phyDamage_LevelWeight * (actingUnit.sheet.level + GameSettings.phyDamage_RangeSpreadConstant) / 100f));
        roll = damage * Random.Range(1-GameSettings.phyDamage_variance, 1+ GameSettings.phyDamage_variance);
        damage = roll;
        
    }

    void ApplyDamage()
    {
        //vitality reduction
        damage = damage * (1f / (1 + (GameSettings.phyDamageResist_AttributeWeight * targetedUnits[0].sheet.stats.Vitality / 100f)));
        damage = damage * (1f / (1 + (GameSettings.phyDamageResist_LevelWeight * targetedUnits[0].sheet.level/ 100f)));


        targetedUnits[0].piece.ShowDamage((int)damage);
        targetedUnits[0].sheet.stats.HitPoints -= (int)damage;
        if (targetedUnits[0].sheet.stats.HitPoints <= 0)
        {
            targetedUnits[0].sheet.stats.HitPoints = 0;
            targetedUnits[0].piece.GetComponent<SpriteRenderer>().color *= 0.2f;
        }

    }

    //called after any action to update the shown order
    //will figure the next 10 characters to act and store in the unitActOrder list
    //this is for UI module
    void CalculateActOrder()
    {
        //initialize calculation
        unitActOrder.Clear();
        List<float> futureActGuage = new List<float>();
        maxAct =0;
        index=0;
        for(int i = 0; i< unitsInBattle.Count; i++ )
        {
            maxAct = unitsInBattle[i].sheet.actGuage;
            futureActGuage.Add(maxAct);
        }

        bool isConditionsMet = false;
        while(unitActOrder.Count<10)
        {
            
            //check all units speed and advance out future act guage
            for(int i = 0; i < unitsInBattle.Count; i++)
            {                  
                //conditions: unit hp>0, 
                if(unitsInBattle[i].sheet.stats.HitPoints > 0)
                {        
                    futureActGuage[i] += 0.2f *(2.5f * Mathf.Sqrt(unitsInBattle[i].sheet.stats.Speed + 25f));
                    isConditionsMet = true;
                }
            }

            if(!isConditionsMet) //failsafe
            {
                Debug.Log("Error: No units gaining Act in battle");
                return; 
            }

            //get max value and index
            maxAct = 0;
            index = 0;
            for(int i = 0; i < futureActGuage.Count; i++)
            {
                if(futureActGuage[i] > maxAct)
                {
                    maxAct = futureActGuage[i];
                    index = i;
                }
            }

            //if the max is over the act value of 100, subtract 100 and add as next ot act to our list
            if (maxAct > 100f)
            {
                futureActGuage[index] -= 100;
                unitActOrder.Add(unitsInBattle[index]);
            }

            
        } //continue while unitActOder list is less than 10

        Debug.Log("Next to Act: " + unitActOrder[0].sheet.characterName + " : " + unitActOrder[1].sheet.characterName + " : " + unitActOrder[2].sheet.characterName + " : "
                    + unitActOrder[3].sheet.characterName + " : " + unitActOrder[4].sheet.characterName + " : " + unitActOrder[5].sheet.characterName + " : "
                     + unitActOrder[6].sheet.characterName + " : " + unitActOrder[7].sheet.characterName + " : " + unitActOrder[8].sheet.characterName + " : ");


    } //end function




}

