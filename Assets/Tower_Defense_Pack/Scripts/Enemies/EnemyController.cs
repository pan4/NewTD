using UnityEngine;
using System.Collections;
using FThLib;
//Show blood when arrow..
//Note, this gameobject has a child rightpoint, his position.z must be==knights.postion.z 
public class EnemyController : MonoBehaviour {
	//public
	public int life=0;
	public float attackDelay = 2f;
	public int moneyWhenKill = 20;//Money when enemy is destroyed
	//private
	private Master_Instance masterPoint;
	private float point=0f;
	private GameObject lifebar = null;
	private bool Attack = false;
	private PathFollower pathFollow;
	private int damage=3;
	private int auxlife=0;
	private Animator anim;

	// Use this for initialization
	void Start () {
		this.gameObject.tag="Respawn";
		Init();
	}

	private void Init(){
		masterPoint = GameObject.Find("Instance_Point").GetComponent<Master_Instance>();
		master.setLayer("enemies",this.gameObject);
		lifebar = master.getChildFrom("Lifebar",this.gameObject);
		getPoint();
		pathFollow = GetComponent<PathFollower>();
		anim = this.gameObject.GetComponent<Animator> ();
		anim.SetBool ("walk", false);
		anim.SetBool ("dead", false);
		anim.SetBool ("attack", false);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.name=="Arrow"){
			reduceLife(other.GetComponent<Damage>().Damage_);
		}
		if(other.name=="Magic"){
			GameObject blood = Instantiate(Resources.Load("Global/blood"), other.transform.position, Quaternion.identity)as GameObject;
			reduceLife(other.GetComponent<MT_Bullet>().Damage_);
		}
	}
	// Update is called once per frame
	void Update () {
		if(!master.isFinish()){
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Attack")) {anim.SetBool ("attack", false);}
			if(point!=0f&&auxlife==0){auxlife = life;}
				
			if(pathFollow.target!=null)
            {

                DefenderController defender = pathFollow.target.GetComponent<DefenderController>();
                if (defender is Knights_Controller)
                {
                    if (pathFollow.auxfight == true)
                    {
                        anim.SetBool("walk", false);
                        if (defender.move == false && Attack == false)
                        {
                            Attack = true;
                            anim.SetBool("attack", true);
                            Invoke("enemyreduceLife", 0.1f);
                            Invoke("attack_delay", attackDelay);
                            defender.reduceLife(damage);
                        }
                    }
                    else
                    {
                        anim.SetBool("walk", true);
                    }
                }
                else if(defender is CannonController)
                {                    
                    if (pathFollow.auxfight == true)
                    {
                        anim.SetBool("walk", false);
                        if (defender.move == false && Attack == false)
                        {
                            Attack = true;
                            anim.SetBool("attack", true);
                            Invoke("enemyreduceLife", 0.1f);
                            Invoke("attack_delay", attackDelay);
                            defender.reduceLife(damage);
                        }
                    }
                    //else
                    //{
                    //    if (Vector3.SqrMagnitude(transform.position - pathFollow.target.transform.position) < 1.5 * 1.5)
                    //        pathFollow.MoveTarget.position = pathFollow.target.transform.position - Vector3.right * 0.4f;
                    //    anim.SetBool("walk", true);
                    //}

                }
			}
            else
                anim.SetBool("walk", true);
        }
	}

	private void attack_delay(){Attack=false;}

	public void increaseLife(int value){
		float aux = value * point;
		Vector3 aux_ = lifebar.transform.localScale;
		aux_.x = aux_.x + aux;
			lifebar.transform.localScale = aux_;
	}

	public void reduceLife(int value){
		GameObject blood = Instantiate(Resources.Load("Global/blood"), this.transform.position, Quaternion.identity)as GameObject;
		float aux = value * point;
		life = life - value;
		Vector3 aux_ = lifebar.transform.localScale;
		aux_.x = aux_.x - aux;
		if(aux_.x<0){
			Destroy(this.gameObject);//------------------Dying
			masterPoint.addMoney(moneyWhenKill);
		}else{
			lifebar.transform.localScale = aux_;
		}
	}

	private void enemyreduceLife(){
		if(pathFollow.target!=null){
            DefenderController properties = pathFollow.target.GetComponent<DefenderController>();
			properties.reduceLife(damage);
		}
	}

	void getPoint(){
		float aux = lifebar.transform.localScale.x;
		point = aux/life;
	}

}
