﻿using UnityEngine;
using System.Collections;
using FThLib;

public class TreeSetZ : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Master_Instance master = GameObject.Find("Master_Instance").GetComponent<Master_Instance>();
		this.transform.position = master.setThisZ(this.transform.position,0.02f);
	}

}
