using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DefenderController : MonoBehaviour
{
    public bool move = false;
    public virtual void reduceLife(int value) { }
}

