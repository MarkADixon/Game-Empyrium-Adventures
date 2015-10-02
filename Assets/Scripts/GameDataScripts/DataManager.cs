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
    public List<Character> playerArmy;
    public List<CharacterSheet> playerSheets;
    public List<Squad> playerSquads;

    public List<Character> enemyArmy;
    public List<CharacterSheet> enemySheets;
    public List<Squad> enemySquads;


    //resources
    GameObject prefabCharacterPiece;

    //manual Initialize Call, called on awake and before load
    void Init()
    {
        playerArmy = new List<Character>();
        playerSheets = new List<CharacterSheet>();
        playerSquads = new List<Squad>();

        enemyArmy = new List<Character>();
        enemySheets = new List<CharacterSheet>();
        enemySquads = new List<Squad>();
    }

    // Use this for initialization
    void Awake()
    {
        if (DM == null) DM = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        //resource initialization
        prefabCharacterPiece = (GameObject)Resources.Load("characterPiece");

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
        PrepareToSaveData();

        //will work on everything except web deploy
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savegame.dat");
        FileData data = new FileData();

        //store data here
        data.isGameDataLoaded = GameSettings.isGameDataLoaded;
        data.playerSheets = playerSheets;
        data.enemySheets = enemySheets;

        //save and close
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Data Saved");

    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savegame.dat"))
        {
            Init(); //clears data and initializes lists

            //open and load
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savegame.dat", FileMode.Open);
            FileData data = (FileData)bf.Deserialize(file);
            
            file.Close();

            //restore data to other objects
            GameSettings.isGameDataLoaded = data.isGameDataLoaded;
            playerSheets = data.playerSheets;
            enemySheets = data.enemySheets;

            InitializeLoadedData();
            Debug.Log("Data Loaded");
        }
    }

    public void PrepareToSaveData()
    {

    }

    public void InitializeLoadedData()
    {
        //create pieces for each sheet then add characters to player army
        for (int i = 0; i < playerSheets.Count; i++)
        {
            GameObject newPiece = (GameObject)Instantiate(prefabCharacterPiece,Vector3.zero, Quaternion.identity);
            Character newCharacter = new Character(playerSheets[i], newPiece.GetComponent<CharacterPiece>());
            playerArmy.Add(newCharacter);
        }

        //create pieces for each enemy sheet then add characters to enemy army
        for(int i = 0; i < enemySheets.Count; i++)
        {
            GameObject newPiece = (GameObject)Instantiate(prefabCharacterPiece, Vector3.zero, Quaternion.identity);
            Character newCharacter = new Character(enemySheets[i], newPiece.GetComponent<CharacterPiece>());
            enemyArmy.Add(newCharacter);
        }

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
    public List<CharacterSheet> playerSheets = new List<CharacterSheet>();
    public List<CharacterSheet> enemySheets = new List<CharacterSheet>();
    
}

#endregion



