using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


//data object for characters grouped together and functions for changing that grouping

public class Squad 
{
    public int squadID = -1;
    public Character leader;
    public List<Character> members = new List<Character>();
    public bool isPlayer = true;

    //calculated values
    public float movement = 0;
    public float moralle =0;
    public float alignment = 0;
    public int[,] formation = new int[6,6]; //references index in the list at location in squad grid

    public Squad ()
    {
        

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
            if(newMembers[i].sheet.isLeader)
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
            members[i].sheet.squadID = squadID;
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
            //movement += members[i].sheet.stats.move;
            moralle += members[i].sheet.stats.morale;
            alignment += members[i].sheet.stats.alignment;
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
            for (int row = members[i].sheet.squadLocX; row < members[i].sheet.squadLocX + (int)members[i].sheet.size; row++ )
            {
                for(int col = members[i].sheet.squadLocY; col < members[i].sheet.squadLocY + (int)members[i].sheet.size; col++)
                {
                    formation[row, col] = i;
                }
            }
        }      
    }

    public List<Character> GetCharactersInRow(int row)
    {
        List<Character> charactersInRow = new List<Character>();

        for(int place = 0; place < 6; place++)
        {
            if (formation[row,place] != -1)
            {
                if (!charactersInRow.Contains(members[formation[row,place]]))
                {
                    charactersInRow.Add(members[formation[row, place]]);
                }
            }
        }

        return charactersInRow;
    }

    public List<Character> GetCharactersInHorizontal(int horizontal)
    {
        List<Character> charactersInHorizontal = new List<Character>();

        for(int row = 0; row < 6; row++)
        {
            if(formation[row, horizontal] != -1)
            {
                if(!charactersInHorizontal.Contains(members[formation[row, horizontal]]))
                {
                    charactersInHorizontal.Add(members[formation[row, horizontal]]);
                }
            }
        }

        return charactersInHorizontal;
    }


}
