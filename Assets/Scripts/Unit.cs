using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{

	GameObject _obj,obj;
	bool isType = true;
	bool isLarge = false;
	public bool isEnemy = false;
	public bool isAttackStart = false;
	public bool isAttackFinish = false;
	Vector3 fixedPosition = new Vector3 (0f, 0f, 0f);
	public int init = 0;

	Animator[] ani;
	float timer = 0;


    public GameObject damageNumber;
    /*UnitClass unitClass;


	// Use this for initialization

    public Unit(BattleControl.UnitTypes _type, GameObject _unitPos, bool _isEnemy) {
				this.type = _type;
				this.isEnemy = _isEnemy;
				unitClass = GameData.obj.gameClasses[0];
			
				switch (type) {
				case BattleControl.UnitTypes.Ashigaru:
						{
								_obj = GameObject.Find ("Ashigaru");
								//unitClass = GameObject.Find ("BattlemaidenClass").GetComponent<UnitClass>();
								break;
						}
				case BattleControl.UnitTypes.Engineer:
						{
								_obj = GameObject.Find ("Engineer");
								//unitClass = GameObject.Find ("BattlemaidenClass").GetComponent<UnitClass>();
								break;
						}
				case BattleControl.UnitTypes.Gunner:
						{
								_obj = GameObject.Find ("Gunner");
								//unitClass = GameObject.Find ("BattlemaidenClass").GetComponent<UnitClass>();
								break;
						}
				case BattleControl.UnitTypes.Junker:
						{
								_obj = GameObject.Find ("Junker");
								//unitClass = GameObject.Find ("BattlemaidenClass").GetComponent<UnitClass>();
								break;
						}
				case BattleControl.UnitTypes.Knight:
						{
								_obj = GameObject.Find ("Knight");
								//unitClass = GameObject.Find ("BattlemaidenClass").GetComponent<UnitClass>();
								break;
						}
				case BattleControl.UnitTypes.Magi:
						{
								_obj = GameObject.Find ("Magi");
								//unitClass = GameObject.Find ("BattlemaidenClass").GetComponent<UnitClass>();
								break;
						}
				case BattleControl.UnitTypes.Pirate:
						{
								_obj = GameObject.Find ("Pirate");
								//unitClass = GameObject.Find ("BattlemaidenClass").GetComponent<UnitClass>();
								break;
						}
				case BattleControl.UnitTypes.Scout:
						{
								_obj = GameObject.Find ("Scout");
								//unitClass = GameObject.Find ("BattlemaidenClass").GetComponent<UnitClass>();
								break;
						}
				case BattleControl.UnitTypes.Valkyrie:
						{
								_obj = GameObject.Find ("Valkyrie");
								//unitClass = GameObject.Find ("BattlemaidenClass").GetComponent<UnitClass>();
								break;
						}
				case BattleControl.UnitTypes.Wark:
						{
								_obj = GameObject.Find ("Wark");
								//unitClass = GameObject.Find ("BattlemaidenClass").GetComponent<UnitClass>();
								isLarge = true;
								break;
						}
				case BattleControl.UnitTypes.Witch:
						{
								_obj = GameObject.Find ("Witch");	 
								//unitClass = GameObject.Find ("BattlemaidenClass").GetComponent<UnitClass>();
								break;
						}
				case BattleControl.UnitTypes.None:
						{
								isType = false;
								break;
						}
				default:
						{
								isType = false;
								break;
						}
				}
				//clone object and set up position, depth, rotation
				if (isType) {
						obj = (GameObject)GameObject.Instantiate(_obj);
						ani = obj.GetComponents<Animator>();
						
						//if (isLarge)
						//		_unitPos.transform.position += new Vector3 (0f, 1f, 0f);

						obj.transform.position = _unitPos.transform.position;
						fixedPosition = obj.transform.position;

						if (isEnemy){
								obj.transform.localScale = new Vector3(obj.transform.localScale.x*-1,
				    				                                   obj.transform.localScale.y,
				    		            		                       obj.transform.localScale.z);

						}
						

				}	
	
	}
    */
    void Start()
    {
        InvokeRepeating("TakeDamage", 2f, 2f);
    }

    // Update is called once per frame
    void Update () {
        
		if (isAttackStart)
		{
			if (!isAttackFinish)
			{
				ani [0].SetTrigger ("Hit");
				isAttackFinish = true;
			}
//			if (isEnemy)obj.transform.position += new Vector3(0.04f,0f,0f);
//				else obj.transform.position += new Vector3(-0.04f,0f,0f);
//		    if ((obj.transform.position - fixedPosition).magnitude >0.66f)
//			{
//				isAttackStart = false;
//				isAttackFinish = true;
//			}

		}
		if (isAttackFinish)
		{
			timer += Time.deltaTime;
			if(timer > 1f)
			{
				isAttackFinish = false;
				isAttackStart = false;
				timer = 0f;
			}
//			if (isEnemy) obj.transform.position += new Vector3(-0.04f,0f,0f);
//				else obj.transform.position += new Vector3(0.04f,0f,0f);
//			if ((obj.transform.position - fixedPosition).magnitude <= 0.03f)
//			{
//				obj.transform.position = fixedPosition;
//				isAttackFinish = false;
//			}
		}
	}

	public void TakeDamage() {

        int _damage = RollDamage();
        


        int[] digit = new int[3];
        bool isHeal = false;
  
        if (_damage < 0)
        {
			isHeal = true;
			_damage *= -1;
		}

        for (int i = 0; i < 3; i++)
        {
            digit[i] = _damage % 10;
            _damage = (int)_damage / 10;
        }//end loop

        GameObject damageNumberClone;
        DamageNumber dn;
        if (digit [2] != 0) {
            damageNumberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)damageNumberClone.GetComponent<DamageNumber>();
            dn.Create (digit[0],fixedPosition + new Vector3 (0.6f, 0f, 0f),isHeal,0.4f);
            damageNumberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)damageNumberClone.GetComponent<DamageNumber>();
            dn.Create(digit[1],fixedPosition,isHeal,0.2f);
            damageNumberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)damageNumberClone.GetComponent<DamageNumber>();
            dn.Create(digit[2],fixedPosition + new Vector3 (-0.6f, 0f, 0f),isHeal,0f);
				}
		if (digit [2] == 0 && digit [1] != 0) {
            damageNumberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)damageNumberClone.GetComponent<DamageNumber>();
            dn.Create(digit[0],fixedPosition + new Vector3 (0.3f, 0f, 0f),isHeal,0.2f);
            damageNumberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)damageNumberClone.GetComponent<DamageNumber>();
            dn.Create(digit[1],fixedPosition + new Vector3 (-0.3f, 0f, 0f),isHeal,0f);
				}
		if (digit [2] == 0 && digit [1] == 0) {
            damageNumberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)damageNumberClone.GetComponent<DamageNumber>();
            dn.Create(digit[0],fixedPosition,isHeal,0f);
		}
	}

	public int RollDamage() {

        return (int)Random.Range(-100, 999);
	}


}


