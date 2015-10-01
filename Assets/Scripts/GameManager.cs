using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for lists


public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    // Use this for initialization
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }




    


    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();




        if (Input.GetKeyDown(KeyCode.B)) BattleManager.instance.StartNewBattle(DataManager.DM.playerSquads[0], DataManager.DM.enemySquads[0]);
        if(Input.GetKeyDown(KeyCode.S)) DataManager.DM.Save();
        if(Input.GetKeyDown(KeyCode.L)) DataManager.DM.Load();

    }
}
