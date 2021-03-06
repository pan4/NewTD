﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using FThLib;

public class CannonTowerController : TowerController
{
	private float instancetime = 11f;

	private GameObject spawner=null;
	//public GameObject flag=null;
	private GameObject zone=null;

	private bool inprocess=false;
	private bool firstime=true;

	public bool shield =false;

    private GameObject _gameObject;
    private Transform _transform;

    [SerializeField]
    private string _unitPath = "Kt/Knight";

    CannonController _cannonController;

	void OnTriggerEnter2D(Collider2D coll)
    {
		if(coll.name=="Cannon")
        {
			StartCoroutine(setZ(coll.gameObject, 1f));
		}
	}

	private void Init_()
    {
		spawner = master.getChildFrom("spawner",this.gameObject);
		//flag = master.getChildFrom("flag",this.gameObject);
		zone = master.getChildFrom("zone",this.gameObject);
	}

	void Start () {

        _gameObject = gameObject;
        _transform = transform;

        this.transform.position = master.setThisZ(this.transform.position,0.02f);
		master.setLayer("tower",this.gameObject);
		setFlagZ();
		Init_();
	}

	protected override void OnUpdate ()
    {
        base.OnUpdate();
		if(!master.isFinish())
        {
			if(inprocess == false)
            {
                CannonCall();
            }

            remove_null();

            if (EnemiesInZone.Count > 0 && _cannonController != null)
            {
                if(_cannonController.AttackTarget == null)
                    SetTargert();
                if (_cannonController.Attacker == null)
                    SetAttacker();
            }
		}
	}

	void SetTargert()
    {
		for(int i=0; i < EnemiesInZone.Count; i++)
        {
			if(EnemiesInZone[i] != null)
            {
                {
                    if (_cannonController.AttackTarget == null)
                    {
                        _cannonController.AttackTarget = EnemiesInZone[i];
                        return;
                    }
                }
			}
		}
    }

    private void SetAttacker()
    {
        for (int i = EnemiesInZone.Count - 1; i >= 0; i--)
        {
            if (EnemiesInZone[i] != null)
            {
                PathFollower enemyProperties = EnemiesInZone[i].GetComponent<PathFollower>();
                if (enemyProperties.fighting == false)
                {

                    if (Vector3.SqrMagnitude(_cannonController.Transform.position - enemyProperties.transform.position) < 1 * 1)
                    {
                        enemyProperties.target = _cannonController.Transform.gameObject;
                        _cannonController.Attacker = enemyProperties.transform;
                        enemyProperties.MoveTarget.position = enemyProperties.target.transform.position - Vector3.right * 0.4f;
                        return;
                    }                    

                }
            }
        }
    }

	private void CannonCall()
    {
        if (master.getChildFrom("Cannon", this.gameObject))
        {
			firstime = false;
		}
        else
        {
			if(firstime == true)
            {
				inprocess = true;
				master.Instantiate_Progressbar(4f, this.gameObject);
                StartCoroutine(InstantiateCannon(4f));
			}
            else
            {
				inprocess = true;
				master.Instantiate_Progressbar(instancetime, this.gameObject);
                StartCoroutine(InstantiateCannon(instancetime));
            }
		}
	}

	private IEnumerator InstantiateCannon(float delay)
    {
        yield return new WaitForSeconds(delay);
             
		inprocess=false;
		GameObject cannon = Instantiate(Resources.Load(_unitPath), spawner.transform.position, Quaternion.identity)as GameObject;
		cannon.transform.SetParent(this.gameObject.transform);
		_cannonController = cannon.GetComponent<CannonController>();
		_cannonController.flag=Flag;
		_cannonController.life=life;
		_cannonController.shield = shield;
		_cannonController.damage = Damage[_level];
        _cannonController.AttackDelay = 1f / AttackSpeed[_level];
        cannon.name = "Cannon";
	}

	private IEnumerator setZ(GameObject go, float delayTime)
    {
		yield return new WaitForSeconds(delayTime);
		go.transform.position = new Vector3(go.transform.position.x,go.transform.position.y,0f);
	}

	private void setFlagZ()
    {
        Transform flag = transform.FindChild("flag");
        flag.position = flag.position + Vector3.up * 0.2f;
    }

    public override void setDamage()
    {
        if (master.getChildFrom("Cannon", this.gameObject))
        {
            master.getChildFrom("Cannon", this.gameObject).GetComponent<CannonController>().damage = Damage[_level];
        }
    }

    public override void SetAttackSpeed()
    {
        if (master.getChildFrom("Cannon", this.gameObject))
        {
            master.getChildFrom("Cannon", this.gameObject).GetComponent<CannonController>().AttackDelay = 1f / AttackSpeed[_level];
        }
    }

    public override void setShield()
    {
        shield = true;

        if (master.getChildFrom("Cannon", this.gameObject))
        {
            master.getChildFrom("Cannon", this.gameObject).GetComponent<CannonController>().shield = true;
            master.getChildFrom("Cannon", this.gameObject).GetComponent<CannonController>().resetLife(life);
        }
    }

    public override void Reset()
    {
        master.getChildFrom("TargetedZone", this.gameObject).transform.position = Flag.transform.position;
        if (EnemiesInZone.Count > 0)
        {
            for (int i = 0; i < EnemiesInZone.Count; i++)
            {
                PathFollower enemyProperties = EnemiesInZone[i].GetComponent<PathFollower>();
                if (enemyProperties.fighting == true)
                {
                    enemyProperties.target = null;
                    enemyProperties.fighting = false;
                }
                EnemyRemove(EnemiesInZone[i]);
            }
        }
        if (master.getChildFrom("Cannon", this.gameObject))
        {
            CannonController properties = master.getChildFrom("Cannon", this.gameObject).GetComponent<CannonController>();
            if (properties.AttackTarget != null)
            {
                properties.AttackTarget.GetComponent<PathFollower>().target = null;
            }
            properties.AttackTarget = null;
        }
    }

    public override void EnemyRemove(Transform enemy)
    {
        base.EnemyRemove(enemy);
        if (_cannonController != null && _cannonController.AttackTarget == enemy)
            _cannonController.AttackTarget = null;
    }
}
