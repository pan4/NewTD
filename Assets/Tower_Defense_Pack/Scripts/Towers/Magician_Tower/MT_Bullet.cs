using UnityEngine;
using System.Collections;
using FThLib;

public class MT_Bullet : MonoBehaviour {
	//--Public
	public GameObject target=null;
	public int accuracy_mode=3;//1 the best
	public float maxLaunch = 4;
	public bool fire = false;
	public bool ice = false;
    private float _damage = 5;
    public float Damage
    {
        set
        {
            _damage = value;
        }
    }
    //--Private
    //private Master_Instance master;
    private bool activated = false;
	private bool sw =false;
	private float launch_placey=0f;

	private Vector3 latestpos = new Vector3(0,0,0);

	// Use this for initialization
	void Start () {
		//master = GameObject.Find("Master_Instance").GetComponent<Master_Instance>();
		sw=true;
		master.setLayer("tower",this.gameObject);
		if(fire==false){
			master.getChildFrom("add3",this.gameObject).GetComponent<MT_AddFire>().enabled=false;
			master.getChildFrom("add4",this.gameObject).GetComponent<MT_AddFire>().enabled=false;
			
		}
		if(ice==false){
			//master.getChildFrom("add4",this.gameObject).GetComponent<MT_AddIce>().enabled=false;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("enemies"))
        {
            sw = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Collider2D>().enabled = false;
            EnemyController enemyController = collider.GetComponent<EnemyController>();
            enemyController.reduceLife(_damage);
            Invoke("onDestroy", 0);
        }
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward * Time.deltaTime * 400);
			if(target!=null){
				latestpos = target.transform.position;
				if(activated==false){
					activated=true;
				}else{
					if(sw==true){	
						if (fire==true){;
							CreateFire();
						}
						transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime*2);
					}
				}
		}else{
			Destroy(this.gameObject);
		}
	}

	private void Istantiate_Add(GameObject pos, string name){
		GameObject Bullet = Instantiate(Resources.Load("MT/Mfire"), pos.transform.position, Quaternion.identity)as GameObject;
		Bullet.transform.parent = GameObject.Find("Environment").transform;
		MT_Bullet BulletProperties = Bullet.GetComponent<MT_Bullet>();
		Bullet.name=name;
		//--
	}

	void CreateFire(){
		GameObject fire_ = Instantiate(Resources.Load("Global/fire"), this.transform.position, Quaternion.identity)as GameObject;
		fire_.transform.parent = GameObject.Find("Environment").transform;
		Vector3 scale = fire_.transform.localScale;
		scale.x = scale.x *2f;
		scale.y = scale.y *2f;
		fire_.transform.localScale = scale;

	}

	void onDestroy(){
		Destroy (this.gameObject);
	}
}
