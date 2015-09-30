using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

    

    public AudioSource sfxSource;
    public AudioSource musicSource;


    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    public static SoundManager SM = null;
    // Use this for initialization
    void Awake ()
    {
        if (SM == null) SM = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
	}
    
    void Start ()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
	
	}

    public void PlaySfx(params AudioClip[] clips)
    {
        if(clips.Length == 0)
        {
            Debug.Log("no sound to play!");
            return;
        }

        sfxSource.clip = clips[Random.Range(0,clips.Length)];
        sfxSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        sfxSource.Play();
    }

}
