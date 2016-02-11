using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]

public class MessageTyper : MonoBehaviour {

    private string message = "";
    private Text textComponent;
    public float startDelay = 1f;
    public float typerDelay = 0.05f;
    public AudioClip typerNoise;

	// Use this for initialization
	void Start () {
        StartCoroutine("TypeIn");
	}

    void Awake ()
    {
        textComponent = GetComponent<Text>();
        message = textComponent.text;
    }
	
    public IEnumerator TypeIn()
    {
        textComponent.text = "";
        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i <= message.Length; i++)
        {
            textComponent.text = message.Substring(0, i);
            SoundManager.SM.sfxSource.PlayOneShot(typerNoise);
            yield return new WaitForSeconds(typerDelay);
        }
    }

    public IEnumerator TypeOff()
    {
        for (int i = message.Length; i >= 0; i--)
        {
            textComponent.text = message.Substring(0, i);
            yield return new WaitForSeconds(typerDelay);
        }
    }
}
