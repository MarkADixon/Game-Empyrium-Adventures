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
        data.playerArmy = playerArmy;
        data.enemyArmy = enemyArmy;

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
            playerArmy = data.playerArmy;
            enemyArmy = data.enemyArmy;

            InitializeData();

        }
    }

    public void InitializeData()
    {
        //temp initialize squads. TODO code to initialize all squads
        Squad p = new Squad();
        Squad q = new Squad();
        p.CreateSquad(0, playerArmy);
        playerSquads.Add(p);
        q.isPlayer = false;
        q.CreateSquad(0, enemyArmy);
        enemySquads.Add(q);
    }
}

[Serializable]
public class FileData
{
   public bool isGameDataLoaded;
    //game state data
    public List<Character> playerArmy = new List<Character>();
    public List<Character> enemyArmy = new List<Character>();
    
}

#endregion



