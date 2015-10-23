using UnityEngine;
using System.Collections;

public class DamageNumber : MonoBehaviour {

	SpriteRenderer spr;
    Transform xform;
	float timer = 4f;
	Vector3 move = new Vector3(0f,0.01f,0f);
	float ypos = 0f;
	float ystart = 0f;
    Color baseColor = Color.white;

    public Sprite[] numbers;
	
	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        if (spr.color.a >= 0.005f)
        {
            timer -= Time.deltaTime * 4f;
            if (timer > 4f) { return; }
            ypos = (timer - 3f) * 0.1f;
            if ((xform.position.y + ypos < ystart)) ypos = 0;
            move = new Vector3(0f, ypos, 0f);
            spr.color = new Color(baseColor.r, baseColor.g, baseColor.b, timer);
            spr.transform.position += move;
        }
        else
        {
            Destroy(gameObject);
        }
    }

	public void Create(int _number, Vector3 _position, Color newColor, float _delay){
        spr = GetComponent<SpriteRenderer>();
        xform = GetComponent<Transform>();
        spr.sprite = numbers[_number];
        timer += _delay;
		xform.position = _position;
		ystart = _position.y;
		spr.transform.localScale = new Vector3 (0.66f, 0.66f, 0.66f);
        baseColor = newColor;
        spr.color = newColor;
	}
}
