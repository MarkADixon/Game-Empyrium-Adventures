using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour {

    public AudioClip battleStart;
    float ellapsedTime = 0f;

    public GameObject unit;

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
        
        GameObject newunit = (GameObject)Instantiate(unit, Vector3.zero, Quaternion.identity);

    }

    // Update is called once per frame
    void Update ()
    {
        if(ellapsedTime == 0f) SoundManager.instance.PlaySfx(battleStart);        
        ellapsedTime += Time.deltaTime;
    }
}
