using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FThLib;

public class PathFollower : MonoBehaviour {
	public Transform[] path;
	public float speed = 0f;
	public int currentPoint = 0;
	public bool fighting=false; 

	public bool auxfight=false;
	public GameObject target=null;
	private float auxspeed=0f;
	private Text life;//Text on Canvas
	private Text money;//Text on Canvas
	private GameObject LifeBtn;//Button on Canvas
	private GameObject MoneyBtn;//Button on Canvas
	public Vector3[] custom;
	private bool Step=false;//in direction to target
	private float seed = 0.2f;

	private float rand=0f;
    //public int knights = 0;
    // Use this for initialization

    Animator _animator;
    private Transform _rightPoin;

    void Start ()
    {
		life = GameObject.Find("Life").GetComponent<Text>();
		money = GameObject.Find("Money").GetComponent<Text>();
		LifeBtn = GameObject.Find("Button");
        _animator = GetComponent<Animator>();
        _rightPoin = transform.FindChild("RightPoint");


        auxspeed = speed;
        custom = new Vector3[path.Length];
        RandomizePath();
        
    }

	// Update is called once per frame
	void Update () {
		if(!master.isFinish())
        {
			if(fighting==false)
            {
                auxfight =false;
            }

            if (auxfight!= fighting && target != null)
            {
				rand = Random.Range(0.001f, 2F);
				float randb = Random.Range(rand, rand+2f);
				Invoke("setFight",Random.Range(rand, randb));
			}

            if (speed != 0f && auxfight == false)
            {

                Vector2 direction = (custom[currentPoint] - transform.position).normalized;

                _animator.SetFloat("WalkDirectionX", direction.x);
                _animator.SetFloat("WalkDirectionY", direction.y);

                transform.position = Vector2.MoveTowards (transform.position, custom[currentPoint], Time.deltaTime*speed);
				this.transform.position=new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.y);
				Vector2 patchPos = new Vector2 (this.transform.position.x,this.transform.position.y);
				Vector2 patchCustomPos = new Vector2 (custom[currentPoint].x,custom[currentPoint].y);

				if(patchPos == patchCustomPos)
                {
					if(path[currentPoint].gameObject.name=="End")
                    {
						int value = int.Parse (life.text);
						if(value>0)
                        {
							Animator anim = LifeBtn.GetComponent<Animator>();
							anim.Play("Size");
							value--;
							life.text = "" + value;
						}else
                        {
							End();
						}
					}
					currentPoint++;
				}
				if(currentPoint>=path.Length)
                {
                    Destroy(this.gameObject);
                }
			}

			if(target==null)
            {
                fighting = false;
            }
		}
	}

	private void End()
    {
		if(!GameObject.Find("GameOver"))
        {
			GameObject GameOver = Instantiate(Resources.Load("Interface/GameOver"), new Vector3(0,0,0), Quaternion.identity)as GameObject;
			GameOver.name="GameOver";
			GameObject.Find("Instance_Point").GetComponent<Master_Instance>().Finish=true;
			master.setGameOver();
		}
	}

	private void setFight()
    {
		auxfight=true;
		fighting=true;
        if (target != null)
        {
            Vector2 direction = (_rightPoin.transform.position - transform.position).normalized;
            _animator.SetFloat("WalkDirectionX", direction.x);
            _animator.SetFloat("WalkDirectionY", direction.y);
        }
    }

	public void reduceSpeed()
    {
		speed = speed/2;
		Invoke("setSpeed",2);
	}

	private void setSpeed()
    {
        speed =auxspeed;
    }

	void RandomizePath()
    {
		for(int i = 0;i < path.Length;i++)
        {
			if(path[i].gameObject.name!="End")
            {
				custom[i] = new Vector3(path[i].position.x + Random.Range(-seed, seed),path[i].position.y + Random.Range(-seed, seed),path[i].position.y);
			}else
            {
				custom[i] = new Vector3(path[i].position.x ,path[i].position.y ,path[i].position.y);
			}
		}
	}

	void Stop()
    {
        Step = true;
    }
}
