using System;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public int damage = 3;
    public int life = 20;

    public virtual void enemyAdd(GameObject other) { }
    public virtual void enemyRemove(string other) { }
    public virtual void Reset() { }
    public virtual void setDamage() { }
    public virtual void setShield() { }
}


