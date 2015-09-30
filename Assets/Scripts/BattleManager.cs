using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {

    public AudioClip battleStart;
    float ellapsedTime = 0f;

    Vector3 playerSquadOffset = new Vector3(3f,-3.75f,0);
    Vector3 enemySquadOffset = new Vector3(-3f,0f, 0);
    Vector3 sizeOffset;

    Squad playerSquad;
    Squad enemySquad;

    public static BattleManager instance = null;
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

    public void StartNewBattle(Squad _playerSquad, Squad _enemySquad)
    {
        playerSquad = _playerSquad;
        enemySquad = _enemySquad;

        ellapsedTime = 0f;

        SoundManager.SM.PlaySfx(battleStart);
        playerSquad.CreateBattleGroup();
        enemySquad.CreateBattleGroup();
    }

    // Update is called once per frame
    void Update ()
    {       
        ellapsedTime += Time.deltaTime;
    }

    

}
