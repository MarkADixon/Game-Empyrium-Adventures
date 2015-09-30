using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour
{

    public static DataManager instance = null;
    // Use this for initialization
    void Awake()
    {
        if (instance == null) instance = this;
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
        GameData data = new GameData();

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
            GameData data = (GameData)bf.Deserialize(file);
            
            file.Close();

            //restore data to other objects
            GameSettings.isGameDataLoaded = data.isGameDataLoaded;
        }
    }
}

[Serializable]
public class GameData
{
   public bool isGameDataLoaded;
}

#endregion


[Serializable]
public class UnitClass
{
    public enum ClassNames
    {
        None,
        Ashigaru, Engineer, Gunner, Junker, Knight, Magi,
        Pirate, Scout, Valkyrie, Wark, Witch
    };
}


[Serializable]
public class Squad
{
    public int squadID;
    public Character leader;
    public List<Character> members;
}

[Serializable]
public class Character
{
    public int unitID;
    public int squadID;
    public Vector2 squadLocation;

    //character values
    public string name = "";
    public UnitClass charClass;
    public int hp;
    public int emp;
    public int atk;
    public int def;
    public int mag;
    public int res;
    public int spi;
    public int agi;
    public int lck;

    public float dodge;
    public float evade;
    public float absorb;
    public float block;

    public Action AutoAct;
    public Action TriggerAct;
    public Ability Active;
    public Ability Passive;

}

[Serializable]
public class Action
{
    
}

[Serializable]
public class Ability
{
    
}

