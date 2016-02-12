using AppsMinistry.Core.Input;
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
    public int Level
    {
        get
        {
            return _level;
        }
    }

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

    [SerializeField]
    private List<Sprite> _sprites;


    protected void Init<T>(ref List<T> _list)
    {
        _list = new List<T>();

        for (int towerLevel = 0; towerLevel <= MAX_TOWER_LEVEL; towerLevel++)
            _list.Add(default(T));
    }

    public List<Transform> EnemiesInZone = new List<Transform>();
    private bool _mouseover;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = _sprites[_level];
    }

    private void OnEnable()
    {
        InputManager.Instance.OnTouchEnd += HideInterface;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnTouchEnd -= HideInterface;
    }

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
    public virtual void SetAttackSpeed() { }
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
        towerInterface.GetComponent<TowerMenuController>().SetTower(this);
        towerInterface.transform.SetParent(transform);
        towerInterface.name = "TowerMenuInterface";
        GetComponent<CircleCollider2D>().enabled = false;
        master.getChildFrom("zoneImg", this.gameObject).GetComponent<SpriteRenderer>().enabled = true;
    }

    private void HideInterface(Vector3 position)
    {
        Transform towerInterface = transform.FindChild("TowerMenuInterface");
        if (towerInterface != null)
        {
            Destroy(towerInterface.gameObject);
            transform.FindChild("zoneImg").GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }


    protected virtual void OnUpdate() { }

    private void Update()
    {
        if (!master.isFinish())
        {
            if (Input.GetMouseButtonUp(0) && _mouseover == true)
            {
                ShowInterface();     
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

    public void Upgrade()
    {
        if (_level < MAX_TOWER_LEVEL)
            _level++;

        GetComponent<SpriteRenderer>().sprite = _sprites[_level];
        setDamage();
        SetAttackSpeed();
    }

    public void Sell()
    {
        Master_Instance masterInstance = GameObject.Find("Master_Instance").GetComponent<Master_Instance>();
        masterInstance.addMoney((int)(masterInstance.getPrice(gameObject) / 3) * 2);
        master.sellTower(gameObject);
    }
}


