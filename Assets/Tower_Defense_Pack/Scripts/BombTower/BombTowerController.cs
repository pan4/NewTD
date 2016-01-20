using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using FThLib;
//It contains the controller of mage in tower
public class BombTowerController : TowerController
{

    public Sprite block;
    //--Private
    private GameObject zone = null;
    private GameObject spawner = null;
    public bool shot_ = false;
    private bool mouseover = false;
    private float searchvalue = 0.1f;// down value is best... but the performance may be affected (used to detect min speed to hit the enemy)
                                     //Public properties
    public float s_timer = 0.9f;
    public int accuracy_mode = 4;//Bassically it is used by the bullet, and add a force in direction to the target.
    public bool fire = false;

    private Animator _bomberAnimator1;
    private Animator _bomberAnimator2;
    private Animator _catapultAnimator;

    void OnMouseOver()
    {
        if (!GameObject.Find("hand")) { master.showHand(true); }
        mouseover = true;
    }

    void OnMouseExit()
    {
        if (GameObject.Find("hand")) { master.showHand(false); }
        mouseover = false;
    }

    void Start()
    {
        this.transform.position = master.setThisZ(this.transform.position, 0.02f);
        spawner = master.getChildFrom("spawner", this.gameObject);
        zone = master.getChildFrom("zone", this.gameObject);
        master.setLayer("tower", this.gameObject);

        _bomberAnimator1 = transform.FindChild("Archer1").GetComponent<Animator>();
        _bomberAnimator2 = transform.FindChild("Archer2").GetComponent<Animator>();
        _catapultAnimator = transform.FindChild("Catapult").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!master.isFinish())
        {
            if (master.getChildFrom("Interface", this.gameObject) == null)
            {
                master.getChildFrom("zoneImg", this.gameObject).GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = true;
            }
            if (Input.GetMouseButtonDown(0) && mouseover == true)
            {
                //master.showInterface(this.gameObject.name, this.gameObject, zone.transform);
                //GetComponent<CircleCollider2D>().enabled = false;
                //master.getChildFrom("zoneImg", this.gameObject).GetComponent<SpriteRenderer>().enabled = true;
            }
            remove_null();
            if (EnemiesInZone.Count > 0)
            {
                if (shot_ == false)
                {
                    shot_ = true;
                    Invoke("shot", s_timer);
                }
            }
        }
    }

    private void shot()
    {
        shot_ = false;

        if (EnemiesInZone.Count == 0)
            return;


        if (EnemiesInZone[EnemiesInZone.Count - 1] != null)
        {
            Transform target = EnemiesInZone[EnemiesInZone.Count - 1].transform;
            BomberShotAnimation(_bomberAnimator1);
            BomberShotAnimation(_bomberAnimator2);
            CatapultShotAnimation(_catapultAnimator, target);
            //Instantiate_Bullet(transform, enemies[enemies.Count - 1]);
            StartCoroutine(StartBulletInstantiate(target.gameObject));
        }
        
    }

    IEnumerator StartBulletInstantiate(GameObject target)
    {
        yield return new WaitForSeconds(0.5f);

        Instantiate_Bullet(transform, target);
    }

    private void BomberShotAnimation(Animator archer)
    {
        archer.SetFloat("AttackDirectionX", 0f);
        archer.SetFloat("AttackDirectionY", -1f);
        archer.SetTrigger("AttackTrigger");
    }

    private void CatapultShotAnimation(Animator catapult, Transform target)
    {
        Vector2 targetPos = new Vector2(target.position.x, target.position.y);
        Vector2 catapultPos = new Vector2(catapult.transform.position.x, catapult.transform.position.y);

        Vector2 direction = (targetPos - catapultPos).normalized;

        catapult.SetFloat("AttackDirectionX", direction.x);
        catapult.SetFloat("AttackDirectionY", direction.y);
        catapult.SetTrigger("AttackTrigger");
    }



    private void Instantiate_Bullet(Transform spawner, GameObject target)
    {
        GameObject Bullet = Instantiate(Resources.Load("RT/bomb"), spawner.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
        BombController BulletProperties = Bullet.GetComponent<BombController>();
        BulletProperties.Damage = damage;
        //############# Bullet properties --
        BulletProperties.target = target;
        if (target != null)
        {
            BulletProperties.maxLaunch = getminSpeed((int)master.angle_(spawner.transform.position, target.gameObject.transform.position), target);
        }
        else
        {
            Destroy(Bullet);
            return;
        }

        BulletProperties.accuracy_mode = accuracy_mode;
        BulletProperties.fire = fire;
        Bullet.name = "bomb";
    }
    private float getminSpeed(int angle, GameObject target)
    {
        float aux = 0.1f;
        while (moreSpeed(aux, target) == true) { aux = aux + searchvalue; }
        return aux;
    }

    private bool moreSpeed(float speed, GameObject target)
    {
        bool aux = false;
        float xTarget = target.transform.position.x;
        float yTarget = target.transform.position.y;
        float xCurrent = transform.position.x;
        float yCurrent = transform.position.y;
        float xDistance = Math.Abs(xTarget - xCurrent);
        float yDistance = yTarget - yCurrent;
        float fireAngle = 1.57075f - (float)(Math.Atan((Math.Pow(speed, 2f) + Math.Sqrt(Math.Pow(speed, 4f) - 9.8f * (9.8f * Math.Pow(xDistance, 2f) + 2f * yDistance * Math.Pow(speed, 2f)))) / (9.8f * xDistance)));
        float xSpeed = (float)Math.Sin(fireAngle) * speed;
        float ySpeed = (float)Math.Cos(fireAngle) * speed;
        if ((xTarget - xCurrent) < 0f) { xSpeed = -xSpeed; }
        if (float.IsNaN(xSpeed) || float.IsNaN(ySpeed)) { aux = true; }
        return aux;
    }
}
