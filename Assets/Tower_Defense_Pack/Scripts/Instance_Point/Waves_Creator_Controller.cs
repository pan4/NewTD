using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FThLib;

public class Waves_Creator_Controller : MonoBehaviour {
	private Master_Instance masterPoint;
	public int wavesIndex = 4; // Number of waves
	public int[] enemiesInWaves    = new int[] {5,  4,  3,  2};//The last object in array is the first to appear
	public int[] delayBetweenWaves = new int[] {40, 35, 30, 0};//The last object in array is the first to appear
	private bool playing=false;
	private bool auxplaying=false;
	private Text waves;
	// Use this for initialization
	void Start () {
		masterPoint = GameObject.Find("Instance_Point").GetComponent<Master_Instance>();
		waves = GameObject.Find("Waves").GetComponent<Text>();
		waves.text = wavesIndex + "/" + enemiesInWaves.Length;
		wavesIndex--;

	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<Master_Instance>().Finish==false){
			if (Input.GetButtonDown("Jump")){
				Destroy (GameObject.Find("pressSpace"));
				playing=true;
			}
			if(playing==true&&wavesIndex>=0){
				if(auxplaying==false){
					auxplaying=true;
					if(delayBetweenWaves[wavesIndex]>0){
						master.Instantiate_Progressbar(delayBetweenWaves[wavesIndex],this.gameObject);
						master.getChildFrom("ProgressBar",this.gameObject).transform.localScale=new Vector3(2,2,1);
					}
					Invoke("Wave_Creator",delayBetweenWaves[wavesIndex]);
				}
			}
			if(wavesIndex<0){playing=false;}
		}
	}

	private void Wave_Creator(){
		auxplaying=false;
		masterPoint.createWave(enemiesInWaves[wavesIndex],1);//This pack only contains enemy type 1
		waves.text = wavesIndex + "/" + enemiesInWaves.Length;
		wavesIndex--;
	}
}
