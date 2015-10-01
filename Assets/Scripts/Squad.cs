using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Squad 
{
    public int squadID = -1;
    public Character leader;
    public List<Character> members = new List<Character>();
    public List<Unit> memberUnit = new List<Unit>(); //graphical representation game object of members
    public bool isPlayer = true;

    //calculated values
    public float movement = 0;
    public float moralle =0;
    public float alignment = 0;
    public int[,] formation = new int[6,6]; //references index in the list at location in squad grid

    //resources
    GameObject prefabUnit;

    GameObject battleGroup = new GameObject();

    public Squad ()
    {
        prefabUnit = (GameObject)Resources.Load("unit");

        //initialize the formation data
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                formation[i, j] = -1;
            }
        }

        
    }

    void Start()
    {
        
    }



    public void AddMember()
    {

    }

    public void RemoveMember()
    {

    }

    public void DisbandSquad()
    {

    }



    public void MoveMember()
    {

    }

    public void ChangeLeader()
    {

    }

    public void CreateSquad(int in_squadID, List<Character> newMembers)
    {
        //initialize local variables
        bool isLeaderAssigned = false;
        squadID = in_squadID;
        members = new List<Character>();

        //add memebrs to the squad list
        for(int i = 0; i < newMembers.Count; i++)
        {      
            members.Add(newMembers[i]);
            if(newMembers[i].isLeader)
            {
                if(isLeaderAssigned)  //check for leader already assigned
                {
                    Debug.Log("Error multiple leaders in squad.");
                }
                else //assign reference to leader
                {
                    leader = newMembers[i];
                    isLeaderAssigned = true;
                }
            }
        }

        //check for no leader in squad
        if(!isLeaderAssigned)
        {
            Debug.Log("Error no leader in squad.");
        }


        //populate formation matrix
        //calculate squad stats
        RecalculateSquad();

        //pass squadID back to characters
        for(int i = 0; i < members.Count; i++)
        {
            members[i].squadID = squadID;
        }
    }

    public void RecalculateSquad()
    {
        RecalculateSquadStats();
        RecalculateSquadPlacement();
    }

    public void RecalculateSquadStats()
    {
        movement = 0f;
        moralle = 0f;
        alignment = 0f;

        for (int i = 0; i < members.Count; i++)
        {
            movement += members[i].stats.Movement;
            moralle += members[i].stats.Moralle;
            alignment += members[i].stats.Alignment;
        }

        movement = movement / members.Count;
        moralle = moralle / members.Count;
        alignment = alignment / members.Count;
    }

    public void RecalculateSquadPlacement()
    {
        //clear the formation data
        for(int i =0;i<6;i++)
        {
            for(int j = 0;j < 6; j++)
            {
                formation[i, j] = -1;
            }
        }

        //add each member
        for(int i = 0; i < members.Count; i++)
        {
            for (int row = members[i].squadLocX; row < members[i].squadLocX + (int)members[i].unitClass.size; row++ )
            {
                for(int col = members[i].squadLocY; col < members[i].squadLocY + (int)members[i].unitClass.size; col++)
                {
                    formation[row, col] = i;
                }
            }
        }      
    }

    public void CreateBattleGroup()
    {      
        battleGroup.name = isPlayer? "playerSquad":"enemySquad";
        battleGroup.transform.position = Vector3.zero;
        Vector3 playerSquadOffset = new Vector3(3f, -3.75f, 0);
        Vector3 sizeOffset;
        for(int i = 0; i < members.Count; i++)
        {
            sizeOffset = ((int)members[i].unitClass.size - 1) * new Vector3(0.5f, 0.5f, 0.5f) * 0.75f;
            Vector3 spawnLocation = new Vector3(members[i].squadLocX * 0.75f, members[i].squadLocY * 0.75f, members[i].squadLocY);
            GameObject newUnit = (GameObject)UnityEngine.Object.Instantiate(prefabUnit, spawnLocation + playerSquadOffset + sizeOffset, Quaternion.identity);
            newUnit.transform.parent = battleGroup.transform;
            memberUnit.Add(newUnit.GetComponent<Unit>());
        }

        // invert enemy squad facing
        if(!isPlayer) battleGroup.transform.localScale = new Vector3(battleGroup.transform.localScale.x * -1, battleGroup.transform.localScale.y, battleGroup.transform.localScale.z);
    }

}
