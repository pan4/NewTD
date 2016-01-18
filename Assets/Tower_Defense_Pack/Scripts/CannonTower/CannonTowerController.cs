using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using FThLib;

public class CannonTowerController : TowerController {
	//Public
	public List<GameObject> enemies;
	public Sprite lvl2;
	public Sprite block;
	//--Private
	private int towerlvl = 0;
	private float instancetime = 11f;//######################### get
	private GameObject door=null;
	private GameObject opened=null;
	private GameObject closed=null;
	private GameObject spawner=null;
	private GameObject flag=null;
	private GameObject zone=null;
	private bool mouseover=false;
	//--About door
	private bool opening=false;
	private bool closing=false;
	private bool inprocess=false;
	private bool firstime=true;
	private int a=0;
	//About knights

	public bool shield =false;

    [SerializeField]
    private string _unitPath = "Kt/Knight";
    private const string _interface = "KT0";

	// Use this for initialization
	void OnMouseOver(){ 
		if(!GameObject.Find("hand")){master.showHand (true);}
		mouseover=true;
	}
	
	void OnMouseExit(){
		if(GameObject.Find("hand")){master.showHand (false);}
		mouseover=false;
	}
	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.name=="Cannon"||coll.name=="Knight2"||coll.name=="Knight3"){
			StartCoroutine(setZ(coll.gameObject, 1f));
		}
	}
	private void Init_(){
		door = master.getChildFrom("door",this.gameObject);
		opened = master.getChildFrom("opened",this.gameObject);
		closed = master.getChildFrom("closed",this.gameObject);
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

	void Update () {
		if(!master.isFinish()){
			if(master.getChildFrom("Interface",this.gameObject)==null&&!GameObject.Find("circle")){
				master.getChildFrom("zoneImg",this.gameObject).GetComponent<SpriteRenderer>().enabled=false;
				GetComponent<CircleCollider2D>().enabled=true;
			}
			if (Input.GetMouseButtonDown(0)&&mouseover==true){
				master.showInterface(_interface,this.gameObject,zone.transform);
				GetComponent<CircleCollider2D>().enabled=false;
				master.getChildFrom("zoneImg",this.gameObject).GetComponent<SpriteRenderer>().enabled=true;
			}
			if(inprocess==false){knightCall();}//progressbar included
			doorisdoing();
			remove_null();
			if(enemies.Count>0){getEnemy();}//If enemy on area and no fighting, call a knight
		}
	}

	void getEnemy(){
		for(int i=0; i<enemies.Count ;i++){
			if(enemies[i]!=null){
				PathFollower enemyProperties = enemies[i].GetComponent<PathFollower>();
				if (enemyProperties.fighting==false){
					enemyProperties.target=getKnight(enemies[i]);
					if(enemyProperties.target!=null){//if get a knight set enemy to fighting
						enemyProperties.fighting=true;
					}else{
						enemyProperties.fighting=false;
					}
				}
			}
		}
	}

	GameObject getKnight(GameObject target)
    {//Knight stoped and no fighting
        GameObject aux = null;
		bool k1 = false;
		if(master.getChildFrom ("Cannon",this.gameObject)!=null){
			k1 = knightCanFight("Cannon", target);
		}
		if(k1 == true){
			aux = master.getChildFrom ("Cannon",this.gameObject);
		}
		return aux;
	}


	public override void setDamage(){
		if(master.getChildFrom("Cannon",this.gameObject)){
			master.getChildFrom("Cannon",this.gameObject).GetComponent<CannonController>().damage=damage;
		}
	}

	public override void setShield(){
		shield = true;

		if(master.getChildFrom("Cannon",this.gameObject)){
			master.getChildFrom("Cannon",this.gameObject).GetComponent<CannonController>().shield=true;
			master.getChildFrom("Cannon",this.gameObject).GetComponent<CannonController>().resetLife(life);
		}
	}

	public override void Reset(){
		master.getChildFrom("TargetedZone",this.gameObject).transform.position = flag.transform.position;
		if(enemies.Count>0){
			for(int i=0; i<enemies.Count ;i++){
				PathFollower enemyProperties = enemies[i].GetComponent<PathFollower>();
				if (enemyProperties.fighting==true){
					enemyProperties.target=null;
					enemyProperties.fighting=false;
				}
				enemyRemove(enemies[i].name);
			}
		}
		if(master.getChildFrom("Cannon",this.gameObject)){
			CannonController properties = master.getChildFrom("Cannon",this.gameObject).GetComponent<CannonController>();
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
		CannonController kproperties = master.getChildFrom(name,this.gameObject).GetComponent<CannonController>();
		if(kproperties.fighting==false){
			kproperties.fighting=true;
			kproperties.target=target;
			kproperties.move=true;
			aux = true;
		}
		return aux;
	}

	private void knightCall(){//Instantiate
		if(master.getChildFrom("Cannon",this.gameObject)){
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
		GameObject Knight = Instantiate(Resources.Load(_unitPath), spawner.transform.position, Quaternion.identity)as GameObject;
		Knight.transform.SetParent(this.gameObject.transform);
		opening = true;
		CannonController KnightProperties = Knight.GetComponent<CannonController>();
		KnightProperties.flag=flag;
		KnightProperties.life=life;
		KnightProperties.shield = shield;
		KnightProperties.damage=damage;
		Knight.name=getKnightName();
	}

	private string getKnightName(){
		string aux_ = "Cannon";
        return aux_;
    }

	private IEnumerator  setZ(GameObject go, float delayTime){
		yield return new WaitForSeconds(delayTime);
		go.transform.position = new Vector3(go.transform.position.x,go.transform.position.y,0f);
	}

	//About list
	void remove_null(){for(int i=0; i<enemies.Count ;i++){if(enemies[i]==null){enemies.RemoveAt(i);}}}
	public override void enemyAdd(GameObject other){enemies.Add (other);}
	public override void enemyRemove(string other){
		for(int i=0; i<enemies.Count ;i++){
			if(enemies[i]!=null){
				if(enemies[i].name==other){enemies.RemoveAt(i);}
			}
		}
	}
	//###############--About door
	private void doorisdoing(){
		if(opening==true){
			getOpen (0);
		}else{
			if(closing==true){getOpen(1);}
		}
	}	
	private void getOpen(int value){//0 open, 1 close
		switch(value){
		case 0:
			if(door.transform.position != opened.transform.position){
				door.transform.position = Vector3.MoveTowards(door.transform.position, opened.transform.position, Time.deltaTime/4);
			}else{
				opening=false;
				Invoke("setclosing",2);
			}
			break;
		case 1:
			if(door.transform.position != closed.transform.position){
				door.transform.position = Vector3.MoveTowards(door.transform.position, closed.transform.position, Time.deltaTime/4);
			}else{
				closing=false;
			}
			break;
		}
	}

	private void setFlagZ()
    {
        Transform flag = transform.FindChild("flag");
        flag.position = flag.position + Vector3.up * 0.2f;
    }

    private void setclosing()
    {
        closing = true;
    }

}
