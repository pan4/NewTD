using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using FThLib;

public class AT_Controller : TowerController {

	public Sprite block;
	//--Private
	private GameObject zone=null;
	private GameObject spawner=null;
	public bool shot_ = false;
	private bool mouseover=false;
	private float searchvalue = 0.1f;// down value is best... but the performance may be affected (used to detect min speed to hit the enemy)
	//Public properties
	public float s_timer = 0.9f;
	public int accuracy_mode = 4;//Bassically it is used by the bullet, and add a force in direction to the target.
	public bool fire = false;

    private Animator _archerAnimator1;
    private Animator _archerAnimator2;

    void OnMouseOver(){ 
		if(!GameObject.Find("hand")){master.showHand (true);}
		mouseover=true;
	}

	void OnMouseExit(){
		if(GameObject.Find("hand")){master.showHand (false);}
		mouseover=false;
    }

	void Start () {
		this.transform.position = master.setThisZ(this.transform.position,0.02f);
		spawner = master.getChildFrom("spawner",this.gameObject);
		zone = master.getChildFrom("zone",this.gameObject);
		master.setLayer("tower",this.gameObject);

        _archerAnimator1 = transform.FindChild("Archer1").GetComponent<Animator>();
        _archerAnimator2 = transform.FindChild("Archer2").GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update () {
		if(!master.isFinish()){
			if(master.getChildFrom("Interface",this.gameObject)==null){
				master.getChildFrom("zoneImg",this.gameObject).GetComponent<SpriteRenderer>().enabled=false;
				GetComponent<CircleCollider2D>().enabled=true;
			}
			if (Input.GetMouseButtonDown(0)&&mouseover==true){
					master.showInterface(this.gameObject.name,this.gameObject,zone.transform);
					GetComponent<CircleCollider2D>().enabled=false;
					master.getChildFrom("zoneImg",this.gameObject).GetComponent<SpriteRenderer>().enabled=true;
			}
			remove_null();
			if(EnemiesInZone.Count>0){
				if(shot_==false){
					shot_=true;
					Invoke("shot",s_timer);
				}
			}
		}
	}

    private int _archersOrder = 0;

    private void shot()
    {
        shot_ = false;

        if (EnemiesInZone.Count == 0)
            return;

        if (_archersOrder % 2 == 0)
        {
            if (EnemiesInZone[EnemiesInZone.Count - 1] != null)
            {
                Transform target = EnemiesInZone[EnemiesInZone.Count - 1].transform;
                ShotAnimation(_archerAnimator1, target);
                Instantiate_Bullet(_archerAnimator1.transform, EnemiesInZone[EnemiesInZone.Count - 1]);
            }
        }
        else
        {
            if (EnemiesInZone[0] != null)
            {
                Transform target = EnemiesInZone[0];
                ShotAnimation(_archerAnimator2, target);
                Instantiate_Bullet(_archerAnimator2.transform, EnemiesInZone[0]);
            }
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


	private void Instantiate_Bullet(Transform spawner, Transform target){
        GameObject Bullet = Instantiate(Resources.Load("AT/arrow"), spawner.position, Quaternion.identity)as GameObject;
		Parabolic_shot_Controller BulletProperties = Bullet.GetComponent<Parabolic_shot_Controller>();
		Bullet.GetComponent<Damage>().Damage_ = damage;
		//############# Bullet properties --
		BulletProperties.target = target.gameObject;
		if(EnemiesInZone[0]!=null){
			BulletProperties.maxLaunch = getminSpeed((int)master.angle_(spawner.transform.position, EnemiesInZone[0].transform.position));
		}else{
			Destroy(this.gameObject);
		}

		BulletProperties.accuracy_mode=accuracy_mode;
		BulletProperties.fire = fire;
		Bullet.name="Arrow";

        _archersOrder++;
	}
	private float getminSpeed(int angle){
		float aux = 0.1f;
		while(moreSpeed(aux)==true){aux = aux + searchvalue;}
		return aux;
	}

	private bool moreSpeed(float speed)
    {
		bool aux = false;
		float xTarget = EnemiesInZone[0].position.x;
		float yTarget = EnemiesInZone[0].position.y; 
		float xCurrent = transform.position.x;
		float yCurrent = transform.position.y;
		float xDistance = Math.Abs(xTarget - xCurrent);
		float yDistance = yTarget - yCurrent;
		float fireAngle = 1.57075f - (float)(Math.Atan((Math.Pow(speed, 2f)+ Math.Sqrt(Math.Pow(speed, 4f) - 9.8f * (9.8f * Math.Pow(xDistance, 2f) + 2f * yDistance * Math.Pow(speed, 2f) )))/(9.8f * xDistance)));
		float xSpeed = (float)Math.Sin(fireAngle) * speed;
		float ySpeed = (float)Math.Cos(fireAngle) * speed;
		if ((xTarget - xCurrent) < 0f){xSpeed = - xSpeed;}
		if(float.IsNaN(xSpeed)||float.IsNaN(ySpeed)){aux = true;}
		return aux;
	}
}
