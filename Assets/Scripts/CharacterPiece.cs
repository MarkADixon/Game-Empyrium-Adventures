using UnityEngine;
using System.Collections;


//unit is responsible for graphical representation of the character and animations on unit prefab
public class CharacterPiece : MonoBehaviour
{
    public bool isAttackStart = false;
    public bool isAttackFinish = false;
    Vector3 fixedPosition = new Vector3(0f, 0f, 0f);
    public int init = 0;
    public bool isClicked = false;


    CharacterSheet sheet;

    GameObject characterDisplay,allyDisplay,enemyDisplay;
    UnityEngine.UI.Text characterName_Text;
    UnityEngine.UI.Image characterHead_Image;
    UnityEngine.UI.Image squadLocation_Image;
    UnityEngine.UI.Image healthFill_Image;
    UnityEngine.UI.Image armorFill_Image;
    UnityEngine.UI.Image shieldFill_Image;

    public GameObject damageNumber,allyCharacterUI,enemyCharacterUI;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        


    }

    public void Initialize(CharacterSheet _sheet, int index)
    {
        sheet = _sheet;
        RectTransform trans;
        int squadindex;
        if(sheet.isPlayer)
        {
            allyDisplay = GameObject.Find("AllyDisplay");
            characterDisplay = (GameObject)Instantiate(allyCharacterUI);
            characterDisplay.transform.SetParent(allyDisplay.transform);
            trans = characterDisplay.GetComponent<RectTransform>();
            trans.anchorMin = Vector2.one;
            trans.anchorMax = Vector2.one;
            characterDisplay.transform.localPosition = new Vector3(110, -20 - (index*48f));
            squadindex = sheet.squadLocX + (sheet.squadLocY * 5);
        }
        else
        {
            enemyDisplay = GameObject.Find("EnemyDisplay");
            characterDisplay = (GameObject)Instantiate(enemyCharacterUI);
            characterDisplay.transform.SetParent(enemyDisplay.transform);
            trans = characterDisplay.GetComponent<RectTransform>();
            trans.anchorMin = new Vector2(0,1f);
            trans.anchorMax = new Vector2(0,1f);
            characterDisplay.transform.localPosition = new Vector3(130, -20 - (index * 48f));
            squadindex = (4- sheet.squadLocX) + (sheet.squadLocY * 5);
        }

        foreach(Transform child in trans)
        {
            if(child.name == "CharacterNameText")
            {
                characterName_Text = child.gameObject.GetComponent<UnityEngine.UI.Text>();
            }
            else if(child.name == "HealthIconFill")
            {
                healthFill_Image = child.gameObject.GetComponent<UnityEngine.UI.Image>();
            }
            else if(child.name == "ArmorIconFill")
            {
                armorFill_Image = child.gameObject.GetComponent<UnityEngine.UI.Image>();
            }
            else if(child.name == "ShieldIconFill")
            {
                shieldFill_Image = child.gameObject.GetComponent<UnityEngine.UI.Image>();
            }
            else if(child.name == "CharacterHeadImage")
            {
                characterHead_Image = child.gameObject.GetComponent<UnityEngine.UI.Image>();
            }
            else if(child.name == "SquadLocationImage")
            {
                squadLocation_Image = child.gameObject.GetComponent<UnityEngine.UI.Image>();
            }
        }

        characterName_Text.text = sheet.characterName + " - " + sheet.characterClass + " Lvl " + sheet.level;
       
        squadLocation_Image.sprite = DataManager.DM.grid_medium[squadindex];
        characterHead_Image.sprite = DataManager.DM.character_heads[0];
        UpdateUI();
    }

    public void UpdateUI()
    {
        if(sheet.stats.maxHealth != 0)
        {
            healthFill_Image.fillAmount = (float) sheet.stats.health / (float)sheet.stats.maxHealth;
        }
        else
        {
            healthFill_Image.fillAmount = 0f;
        }
        if(sheet.stats.maxArmor != 0)
        {
            armorFill_Image.fillAmount = (float)sheet.stats.armor / (float)sheet.stats.maxArmor;
        }
        else
        {
            armorFill_Image.fillAmount = 0f;
        }
        if(sheet.stats.maxShield != 0)
        {
            shieldFill_Image.fillAmount = (float)sheet.stats.shield / (float)sheet.stats.maxShield;
        }
        else
        {
            shieldFill_Image.fillAmount = 0f;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnMouseDown()
    {
        isClicked = true;
    }


    public void Animate_Attack()
    {
        anim.Play("characterPieceAttack");

    }
    public void Animate_Idle()
    {
        anim.Play("characterPiece_Idle");
    }

    public void ShowNumber(int _damage, Color color)
    {

        fixedPosition = gameObject.GetComponent<Transform>().transform.position;


        int[] digit = new int[4];

        for(int i = 0; i < 4; i++)
        {
            digit[i] = _damage % 10;
            _damage = (int)_damage / 10;
        }//end loop

        GameObject numberClone;
        DamageNumber dn;
        if(digit[3]!=0)
        {
            numberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)numberClone.GetComponent<DamageNumber>();
            dn.Create(digit[0], fixedPosition + new Vector3(0.9f, 0f, 0f), color, 0.4f);
            numberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)numberClone.GetComponent<DamageNumber>();
            dn.Create(digit[1], fixedPosition + new Vector3(0.3f, 0f, 0f), color, 0.2f);
            numberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)numberClone.GetComponent<DamageNumber>();
            dn.Create(digit[2], fixedPosition + new Vector3(-0.3f, 0f, 0f), color, 0f);
            numberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)numberClone.GetComponent<DamageNumber>();
            dn.Create(digit[3], fixedPosition + new Vector3(-0.9f, 0f, 0f), color, 0f);
        }
        if(digit[3] == 0 && digit[2] != 0)
        {
            numberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)numberClone.GetComponent<DamageNumber>();
            dn.Create(digit[0], fixedPosition + new Vector3(0.6f, 0f, 0f), color, 0.4f);
            numberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)numberClone.GetComponent<DamageNumber>();
            dn.Create(digit[1], fixedPosition, color, 0.2f);
            numberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)numberClone.GetComponent<DamageNumber>();
            dn.Create(digit[2], fixedPosition + new Vector3(-0.6f, 0f, 0f), color, 0f);
        }
        if(digit[3] == 0 && digit[2] == 0 && digit[1] != 0)
        {
            numberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)numberClone.GetComponent<DamageNumber>();
            dn.Create(digit[0], fixedPosition + new Vector3(0.3f, 0f, 0f), color, 0.2f);
            numberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber)numberClone.GetComponent<DamageNumber>();
            dn.Create(digit[1], fixedPosition + new Vector3(-0.3f, 0f, 0f), color, 0f);
        }
        if(digit[3] == 0 && digit[2] == 0 && digit[1] == 0)
        {
            numberClone = (GameObject)Instantiate(damageNumber, Vector3.one, Quaternion.identity);
            dn = (DamageNumber) numberClone.GetComponent<DamageNumber>();
            dn.Create(digit[0], fixedPosition, color, 0f);
        }
    }

    


}
