using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using FThLib;
//It contains the controller of mage in tower
public class MT_Controller : TowerController {

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

	//Public properties
	public bool fire = false;
	public bool ice = false;
	public bool trap = false;

	// Use this for initialization
	void Start () {
		Init ();
	}

	private void Init(){
		//master = GameObject.Find("Master_Instance").GetComponent<Master_Instance>();
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
	protected override void OnUpdate ()
    {
        base.OnUpdate();
		if(!master.isFinish()){
			remove_null();
			if(EnemiesInZone.Count>0){
				if(shot_==false){
					shot_=true;
					Invoke("shot",1 / AttackSpeed[_level]);
				}
			}
		}
	}
	private void shot()
    {
		shot_=false;        
        if (EnemiesInZone.Count > 0 && EnemiesInZone[EnemiesInZone.Count - 1] != null)
        {
            Transform target = EnemiesInZone[EnemiesInZone.Count - 1].transform;
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
		BulletProperties.target = EnemiesInZone[EnemiesInZone.Count-1].gameObject;
		//BulletProperties.maxLaunch = getminSpeed((int)master.angle_(spawner.transform.position,enemies[0].transform.position));
		//BulletProperties.accuracy_mode=accuracy_mode;
		BulletProperties.fire = fire;
		BulletProperties.ice = ice;
		BulletProperties.Damage = Damage[_level];
		Bullet.name=name;
		//--
	}

	void Flip(){//only mage
		faceright=!faceright;
		Vector3 theScale = mage.transform.localScale;
		theScale.x *= -1;
		mage.transform.localScale = theScale;
	}
}
