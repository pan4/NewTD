using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FThLib;

public class PathFollower : MonoBehaviour
{
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
	private Vector3[] randomisedPath;
	private bool Step=false;//in direction to target
	private float seed = 0.2f;

	private float rand=0f;

    Animator _animator;
    private Transform _rightPoin;

    private Transform _moveTarget = null;
    public Transform MoveTarget
    {
        get
        {
            return _moveTarget;
        }
        set
        {
            _moveTarget = value;
        }
    }

    private Transform _transform;

    void Start ()
    {
		life = GameObject.Find("Life").GetComponent<Text>();
		money = GameObject.Find("Money").GetComponent<Text>();
		LifeBtn = GameObject.Find("Button");
        _animator = GetComponent<Animator>();
        _rightPoin = transform.FindChild("RightPoint");
        _transform = transform;

        auxspeed = speed;
        randomisedPath = new Vector3[path.Length];
        RandomizePath();

        _moveTarget = new GameObject(name + "MoveTarget").transform;
        _moveTarget.position = randomisedPath[currentPoint];
    }

	void Update ()
    {
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
                Vector2 direction = (_moveTarget.position - _transform.position).normalized;

                _animator.SetFloat("WalkDirectionX", direction.x);
                _animator.SetFloat("WalkDirectionY", direction.y);
                
                _transform.position = Vector2.MoveTowards (_transform.position, _moveTarget.position, Time.deltaTime*speed);

                Vector2 pos = new Vector2(_transform.position.x, _transform.position.y);
                Vector2 moveTargetPos = new Vector2(_moveTarget.position.x, _moveTarget.position.y);
                _transform.position = new Vector3(_transform.position.x, _transform.position.y, _transform.position.y);
                if (pos == moveTargetPos && target == null)
                {
                    if (path[currentPoint].gameObject.name == "End")
                    {
                        int value = int.Parse(life.text);
                        if (value > 0)
                        {
                            Animator anim = LifeBtn.GetComponent<Animator>();
                            anim.Play("Size");
                            value--;
                            life.text = "" + value;
                        }
                        else
                        {
                            End();
                        }
                    }
                
                    currentPoint++;
                    if(currentPoint < randomisedPath.Length)
                        _moveTarget.position = randomisedPath[currentPoint];                    
                }

                if (pos == moveTargetPos && target != null)
                {
                    direction = (target.transform.position - _moveTarget.position).normalized;
                    _animator.SetFloat("WalkDirectionX", direction.x);
                    _animator.SetFloat("WalkDirectionY", direction.y);
                    auxfight = true;
                }

                if (currentPoint>=path.Length)
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
			GameObject GameOver = Instantiate(Resources.Load("Interface/GameOver"), new Vector3(0,0,0), Quaternion.identity) as GameObject;
			GameOver.name="GameOver";
			GameObject.Find("Instance_Point").GetComponent<Master_Instance>().Finish = true;
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
				randomisedPath[i] = new Vector3(path[i].position.x + Random.Range(-seed, seed),path[i].position.y + Random.Range(-seed, seed),path[i].position.y);
			}else
            {
				randomisedPath[i] = new Vector3(path[i].position.x ,path[i].position.y ,path[i].position.y);
			}
		}
	}

	void Stop()
    {
        Step = true;
    }
}
