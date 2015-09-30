using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//Singleton
//Holds all game state data
//performs save/load operations
public class DataManager : MonoBehaviour
{
    public static DataManager DM = null;  //This is the singleton instance!

    //game state data
    public List<Character> playerArmy = new List<Character>();
    public List<Squad> playerSquads = new List<Squad>();

    public List<Character> enemyArmy = new List<Character>();
    public List<Squad> enemySquads = new List<Squad>();

    // Use this for initialization
    void Awake()
    {
        if (DM == null) DM = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {

        //temproary test data
        /*
        Character a = new Character();
        a.squadID = 0;
        a.squadLocX = 0;
        a.squadLocY = 0;
        a.unitClass.size = Size.Medium;
        a.isLeader = true;
        Character b = new Character();
        b.squadID = 100;
        b.squadLocX = 2;
        b.squadLocY = 2;
        b.unitClass.size = Size.Medium;
        b.isLeader = true;
        Squad p = new Squad();
        Squad q = new Squad();
        p.CreateSquad(0,a);
        playerSquads.Add(p);
        q.CreateSquad(0,b);
        enemySquads.Add(q);
        */

        Squad p = new Squad();
        Squad q = new Squad();
        p.CreateSquad(0, playerArmy);
        playerSquads.Add(p);
        q.isPlayer = false;
        q.CreateSquad(0, enemyArmy);
        enemySquads.Add(q);


    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnGUI()
    {

    }

    #region Save/Load Game
    public void Save()
    {
        //will work on everything except web deploy
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savegame.dat");
        FileData data = new FileData();

        //store data here
        data.isGameDataLoaded = GameSettings.isGameDataLoaded;

        //save and close
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savegame.dat"))
        {
            //open and load
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savegame.dat", FileMode.Open);
            FileData data = (FileData)bf.Deserialize(file);
            
            file.Close();

            //restore data to other objects
            GameSettings.isGameDataLoaded = data.isGameDataLoaded;
        }
    }
}

[Serializable]
public class FileData
{
   public bool isGameDataLoaded;
}

#endregion



