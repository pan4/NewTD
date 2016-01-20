using FThLib;
using System.Collections;
using UnityEngine;

public class CannonController : DefenderController
{
    public int life = 0;
    public int damage = 3;
    public GameObject flag = null;
    public Transform AttackTarget = null;
    public Transform Attacker = null;

    public bool shield = false;

    private Animator _animator;
    private float point = 0f;
    private GameObject lifebar = null;
    private bool isActive = false;
    private bool Attack = false;
    private float delay = 1.5f;
    private int auxlife = 0;
    
    //About healing
    private bool healing = false;
    private float healingdelay = 2f;//Change it for fast healing
    private int healingvalue = 1;
    private Vector3 auxbar = new Vector3(0, 0, 0);
    private Transform _bulletSpawnPos;
    CannonTowerController _towerController;

    Transform _transform;
    public Transform Transform
    {
        get
        {
            return _transform;
        }
    }

    void Start()
    {
        Init();
        _animator = GetComponent<Animator>();
        _transform = transform;
    }

    private void Init()
    {
        Invoke("activation", 2f);
        master.setLayer("tower", this.gameObject);
        lifebar = master.getChildFrom("Lifebar", this.gameObject);
        auxbar = lifebar.transform.localScale;
        _animator = this.gameObject.GetComponent<Animator>();

        _animator.SetBool("walk", false);
        _animator.SetFloat("WalkDirectionX", 0f);
        _animator.SetFloat("WalkDirectionY", -1f);

        _animator.SetBool("dead", false);
        _animator.SetBool("attack", false);
        _bulletSpawnPos = transform.FindChild("CannonBallSpawner");
        _towerController = transform.parent.GetComponent<CannonTowerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!master.isFinish())
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.y);

            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                _animator.SetBool("attack", false);
            }

            if (life != 0 && auxlife == 0)
            {
                getPoint();
            }

            if (point != 0f && auxlife == 0)
            {
                auxlife = life;
            }

            if (isActive == true)
            {
                Vector3 customPos = master.getChildFrom(this.gameObject.name + "p", flag).transform.position;
                if (AttackTarget == null)
                {
                    Vector2 patchPos = new Vector2(this.transform.position.x, this.transform.position.y);
                    Vector2 patchCustomPos = new Vector2(customPos.x, customPos.y);
                    if (patchPos != patchCustomPos)
                    {
                        SetDirectin(customPos);
                        _animator.SetBool("walk", true);
                        transform.position = Vector2.MoveTowards(patchPos, patchCustomPos, Time.deltaTime / 3);
                        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.y);
                    }
                    else
                    {
                        _animator.SetBool("walk", false);
                    }
                }

                if (AttackTarget != null)
                {
                    PathFollower ememyPathFollower = AttackTarget.GetComponent<PathFollower>();
                    //ememyPathFollower.fighting = true;

                    Transform rightp = AttackTarget.FindChild("RightPoint");
                    Transform leftp = AttackTarget.FindChild("LeftPoint");

                    if (move == false && Attack == false)
                    {
                        SetDirectin(AttackTarget.transform.position);
                        _animator.SetBool("attack", true);
                        Attack = true;
                        StartCoroutine(InstantiateBullet(_bulletSpawnPos, AttackTarget.transform, 0.1f));
                        StartCoroutine(DeactivateAttack(delay));
                    }
                }
                else
                {
                    move = false;
                    Attack = false;

                    if (life < auxlife && healing == false)
                    {
                        healing = true;
                        StartCoroutine(IncreaseLife(healingdelay));
                    }
                }
            }
        }
    }

    public void resetLife(int newlife)
    {
        lifebar.transform.localScale = auxbar;
        auxlife = newlife;
        life = newlife;
        getPoint();
    }

    void getPoint()
    {
        float aux = lifebar.transform.localScale.x;
        point = aux / life;
    }

    public IEnumerator IncreaseLife(float delay)
    {
        yield return new WaitForSeconds(delay);
        life = life + healingvalue;
        float aux = healingvalue * point;
        Vector3 aux_ = lifebar.transform.localScale;
        aux_.x = aux_.x + aux;
        lifebar.transform.localScale = aux_;
        healing = false;
    }

    public override void reduceLife(int value)
    {
        GameObject blood = Instantiate(Resources.Load("Global/blood"), this.transform.position, Quaternion.identity) as GameObject;
        life = life - value;
        float aux = value * point;
        Vector3 aux_ = lifebar.transform.localScale;
        aux_.x = aux_.x - aux;
        if (aux_.x < 0)
        {
            aux_.x = 0;
            lifebar.transform.localScale = aux_;
            _animator.SetBool("dead", true);
            Destroy(master.getChildFrom("Shadow", this.gameObject));
            isActive = false;
            Invoke("onDestroy", 1);
        }
        else
        {
            lifebar.transform.localScale = aux_;
        }
    }

    private IEnumerator InstantiateBullet(Transform spawnPoint, Transform target, float delay)
    {
        yield return new WaitForSeconds(delay);             
        GameObject Bullet = Instantiate(Resources.Load("CT/CannonBall"), spawnPoint.position, Quaternion.identity) as GameObject;
        CannonBallController bulletController = Bullet.GetComponent<CannonBallController>();
        bulletController.Target = target;
        bulletController.Damage = damage;
        Bullet.name = "CannonBall";
    }

    private IEnumerator DeactivateAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        Attack = false;
    }

    void activation()
    {
        isActive = true;
    }

    void onDestroy()
    {
        Destroy(this.gameObject);
    }

    private void SetDirectin(Vector3 dir)
    {
        Vector2 direction = (dir - transform.position).normalized;
        _animator.SetFloat("WalkDirectionX", direction.x);
        _animator.SetFloat("WalkDirectionY", direction.y);
    }
}

