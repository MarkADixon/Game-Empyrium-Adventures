using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BattleManager : MonoBehaviour {

    public bool isStatePaused = false;
    public bool isStateSelectTarget = false;

    public AudioClip battleStart;
    float ellapsedTime = 0f;

    //incoming data to instantate battle from
    Squad playerSquad;
    Squad enemySquad;

    //for location calculations on screen placement
    Vector3 playerSquadOffset = new Vector3(2.6f, -3.2f, 0);
    Vector3 sizeOffset, spawnLocation;
    float scaleDistBetweenUnits = 0.75f;

    List<Character> unitsInBattle = new List<Character>();
    List<Character> unitActOrder = new List<Character>();

    bool isUnitActing = false;
    Character actingUnit, clickedUnit;
    List<Character> targetedUnits = new List<Character>();
    List<Character> hitUnits = new List<Character>();
    List<Character> selectUnits = new List<Character>();

    //game object containers for squad graphics objects
    GameObject playerBattleGroup;
    GameObject enemyBattleGroup;

    //for battle calculations
    float maxAct = 0f;
    int index = 0;
    float damage;
    Effect appliedEffect;
    bool isAutoHit = false;


    //make BM a singleton
    public static BattleManager instance = null;

    //initialize here
    void Awake()
    {
        //make BM a singleton
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);


        
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

        playerBattleGroup = new GameObject();
        enemyBattleGroup = new GameObject();
        CreateBattleGroup(playerSquad, playerBattleGroup);
        CreateBattleGroup(enemySquad, enemyBattleGroup);

        RollInitative();

        //set up action
        for(int i = 0; i < unitsInBattle.Count; i++)
        {
            SetNextAction(unitsInBattle[i]);
        }


        CalculateActOrder();
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
            sizeOffset = ((int)_squad.members[i].sheet.size - 1) * Vector3.one * 0.5f * scaleDistBetweenUnits;
            spawnLocation = new Vector3(_squad.members[i].sheet.squadLocX, _squad.members[i].sheet.squadLocY, _squad.members[i].sheet.squadLocY) * scaleDistBetweenUnits;
            _squad.members[i].piece.transform.localPosition = spawnLocation + playerSquadOffset + sizeOffset;
            _squad.members[i].piece.gameObject.SetActive(true);
            _squad.members[i].piece.Initialize(_squad.members[i].sheet,i);

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
            unitsInBattle[i].sheet.stats.action = Random.Range(0f, 75f);
        }
    }


    void OnGUI()
    {
        if(isStateSelectTarget)
        {
            GUILayout.Label("Select a target!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();

        if(isStatePaused || isStateSelectTarget)
            return;


        ellapsedTime += Time.deltaTime;

        //if a unit is not acting, advance ATguages until one does and set next to act 
        if (!isUnitActing)
        {
            AdvanceACTGuagesTurnBased();           
        }

        if(isUnitActing)
        {
            actingUnit.sheet.action_Next.UpdateAction();

            if(actingUnit.sheet.action_Next.isTriggerEffect)
            {
                //purely debug block
                DetermineTargets(actingUnit.sheet.action_Next.actionComponents[0].targetType);
                foreach(Character unit in targetedUnits)
                {
                    Debug.Log(actingUnit.sheet.characterName + " uses " + actingUnit.sheet.action_Next.actionName + " on " + unit.sheet.characterName);
                }
                //end debug output

                foreach(ActionComponent actionComponent in actingUnit.sheet.action_Next.actionComponents)
                {
                    isAutoHit = false;  //certain targeting will set this true to skip roll to hit
                    DetermineTargets(actionComponent.targetType);
                    hitUnits = new List<Character>();
                    foreach(Character targetUnit in targetedUnits)
                    {
                        bool isHit = false;
                        if(isAutoHit)
                        {
                            isHit = true;
                        }
                        else
                        {
                            isHit = RollToHit(actionComponent.baseToHit, actionComponent.hitType, targetUnit);
                        }
                        if(isHit)
                        {
                            hitUnits.Add(targetUnit);
                            //apply effect
                            appliedEffect = new Effect(actionComponent,actingUnit,targetUnit);
                            appliedEffect.AddEffect();
                        }
                        else
                        {
                            //missed
                        }
                    }
                }

                CalculateActOrder();
                actingUnit.sheet.action_Next.isTriggerEffect = false;
                actingUnit.sheet.action_Next.isEffectResolved = true;
            }
            if (actingUnit.sheet.action_Next.isActionFinished)
            {
                UpdateEffects(); //tick all effects
                RegenShields();
                isUnitActing = false;
            }

        }

    }

    void CheckInput()
    {
        foreach(Character unit in unitsInBattle)
        {
            //hack, activate special
            if (unit.piece.isClicked)
            {
                if(!isStateSelectTarget)
                {
                    unit.sheet.action_Next = unit.sheet.actions_Activated[Random.Range(0, unit.sheet.actions_Activated.Count)];
                    unit.piece.isClicked = false;
                    clickedUnit = unit;
                    isStateSelectTarget = true;
                }
                else
                {
                    clickedUnit.selectedTarget = unit;
                    unit.piece.isClicked = false;
                    isStateSelectTarget = false;
                }
            }
        }
    }


 
    void UpdateEffects ()
    {
        foreach(Character unit in unitsInBattle)
        {
            for(int i = unit.effects.Count - 1; i >= 0; i--)
            {
                if(unit == actingUnit) //acting unit tick all effects and reduce duration
                {
                    unit.effects[i].ActiveTick();
                }
                else  //tick effects for all non active units, does not reduce duration
                {
                    unit.effects[i].InactiveTick(); 
                }
            }
        }
    }

    void RegenShields()
    {
        foreach(Character unit in unitsInBattle)
        {
            if(unit.sheet.stats.health > 0)
            {
                if(unit.sheet.stats.shield < unit.sheet.stats.maxShield)
                {
                    unit.sheet.stats.shield += (0.025f * unit.sheet.stats.maxShield);
                }
                else
                {
                    unit.sheet.stats.shield = unit.sheet.stats.maxShield;
                }
                unit.piece.UpdateUI();
            }
        }
    }



    void SetNextAction(Character character)
    {
        bool isSelectFront = true; //true to select front, false to select from rear
        float odds = 0f;
        //use position to determine if rear selection should occur
        if (character.sheet.size == SizeType.SMALL)
        {
            odds = character.sheet.squadLocX * 0.2f;
        }
        else if (character.sheet.size == SizeType.NORMAL)
        {
            odds = character.sheet.squadLocX * 0.25f;
        }
        else if(character.sheet.size == SizeType.LARGE)
        {
            odds = character.sheet.squadLocX * 0.33334f;
        }
        else if(character.sheet.size == SizeType.HUGE)
        {
            odds = character.sheet.squadLocX * 0.5f;
        }

        //roll for rear action
        if (GameManager.GM.RollZeroToUnderOne() <= odds)
        {
            isSelectFront = false;
        }
        
        //assign action
        if (isSelectFront)
        {
            character.sheet.action_Next = character.sheet.actions_FrontRow[Random.Range(0, character.sheet.actions_FrontRow.Count)];
        }
        else
        {
            character.sheet.action_Next = character.sheet.actions_BackRow[Random.Range(0, character.sheet.actions_BackRow.Count)];
        }


    }

    //dertemine targets of the action and store in targetedUnits[]
    void DetermineTargets(TargetType targetType)
    {
        //reset targetunits array 
        targetedUnits = new List<Character>(unitsInBattle);
    
        switch (targetType)
        {
            case TargetType.SELF:
                {
                    targetedUnits.Clear();
                    targetedUnits.Add(actingUnit);
                    break;
                }
            case TargetType.SAME:
                {
                    targetedUnits = new List<Character>(hitUnits);
                    break;
                }
            case TargetType.SELECTED_ENEMY:  //todo break these cases out and add a check to verify target selection
            case TargetType.SELECTED_ALLY:
            case TargetType.SELECTED_OTHER_ALLY:
                {
                    targetedUnits.Clear();
                    targetedUnits.Add(actingUnit.selectedTarget);
                    break;
                }
            case TargetType.SAME_IFHIT:
                {
                    targetedUnits.Clear();
                    if(hitUnits.Count>0)
                    {
                        targetedUnits = new List<Character>(hitUnits);
                        isAutoHit = true;
                    }
                    break;
                }
            case TargetType.SELF_IFHIT:
                {
                    targetedUnits.Clear();
                    if(hitUnits.Count > 0)
                    {
                        targetedUnits.Add(actingUnit);
                        isAutoHit = true;
                    }
                    break;
                }
            case TargetType.ENEMY_SINGLE_MELEE:  //taget nearest with preference to same horizontal
                {
                    Target_RemoveAllies();
                    Target_RemoveFallen();
                    Target_NearestWithLinePreference();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.ENEMY_SINGLE_RANGED: //target nearest in backmost row
                {
                    Target_BackRow(); //no need to remove allies/fallen as it polls squad directly
                    Target_Nearest();
                    break;
                }
            case TargetType.ENEMY_SINGLE_RANDOM:
                {
                    Target_RemoveAllies();
                    Target_RemoveFallen();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.ENEMY_LOWEST_HP:
                {
                    Target_RemoveAllies();
                    Target_RemoveFallen();
                    Target_LowestHP();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.ENEMY_HIGHEST_HP:
                {
                    Target_RemoveAllies();
                    Target_RemoveFallen();
                    Target_HighestHP();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.ALLY_SINGLE_FALLEN:
                {
                    Target_RemoveEnemies();
                    Target_RemoveUnfallen();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.ALLY_LOWEST_HP:
                {
                    Target_RemoveEnemies();
                    Target_RemoveFallen();
                    Target_LowestHP();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.ALLY_HIGHEST_HP:
                {
                    Target_RemoveEnemies();
                    Target_RemoveFallen();
                    Target_HighestHP();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.ALLY_SINGLE_RANDOM:
                {
                    Target_RemoveEnemies();
                    Target_RemoveFallen();
                    Target_RandomSelectOne();
                    break;
                }
            case TargetType.ALL_NOT_FALLEN:
                {
                    Target_RemoveFallen();
                    break;
                }
            default:
                {                   
                    break;
                }
        }
    }

    #region Target Sub Functions
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
            if(targetedUnits[i].sheet.stats.health <= 0)
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
            if(targetedUnits[i].sheet.stats.health > 0)
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
            if(targetedUnits[i].sheet.stats.health == lowest)
            {
                selectUnits.Add(targetedUnits[i]);
            }
            else if(targetedUnits[i].sheet.stats.health < lowest)
            {
                selectUnits.Clear();
                lowest = targetedUnits[i].sheet.stats.health;
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
            if(targetedUnits[i].sheet.stats.health == highest)
            {
                selectUnits.Add(targetedUnits[i]);
            }
            else if(targetedUnits[i].sheet.stats.health > highest)
            {
                selectUnits.Clear();
                highest = targetedUnits[i].sheet.stats.health;
                selectUnits.Add(targetedUnits[i]);
            }
        }
        //make highest HP targeted
        targetedUnits = new List<Character>(selectUnits);
    }
    #endregion

    bool RollToHit(float baseToHit, HitType hitType, Character target)
    {
        float toHitChance = baseToHit;
        float roll = GameManager.GM.RollZeroToUnderOne();
        switch(hitType)
        {
            case HitType.NONE:
                {
                    //straight up roll < hit chance
                    break;
                }
            case HitType.PHYSICAL:
                {
                    toHitChance *= (1 + (GameSettings.phyHit_AttributeWeight * (actingUnit.sheet.stats.agility + GameSettings.phyHit_RangeSpreadConstant) / 100f));
                    toHitChance *= (1 + (GameSettings.phyHit_LevelWeight * (actingUnit.sheet.level + GameSettings.phyHit_RangeSpreadConstant) / 100f));
                    toHitChance *= (1f / (1 + (GameSettings.phyHit_AttributeWeight * (target.sheet.stats.agility + GameSettings.phyHit_RangeSpreadConstant) / 100f)));
                    toHitChance *= (1f / (1 + (GameSettings.phyHit_LevelWeight * (target.sheet.level + GameSettings.phyHit_RangeSpreadConstant) / 100f)));
                    break;
                }
            case HitType.MAGICAL:
                {
                    toHitChance *= (1 + (GameSettings.phyHit_AttributeWeight * (actingUnit.sheet.stats.mind + GameSettings.phyHit_RangeSpreadConstant) / 100f));
                    toHitChance *= (1 + (GameSettings.phyHit_LevelWeight * (actingUnit.sheet.level + GameSettings.phyHit_RangeSpreadConstant) / 100f));
                    toHitChance *= (1f / (1 + (GameSettings.phyHit_AttributeWeight * (target.sheet.stats.mind + GameSettings.phyHit_RangeSpreadConstant) / 100f)));
                    toHitChance *= (1f / (1 + (GameSettings.phyHit_LevelWeight * (target.sheet.level + GameSettings.phyHit_RangeSpreadConstant) / 100f)));
                    break;
                }
            case HitType.BOTH:
                {
                    toHitChance *= (1 + (GameSettings.phyHit_AttributeWeight * (actingUnit.sheet.stats.agility + GameSettings.phyHit_RangeSpreadConstant) / 100f));
                    toHitChance *= (1 + (GameSettings.phyHit_LevelWeight * (actingUnit.sheet.level + GameSettings.phyHit_RangeSpreadConstant) / 100f));
                    toHitChance *= (1f / (1 + (GameSettings.phyHit_AttributeWeight * (target.sheet.stats.agility + GameSettings.phyHit_RangeSpreadConstant) / 100f)));
                    toHitChance *= (1f / (1 + (GameSettings.phyHit_LevelWeight * (target.sheet.level + GameSettings.phyHit_RangeSpreadConstant) / 100f)));
                    toHitChance *= (1 + (GameSettings.phyHit_AttributeWeight * (actingUnit.sheet.stats.mind + GameSettings.phyHit_RangeSpreadConstant) / 100f));
                    toHitChance *= (1 + (GameSettings.phyHit_LevelWeight * (actingUnit.sheet.level + GameSettings.phyHit_RangeSpreadConstant) / 100f));
                    toHitChance *= (1f / (1 + (GameSettings.phyHit_AttributeWeight * (target.sheet.stats.mind + GameSettings.phyHit_RangeSpreadConstant) / 100f)));
                    toHitChance *= (1f / (1 + (GameSettings.phyHit_LevelWeight * (target.sheet.level + GameSettings.phyHit_RangeSpreadConstant) / 100f)));
                    break;
                }
            default:
                break;
        }

        Debug.Log("Chance to Hit : " + toHitChance.ToString() + "   Rolled : " + roll.ToString());
        return (roll < toHitChance);
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
            maxAct = unitsInBattle[i].sheet.stats.action;
            futureActGuage.Add(maxAct);
        }

        bool isConditionsMet = false;
        while(unitActOrder.Count<10)
        {
            
            //check all units speed and advance out future act guage
            for(int i = 0; i < unitsInBattle.Count; i++)
            {                  
                //conditions: unit hp>0, 
                if(unitsInBattle[i].sheet.stats.health > 0)
                {        
                    futureActGuage[i] += 0.1f *(2.5f * Mathf.Sqrt(unitsInBattle[i].sheet.stats.speed + 25f));
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
                futureActGuage[index] -= unitsInBattle[index].sheet.action_Next.actionGuageUsed*100f;
                unitActOrder.Add(unitsInBattle[index]);
            }

            
        } //continue while unitActOder list is less than 10

        Debug.Log("Next to Act: " + unitActOrder[0].sheet.characterName + " : " + unitActOrder[1].sheet.characterName + " : " + unitActOrder[2].sheet.characterName + " : "
                    + unitActOrder[3].sheet.characterName + " : " + unitActOrder[4].sheet.characterName + " : " + unitActOrder[5].sheet.characterName + " : "
                     + unitActOrder[6].sheet.characterName + " : " + unitActOrder[7].sheet.characterName + " : " + unitActOrder[8].sheet.characterName + " : ");


    } //end function


    //determine which unit will be acting this frame (max ACT) if over 100 ACT and flag the frame for a unit acting and set reference to acting unit
    void SetActingUnit()
    {
        isUnitActing = false;

        //get max value and index
        maxAct = 0;
        index = 0;
        for(int i = 0; i < unitsInBattle.Count; i++)
        {
            if(unitsInBattle[i].sheet.stats.action > maxAct)
            {
                maxAct = unitsInBattle[i].sheet.stats.action;
                index = i;
            }
        }

        //if the max is over the act value of 100, subtract 100 and flag to act
        if(maxAct > 100f)
        {
            isUnitActing = true;
            actingUnit = unitsInBattle[index];
            unitsInBattle[index].sheet.stats.action -= unitsInBattle[index].sheet.action_Next.actionGuageUsed*100f;
            unitsInBattle[index].piece.Animate_Attack();
        }
    }



    //speed formula: Add to act guage each combat tick
    void AdvanceACTGuagesTurnBased()
    {
        bool isConditionsMet = false;
        while(!isUnitActing)
        {

            //check all units speed and advance out future act guage
            for(int i = 0; i < unitsInBattle.Count; i++)
            {
                //conditions: unit hp>0, 
                if(unitsInBattle[i].sheet.stats.health > 0)
                {
                    unitsInBattle[i].sheet.stats.action += 0.1f * (2.5f * Mathf.Sqrt(unitsInBattle[i].sheet.stats.speed + 25f));
                    isConditionsMet = true;
                }
            }

            if(!isConditionsMet) //failsafe
            {
                //Debug.Log("Error: No units gaining Act in battle");
                return;
            }

            //get max value and index
            maxAct = 0;
            index = 0;
            for(int i = 0; i < unitsInBattle.Count; i++)
            {
                if(unitsInBattle[i].sheet.stats.action > maxAct)
                {
                    maxAct = unitsInBattle[i].sheet.stats.action;
                    index = i;
                }
            }

            //if the max is over the act value of 100, subtract action value and flag unit as acting
            if(maxAct > 100f)
            {
                isUnitActing = true;
                actingUnit = unitsInBattle[index];
                unitsInBattle[index].sheet.stats.action -= unitsInBattle[index].sheet.action_Next.actionGuageUsed*100f;
                actingUnit.sheet.action_Next.StartAction();
                actingUnit.piece.Animate_Attack();
            }
        } //repeat while no unit set to act

    }

}

