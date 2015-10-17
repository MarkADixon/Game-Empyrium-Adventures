using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.IO;

//Singleton
//Holds all game state data
//performs save/load operations
public class DataManager : MonoBehaviour
{
    public static DataManager DM = null;  //This is the singleton instance!

    //game state data
    public List<Character> playerArmy = new List<Character>();
    public List<CharacterSheet> playerSheets = new List<CharacterSheet>();
    public List<Squad> playerSquads = new List<Squad>();

    public List<Character> enemyArmy = new List<Character>();
    public List<CharacterSheet> enemySheets = new List<CharacterSheet>();
    public List<Squad> enemySquads = new List<Squad>();

    //reference data
    public Dictionary<string, Action> actionDictionary = new Dictionary<string, Action>();
    public Dictionary<string, CharacterClass> characterClassDictionary = new Dictionary<string, CharacterClass>();

    //resources   
    GameObject prefabCharacterPiece;
    TextAsset actionDataFile, classDataFile;



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
        if(DM == null)
            DM = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        //resource initialization
        prefabCharacterPiece = (GameObject)Resources.Load("characterPiece");

        actionDataFile = (TextAsset)Resources.Load("Data/ActionData", typeof(TextAsset));
        InitializeActionData();

        classDataFile = (TextAsset)Resources.Load("Data/ClassData", typeof(TextAsset));
        InitializeClassData();
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
        if(File.Exists(Application.persistentDataPath + "/savegame.dat"))
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
        for(int i = 0; i < playerSheets.Count; i++)
        {
            GameObject newPiece = (GameObject)Instantiate(prefabCharacterPiece, Vector3.zero, Quaternion.identity);
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

    void InitializeActionData()
    {
        Dictionary<string, string> actionDataElements = new Dictionary<string, string>();
        Action newAction;

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(actionDataFile.text);
        XmlNodeList actionDataNodes = xmlDocument.GetElementsByTagName("Action");

        foreach(XmlNode actionDataNode in actionDataNodes)
        {
            XmlNodeList actionSubNodes = actionDataNode.ChildNodes;
            actionDataElements = new Dictionary<string, string>();
            foreach(XmlNode tag in actionSubNodes)
            {
                actionDataElements.Add(tag.Name, tag.InnerText);
            }//end for each tag in action

            //initialize an Action and store it for game use
            newAction = new Action(actionDataElements);
            actionDictionary.Add(newAction.actionName, newAction);

        }//end of for each action in xml
        return;
    }//end of InitializeActionData()

    void InitializeClassData()
    {
        Dictionary<string, string> classDataElements = new Dictionary<string, string>();
        CharacterClass newCharClass;

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(classDataFile.text);
        XmlNodeList classNodes = xmlDocument.GetElementsByTagName("CharacterClass");

        foreach(XmlNode classNode in classNodes)
        {
            XmlNodeList classSubNodes = classNode.ChildNodes;
            classDataElements = new Dictionary<string, string>();
            foreach(XmlNode tag in classSubNodes)
            {
                if(tag.Name == "StatOnLevelUpOdds")
                {
                    XmlNodeList levelUpSubNodes = tag.ChildNodes;
                    foreach(XmlNode levelUpTag in levelUpSubNodes)
                    {
                        classDataElements.Add(levelUpTag.Name, levelUpTag.InnerText);
                    }
                }
                else
                {
                    classDataElements.Add(tag.Name, tag.InnerText);
                }
            }//end for each tag in action

            //initialize an Class and store it for game use
            newCharClass = new CharacterClass(classDataElements);
            characterClassDictionary.Add(newCharClass.name, newCharClass);
        }
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



