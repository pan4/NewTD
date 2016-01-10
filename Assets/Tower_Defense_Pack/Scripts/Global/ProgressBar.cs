using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {
	public float time=0f;
	private bool activated = false;
	private CustomFixedUpdate m_FixedUpdate;
	private int cont =0;
	private float customtime = 0f;
	// Use this for initialization
	void Start () {
		customtime = time / 0.01f;
		m_FixedUpdate = new CustomFixedUpdate(0.01f, MyFixedUpdate);
	}
	
	// Update is called once per frame
	void Update () {
		if(activated==false&&time!=0f){
			activated=true;
		}
		m_FixedUpdate.Update(Time.deltaTime);
	}

	void MyFixedUpdate()
	{
		if(activated==true){
			decrease ();
		}
	}
	
	void decrease(){
		if(transform.localScale.x>=0){
			Vector3 aux = transform.localScale;
			//aux.x = aux.x - 0.001f;
			aux.x = aux.x - 1/customtime;
			transform.localScale = aux;
		}else{
			Destroy(this.transform.parent.gameObject);
		}
	}
}
