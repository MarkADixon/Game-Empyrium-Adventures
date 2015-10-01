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

    List<Character> unitsInBattle = new List<Character>();


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

        for(int i = 0; i < playerSquad.members.Count; i++)
        {
            unitsInBattle.Add( playerSquad.members[i]);
        }
        for(int i = 0; i < enemySquad.members.Count; i++)
        {
            unitsInBattle.Add(enemySquad.members[i]);
        }
    }

    // Update is called once per frame
    void Update ()
    {       
        ellapsedTime += Time.deltaTime;
        AdvanceACTGuages();
    }

    //speed formula: Add to act guage each combat tick
    public void AdvanceACTGuages()
    {
        actGuage += (2f * Mathf.Sqrt(stats.Speed + 25f));
    }



}
