using UnityEngine;
using System.Collections;

public class DamageNumber : MonoBehaviour {

	SpriteRenderer spr;
    Transform xform;
	float timer = 4f;
	bool isHeal = false;
	Vector3 move = new Vector3(0f,0.01f,0f);
	float ypos = 0f;
	float ystart = 0f;

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
            if (!isHeal)
                spr.color = new Color(1f, 1f, 1f, timer);
            else
                spr.color = new Color(0f, 1f, 0f, timer);
            spr.transform.position += move;
        }
        else
        {
            Destroy(gameObject);
        }
    }

	public void Create(int _number, Vector3 _position, bool _isHeal, float _delay){
        spr = GetComponent<SpriteRenderer>();
        xform = GetComponent<Transform>();
        spr.sprite = numbers[_number];
        timer += _delay;
		isHeal = _isHeal;
		xform.position = _position;
		ystart = _position.y;
		spr.transform.localScale = new Vector3 (0.66f, 0.66f, 0.66f);
        if (isHeal)
            spr.color = Color.green;
        else spr.color = Color.white;
	}
}
