using UnityEngine;
using System.Collections;

public class Zone_Controller : MonoBehaviour {
	private GameObject parent_;
	private TowerController Properties;

	// Use this for initialization
	void Start () {
		parent_ = this.transform.parent.gameObject;
		Properties = parent_.GetComponent<TowerController>();		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.tag=="Respawn"){
			Properties.EnemyAdd(other.transform);
		}

	}
	void OnTriggerExit2D(Collider2D other) {
		if(other.tag=="Respawn"){
			Properties.EnemyRemove(other.transform);
		}
	}
}
