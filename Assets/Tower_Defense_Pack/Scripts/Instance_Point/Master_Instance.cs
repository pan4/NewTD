using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FThLib;

public class Master_Instance : MonoBehaviour
{
	//To change the money when kill an enemy go to Enemies_Controller, change the value of moneyWhenKill
	//Public
	public int startWithMoney = 240;//Money when start
	public bool anybuttonclicked = false;
	public float Enemyspeed = 0.4f;//Enemies speed
	//Prices
	public int KT_price = 100;//Knights tower
	public int AT_price = 110;//Archers tower
	public int MT_price = 120;//Magicians tower
	//Tower Upgrades prices
	public int KT_Damage_price = 40;
	public int KT_Shield_price = 70;
	public int AT_Fire_price = 110;
	public int AT_Ratio_price = 100;
	public int AT_Accuracy_price = 70;
	public int MT_Fire_price = 120;
	public bool Finish = false;
	//Private

	private Text money;

    public bool Playing = false;

    void Start ()
    {
		money = GameObject.Find("Money").GetComponent<Text>();
		addMoney(startWithMoney);
	}
	
	void Update ()
    {
		if(Finish == false)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (GameObject.Find("Interface"))
                {
                    Destroy (GameObject.Find("Interface"));
                }
            }

			if(Input.GetMouseButtonDown(0))
            {
				if(GameObject.Find("Circle") == null && GameObject.Find("flag_") == null && GameObject.Find("hand") == null && GameObject.Find("trap_") == null)
                {
					if(GameObject.Find("Interface"))
                    {
                        Destroy (GameObject.Find("Interface"));
                    }
				}
			}
		}
	}

    public void OnPlay()
    {
        Destroy(GameObject.Find("pressSpace"));
        Playing = true;
    }
	
	//About money
	public int getPrice(GameObject go)
    {
		int aux_ = 0;
		if(go.name=="KT"||go.name=="KT0"){aux_=KT_price;}
		if(go.name=="AT"||go.name=="AT0"){aux_=AT_price;}
		if(go.name=="MT"||go.name=="MT0"){aux_=MT_price;}
		if(go.name=="Damage"){aux_=KT_Damage_price;}
		if(go.name=="Life"){aux_=KT_Shield_price;}
		if(go.name=="MTFire"){aux_=MT_Fire_price;}
		if(go.name=="Fire"){aux_=AT_Fire_price;}
		if(go.name=="Ratio"){aux_=AT_Ratio_price;}
		if(go.name=="Accuracy"){aux_=AT_Accuracy_price;}
		return aux_;
	}

	public int countMoney()
    {
        return int.Parse (money.text);
    }

	public void addMoney(int value)
    {
		int valueaux = int.Parse (money.text);
		valueaux = valueaux + value;
		money.text = "" + valueaux;
	}

	public void removeMoney(int value)
    {
		int valueaux = int.Parse (money.text);
		valueaux = valueaux - value;
		money.text = "" + valueaux;
	}
}
