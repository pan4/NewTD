using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public int damage = 3;
    public int life = 20;

    public List<Transform> EnemiesInZone = new List<Transform>();

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
}


