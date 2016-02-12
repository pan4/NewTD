using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using FThLib;

public class KT_Controller : TowerController {

	public Sprite lvl2;
	public Sprite block;
	//--Private
	private float instancetime = 11f;//######################### get
	private GameObject spawner=null;
	private GameObject flag=null;
	private GameObject zone=null;
	//--About door
	private bool inprocess=false;
	private bool firstime=true;
	private int a=0;
	//About knights

	public bool shield =false;

    [SerializeField]
    private string _unitPath = "Kt/Knight";
    private const string _interface = "KT0";

	// Use this for initialization

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.name=="Knight1"||coll.name=="Knight2"||coll.name=="Knight3"){
			StartCoroutine(setZ(coll.gameObject, 1f));
		}
	}
	private void Init_()
    {
		spawner = master.getChildFrom("spawner",this.gameObject);
		flag = master.getChildFrom("flag",this.gameObject);
		zone = master.getChildFrom("zone",this.gameObject);
	}
	void Start () {
		this.transform.position = master.setThisZ(this.transform.position,0.02f);
		master.setLayer("tower",this.gameObject);
		setFlagZ();
		Init_();
	}

	protected override void OnUpdate ()
    {
        base.OnUpdate();
		if(!master.isFinish())
        {
			if(inprocess==false){knightCall();}//progressbar included
			remove_null();
			if(EnemiesInZone.Count>0){getEnemy();}//If enemy on area and no fighting, call a knight
		}
	}

	void getEnemy(){
		for(int i=0; i< EnemiesInZone.Count ;i++){
			if(EnemiesInZone[i]!=null){
				PathFollower enemyProperties = EnemiesInZone[i].GetComponent<PathFollower>();
				if (enemyProperties.fighting==false){
					enemyProperties.target=getKnight(EnemiesInZone[i].gameObject);
					if(enemyProperties.target!=null){//if get a knight set enemy to fighting
						enemyProperties.fighting=true;
					}else{
						enemyProperties.fighting=false;
					}
				}
			}
		}
	}

	GameObject getKnight(GameObject target){//Knight stoped and no fighting
		GameObject aux = null;
		Knights_Controller k1properties;
		Knights_Controller k2properties;
		Knights_Controller k3properties;
		bool k1 =false;
		bool k2 =false;
		bool k3 =false;
		if(master.getChildFrom ("Knight1",this.gameObject)!=null){
			k1properties = master.getChildFrom("Knight1",this.gameObject).GetComponent<Knights_Controller>();
			k1 = knightCanFight("Knight1", target);
		}
		if(master.getChildFrom ("Knight2",this.gameObject)!=null&&k1==false){
			k2properties = master.getChildFrom("Knight2",this.gameObject).GetComponent<Knights_Controller>();
			k2 = knightCanFight("Knight2", target);
		}
		if(master.getChildFrom ("Knight3",this.gameObject)!=null&&k1==false&&k2==false){
			k3properties = master.getChildFrom("Knight3",this.gameObject).GetComponent<Knights_Controller>();
			k3 = knightCanFight("Knight3", target);
		}
		if(k1 == true){
			aux = master.getChildFrom ("Knight1",this.gameObject);
		}else{
			if(k2==true){
				aux = master.getChildFrom ("Knight2",this.gameObject);
			}else{
				if(k3==true){aux = master.getChildFrom ("Knight3",this.gameObject);}
			}
		}
		return aux;
	}


	public override void setDamage(){
		if(master.getChildFrom("Knight1",this.gameObject)){
			master.getChildFrom("Knight1",this.gameObject).GetComponent<Knights_Controller>().damage = Damage[_level];
		}
		if(master.getChildFrom("Knight2",this.gameObject)){
			master.getChildFrom("Knight2",this.gameObject).GetComponent<Knights_Controller>().damage = Damage[_level]; 
		}
		if(master.getChildFrom("Knight3",this.gameObject)){
			master.getChildFrom("Knight3",this.gameObject).GetComponent<Knights_Controller>().damage = Damage[_level]; 
		}
	}

    public override void SetAttackSpeed()
    {
        if (master.getChildFrom("Knight1", this.gameObject))
        {
            master.getChildFrom("Knight1", this.gameObject).GetComponent<Knights_Controller>().AttackDelay = 1f / AttackSpeed[_level];
        }
        if (master.getChildFrom("Knight2", this.gameObject))
        {
            master.getChildFrom("Knight2", this.gameObject).GetComponent<Knights_Controller>().AttackDelay = 1f / AttackSpeed[_level];
        }
        if (master.getChildFrom("Knight3", this.gameObject))
        {
                master.getChildFrom("Knight3", this.gameObject).GetComponent<Knights_Controller>().AttackDelay = 1f / AttackSpeed[_level];
        }
    }

    public override void setShield(){
		shield = true;
		if(master.getChildFrom("Knight1",this.gameObject)){
			master.getChildFrom("Knight1",this.gameObject).GetComponent<Knights_Controller>().shield=true;
			master.getChildFrom("Knight1",this.gameObject).GetComponent<Knights_Controller>().resetLife(life);
		}
		if(master.getChildFrom("Knight2",this.gameObject)){
			master.getChildFrom("Knight2",this.gameObject).GetComponent<Knights_Controller>().shield=true;
			master.getChildFrom("Knight2",this.gameObject).GetComponent<Knights_Controller>().resetLife(life);
		}
		if(master.getChildFrom("Knight3",this.gameObject)){
			master.getChildFrom("Knight3",this.gameObject).GetComponent<Knights_Controller>().shield=true;
			master.getChildFrom("Knight3",this.gameObject).GetComponent<Knights_Controller>().resetLife(life);
		}
	}

	public override void Reset(){
		master.getChildFrom("TargetedZone",this.gameObject).transform.position = flag.transform.position;
		if(EnemiesInZone.Count>0){
			for(int i=0; i< EnemiesInZone.Count ;i++){
				PathFollower enemyProperties = EnemiesInZone[i].GetComponent<PathFollower>();
				if (enemyProperties.fighting==true){
					enemyProperties.target=null;
					enemyProperties.fighting=false;
				}
				EnemyRemove(EnemiesInZone[i]);
			}
		}
		if(master.getChildFrom("Knight1",this.gameObject)){
			Knights_Controller properties = master.getChildFrom("Knight1",this.gameObject).GetComponent<Knights_Controller>();
			if(properties.fighting==true&&properties.target!=null){
				properties.target.GetComponent<PathFollower>().fighting=false;
				properties.target.GetComponent<PathFollower>().target = null;
			}
			properties.fighting=false;
			properties.target=null;
		}
		if(master.getChildFrom("Knight2",this.gameObject)){
			Knights_Controller properties = master.getChildFrom("Knight2",this.gameObject).GetComponent<Knights_Controller>();
			if(properties.fighting==true&&properties.target!=null){
				properties.target.GetComponent<PathFollower>().fighting=false;
				properties.target.GetComponent<PathFollower>().target = null;
			}
			properties.fighting=false;
			properties.target=null;
		}
		if(master.getChildFrom("Knight3",this.gameObject)){
			Knights_Controller properties = master.getChildFrom("Knight3",this.gameObject).GetComponent<Knights_Controller>();
			if(properties.fighting==true&&properties.target!=null){
				properties.target.GetComponent<PathFollower>().fighting=false;
				properties.target.GetComponent<PathFollower>().target = null;
			}
			properties.fighting=false;
			properties.target=null;
		}
	}

	// Update is called once per frame
	bool knightCanFight(string name, GameObject target){
		bool aux = false;
		Knights_Controller kproperties = master.getChildFrom(name,this.gameObject).GetComponent<Knights_Controller>();
		if(kproperties.fighting==false){
			kproperties.fighting=true;
			kproperties.target=target;
			kproperties.move=true;
			aux = true;
		}
		return aux;
	}

	private void knightCall(){//Instantiate
		if(master.getChildFrom("Knight1",this.gameObject)&&master.getChildFrom("Knight2",this.gameObject)&&master.getChildFrom("Knight3",this.gameObject)){
			firstime=false;
		}else{
			if(firstime==true){//Fist time respawn is better
				inprocess=true;
				master.Instantiate_Progressbar(4f,this.gameObject);
				Invoke ("Instantiate_Knight",4f);
			}else{
				inprocess=true;
				master.Instantiate_Progressbar(instancetime,this.gameObject);
				Invoke ("Instantiate_Knight",instancetime);
			}

		}
	}

	private void Instantiate_Knight(){
		inprocess=false;
		GameObject Knight = Instantiate(Resources.Load(_unitPath), new Vector3(spawner.transform.position.x,spawner.transform.position.y,spawner.transform.position.y), Quaternion.identity)as GameObject;
		Knight.transform.SetParent(this.gameObject.transform);
		Knights_Controller KnightProperties = Knight.GetComponent<Knights_Controller>();
		KnightProperties.flag=flag;
		KnightProperties.life=life;
		KnightProperties.shield = shield;
		KnightProperties.damage = Damage[_level];
        KnightProperties.AttackDelay = 1f / AttackSpeed[_level];
        Knight.name=getKnightName();
	}

	private string getKnightName(){
		string aux_ = "";
		if(master.getChildFrom("Knight1",this.gameObject)){
			if(master.getChildFrom("Knight2",this.gameObject)){
				if(master.getChildFrom("Knight3",this.gameObject)){
				}else{
					aux_="Knight3";
				}
			}else{
				aux_="Knight2";
			}
		}else{
			aux_="Knight1";
		}
		return aux_;
	}

	private IEnumerator  setZ(GameObject go, float delayTime){
		yield return new WaitForSeconds(delayTime);
		go.transform.position = new Vector3(go.transform.position.x,go.transform.position.y,0f);
	}

	private void setFlagZ()
    {
        master.getChildFrom("flag",this.gameObject).transform.position=new Vector3(master.getChildFrom("flag",this.gameObject).transform.position.x,master.getChildFrom("flag",this.gameObject).transform.position.y,master.getChildFrom("flag",this.gameObject).transform.position.y+0.2f);
    }
}
