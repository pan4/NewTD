using FThLib;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    //public int damage = 3;
    public int life = 20;

    public const int MAX_TOWER_LEVEL = 3;
    protected int _level = 0;

    [SerializeField]
    [HideInInspector]
    private List<float> _damage;
    public List<float> Damage
    {
        get
        {
            if (_damage == null || _damage.Count <= MAX_TOWER_LEVEL)
                Init(ref _damage);

            return _damage;
        }
    }

    [SerializeField]
    [HideInInspector]
    private List<float> _attackSpeed;
    public List<float> AttackSpeed
    {
        get
        {
            if (_attackSpeed == null || _attackSpeed.Count <= MAX_TOWER_LEVEL)
                Init(ref _attackSpeed);

            return _attackSpeed;
        }
    }

    protected void Init<T>(ref List<T> _list)
    {
        _list = new List<T>();

        for (int towerLevel = 0; towerLevel <= MAX_TOWER_LEVEL; towerLevel++)
            _list.Add(default(T));
    }

    public List<Transform> EnemiesInZone = new List<Transform>();
    private bool _mouseover;

    public virtual void EnemyAdd(Transform enemy)
    {
        EnemiesInZone.Add(enemy);
    }

    public virtual void EnemyRemove(Transform enemy)
    {
        for (int i = 0; i < EnemiesInZone.Count; i++)
        {

            if (EnemiesInZone[i] == enemy)
            {
                EnemiesInZone.RemoveAt(i);
            }            
        }
    }

    public virtual void Reset() { }
    public virtual void setDamage() { }
    public virtual void setShield() { }

    protected void remove_null()
    {
        for (int i = 0; i < EnemiesInZone.Count; i++)
        {
            if (EnemiesInZone[i] == null)
            {
                EnemiesInZone.RemoveAt(i);
            }
        }
    }

    private void ShowInterface()
    {
        if (GameObject.Find("Interface"))
        {
            master.other_Interfaces_off();
        }
        GameObject towerInterface = Instantiate(Resources.Load("Interface/TowerInterface"), transform.position, Quaternion.identity) as GameObject;
        towerInterface.transform.SetParent(transform);
        towerInterface.name = "Interface";
    }

    protected virtual void OnUpdate() { }

    private void Update()
    {
        if (!master.isFinish())
        {
            if (master.getChildFrom("Interface", this.gameObject) == null)
            {
                master.getChildFrom("zoneImg", this.gameObject).GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = true;
            }
            if (Input.GetMouseButtonDown(0) && _mouseover == true)
            {
                ShowInterface();
                GetComponent<CircleCollider2D>().enabled = false;
                master.getChildFrom("zoneImg", this.gameObject).GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        OnUpdate();
    }

    void OnMouseOver()
    {
        if (!GameObject.Find("hand")) { master.showHand(true); }
        _mouseover = true;
    }

    void OnMouseExit()
    {
        if (GameObject.Find("hand")) { master.showHand(false); }
        _mouseover = false;
    }

}


