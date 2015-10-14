using UnityEngine;
using System.Collections;


//unit is responsible for graphical representation of the character and animations on unit prefab
public class CharacterPiece : MonoBehaviour
{
    public bool isAttackStart = false;
    public bool isAttackFinish = false;
    Vector3 fixedPosition = new Vector3(0f, 0f, 0f);
    public int init = 0;

    public GameObject damageNumber;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(isAttackStart)
        {
            //if(!isAttackFinish)
            // {
            
            //    isAttackFinish = true;
            //}
            gameObject.transform.position += new Vector3(0.04f, 0f, 0f);
            if((gameObject.transform.position - fixedPosition).magnitude > 0.66f)
            {
                isAttackStart = false;
                isAttackFinish = true;
            }

        }
        if(isAttackFinish)
        {
            timer += Time.deltaTime;
            if(timer > 1f)
            {
                isAttackFinish = false;
                isAttackStart = false;
                timer = 0f;
            }
            else
            {
                gameObject.transform.position += new Vector3(-0.04f, 0f, 0f);
                if((gameObject.transform.position - fixedPosition).magnitude <= 0.03f)
                {
                    gameObject.transform.position = fixedPosition;
                    isAttackFinish = false;
                }
            }
        }
        */
    }
    public void Animate_Attack()
    {
        anim.Play("characterPieceAttack");

    }
    public void Animate_Idle()
    {
        anim.Play("characterPiece_Idle");
    }
    public void ShowDamage(int _damage)
    {

        fixedPosition = gameObject.GetComponent<Transform>().transform.position;


        int[] digit = new int[3];
        bool isHeal = false;

        if(_damage < 0)
        {
            isHeal = true;
            _damage *= -1;
        }

        for(int i = 0; i < 3; i++)
        {
            digit[i] = _damage % 10;
            _damage = (int)_damage / 10;
        }//end loop

        GameObject damageNumberClone;
        DamageNumber dn;
        if(digit[2] != 0)
        {
            damageNumberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)damageNumberClone.GetComponent<DamageNumber>();
            dn.Create(digit[0], fixedPosition + new Vector3(0.6f, 0f, 0f), isHeal, 0.4f);
            damageNumberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)damageNumberClone.GetComponent<DamageNumber>();
            dn.Create(digit[1], fixedPosition, isHeal, 0.2f);
            damageNumberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)damageNumberClone.GetComponent<DamageNumber>();
            dn.Create(digit[2], fixedPosition + new Vector3(-0.6f, 0f, 0f), isHeal, 0f);
        }
        if(digit[2] == 0 && digit[1] != 0)
        {
            damageNumberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)damageNumberClone.GetComponent<DamageNumber>();
            dn.Create(digit[0], fixedPosition + new Vector3(0.3f, 0f, 0f), isHeal, 0.2f);
            damageNumberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)damageNumberClone.GetComponent<DamageNumber>();
            dn.Create(digit[1], fixedPosition + new Vector3(-0.3f, 0f, 0f), isHeal, 0f);
        }
        if(digit[2] == 0 && digit[1] == 0)
        {
            damageNumberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)damageNumberClone.GetComponent<DamageNumber>();
            dn.Create(digit[0], fixedPosition, isHeal, 0f);
        }
    }

    


}
