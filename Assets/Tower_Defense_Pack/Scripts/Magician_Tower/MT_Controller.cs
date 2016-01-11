using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using FThLib;
//It contains the controller of mage in tower
public class MT_Controller : MonoBehaviour {
	//--Public
	public List<GameObject> enemies;
	public Sprite block;
	//public Sprite trapbtn;
	//--Private
	private GameObject mage=null;
	private bool faceright = false;
	private Animator anim;
	//private Master_Instance master;
	private GameObject zone=null;
	private GameObject spawner=null;
	private GameObject spawner1=null;
	private GameObject spawner2=null;
	private bool shot_ = false;
	private bool mouseover=false;
	//Public properties
	public float s_timer = 0.9f;
	public int Damage_ = 1;
	public bool fire = false;
	public bool ice = false;
	public bool trap = false;

	void OnMouseOver(){ 
		if(!GameObject.Find("hand")){master.showHand (true);}
		mouseover=true;
	}
	
	void OnMouseExit(){
		if(GameObject.Find("hand")){master.showHand (false);}
		mouseover=false;
	}
	// Use this for initialization
	void Start () {
		Init ();
	}

	private void Init(){
		//master = GameObject.Find("Instance_Point").GetComponent<Master_Instance>();
		this.transform.position = master.setThisZ(this.transform.position,0.02f);
		spawner = master.getChildFrom("spawner",this.gameObject);
		spawner1 = master.getChildFrom("spawner1",this.gameObject);
		spawner2 = master.getChildFrom("spawner2",this.gameObject);
		zone = master.getChildFrom("zone",this.gameObject);
		master.setLayer("tower",this.gameObject);
		//About mage in tower
		mage = master.getChildFrom("Mage",this.gameObject);
		anim = mage.GetComponent<Animator> ();
		anim.SetBool ("attack", false);
	}
	
	// Update is called once per frame
	void Update () {
		if(!master.isFinish()){
			if(master.getChildFrom("Interface",this.gameObject)==null&&!GameObject.Find("circle")){
				master.getChildFrom("zoneImg",this.gameObject).GetComponent<SpriteRenderer>().enabled=false;
				GetComponent<CircleCollider2D>().enabled=true;
			}
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Attack")) {anim.SetBool ("attack", false);}
			if (Input.GetMouseButtonDown(0)&&mouseover==true){
				master.showInterface(this.gameObject.name,this.gameObject,zone.transform);
				GetComponent<CircleCollider2D>().enabled=false;
				master.getChildFrom("zoneImg",this.gameObject).GetComponent<SpriteRenderer>().enabled=true;
			}
			remove_null();
			if(enemies.Count>0){
				if(shot_==false){
					shot_=true;
					Invoke("shot",s_timer);
				}
			}
		}
	}
	private void shot()
    {
		shot_=false;        
        if (enemies.Count > 0 && enemies[enemies.Count - 1] != null)
        {
            Transform target = enemies[enemies.Count - 1].transform;
            ShotAnimation(anim, target);
            Instantiate_Bullet(anim.transform, "Magic");
		}
	}

    private void ShotAnimation(Animator archer, Transform target)
    {
        Vector2 targetPos = new Vector2(target.position.x, target.position.y);
        Vector2 archerPos = new Vector2(archer.transform.position.x, archer.transform.position.y);

        Vector2 direction = (targetPos - archerPos).normalized;

        archer.SetFloat("AttackDirectionX", direction.x);
        archer.SetFloat("AttackDirectionY", direction.y);
        archer.SetTrigger("AttackTrigger");
    }

 //   private void InstantateWithDelay(){
	//	Instantiate_Bullet(spawner, "Magic");
	//}

	private void Instantiate_Bullet(Transform pos, string name)
    {
		GameObject Bullet = Instantiate(Resources.Load("MT/Mfire"), pos.position, Quaternion.identity)as GameObject;
		MT_Bullet BulletProperties = Bullet.GetComponent<MT_Bullet>();
		//############# Bullet properties --
		BulletProperties.target = enemies[enemies.Count-1];
		//BulletProperties.maxLaunch = getminSpeed((int)master.angle_(spawner.transform.position,enemies[0].transform.position));
		//BulletProperties.accuracy_mode=accuracy_mode;
		BulletProperties.fire = fire;
		BulletProperties.ice = ice;
		BulletProperties.Damage_ = Damage_;
		Bullet.name=name;
		//--
	}
	//--About enemy list
	public void enemyAdd(GameObject other){enemies.Add (other);}
	public void enemyRemove(string other){
		for(int i=0; i<enemies.Count ;i++){
			if(enemies[i]!=null){
				if(enemies[i].name==other){enemies.RemoveAt(i);}
			}
		}
	}
	void remove_null(){for(int i=0; i<enemies.Count ;i++){if(enemies[i]==null){enemies.RemoveAt(i);}}}
	void Flip(){//only mage
		faceright=!faceright;
		Vector3 theScale = mage.transform.localScale;
		theScale.x *= -1;
		mage.transform.localScale = theScale;
	}
}
