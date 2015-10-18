using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BattleUIManager : MonoBehaviour
{

    //make BM a singleton
    public static BattleUIManager instance = null;

    //initialize here
    void Awake()
    {
        //make BM a singleton
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {

    }
}
