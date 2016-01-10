using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {
	Animator anim;
	// Use this for initialization
	void Start () {
		this.transform.parent = GameObject.Find("Environment").transform;
		anim = this.gameObject.GetComponent<Animator> ();
		Color aux = GetComponent<SpriteRenderer>().material.color;
		aux.a =0.5f;
		GetComponent<SpriteRenderer>().material.color = aux;
	}
	
	// Update is called once per frame
	void Update () {
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("empty")) {
			Invoke ("onDestroy",1);
		}
	}

	void onDestroy(){
		Destroy(this.gameObject);
	}
}
