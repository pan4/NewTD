using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FThLib;

public class Waves_Creator_Controller : MonoBehaviour
{
	private Master_Instance _masterInstance;
    private int wavesIndex = 0;
	public int[] enemiesInWaves    = new int[] {2,  3,  4,  5};
	public int[] delayBetweenWaves = new int[] {0, 10, 10, 10};
	//private bool playing=false;
	private bool auxplaying=false;
	private Text waves;

    private int count = 0;
    private Transform[] path;
    private float seed = 0.2f;

    private GameObject _gameObject;
    Transform _movePathRoot;

    [SerializeField]
    private float enemySpeed = 0.3f;
    [SerializeField]
    private int enemyHelth = 20;

    private float delayBetweenEnemies = 1f;
    private bool wavePlaying = false;
    private bool _ememySpawnFlag;
    private int _delayIndex = 0;

    void Awake()
    {
        _movePathRoot = transform.FindChild("MovePath");
        int pathSize = GetPathSize(_movePathRoot);
        Transform tr;
        int i = 1;
        do
        {
            tr = _movePathRoot.FindChild(string.Format("a0 ({0})", i));
            if (tr)
                tr.gameObject.name = "a" + i;
            i++;
        }
        while (tr != null);
    }

    void Start ()
    {
		_masterInstance = FindObjectOfType<Master_Instance>();
		waves = GameObject.Find("Waves").GetComponent<Text>();
		waves.text = wavesIndex + "/" + enemiesInWaves.Length;
        _movePathRoot = transform.FindChild("MovePath");
        int pathSize = GetPathSize(_movePathRoot);
        path = new Transform[pathSize];

        for (int i = 0; i < pathSize - 1; i++)
        {
            path[i] = _movePathRoot.FindChild("a" + i);
        }

        path[pathSize - 1] = _movePathRoot.FindChild("End");
        _gameObject = gameObject; 
    }
	

	void Update ()
    {
		if (_masterInstance.Finish == false)
        {
			if(_masterInstance.Playing == true && _delayIndex < enemiesInWaves.Length)
            {
				if(auxplaying == false)
                {
					auxplaying = true;
					//if(delayBetweenWaves[_delayIndex] > 0)
     //               {
					//	master.Instantiate_Progressbar(delayBetweenWaves[wavesIndex], this.gameObject);
					//	master.getChildFrom("ProgressBar", this.gameObject).transform.localScale = new Vector3(2, 2, 1);
					//}
					Invoke("Wave_Creator", delayBetweenWaves[_delayIndex]);
				}
			}

            if (wavePlaying == true && wavesIndex < enemiesInWaves.Length)
            {
                if (_ememySpawnFlag == false)
                {
                    _ememySpawnFlag = true;
                    Invoke("Instantiate_Enemy", delayBetweenEnemies);
                }
            }
            
        }
	}

	private void Wave_Creator()
    {
		auxplaying = false;
		waves.text = (wavesIndex + 1) + "/" + enemiesInWaves.Length;
        wavePlaying = true;
        _delayIndex++;
    }

    private void Instantiate_Enemy()
    {
        int type = 1;

        GameObject Enemy = Instantiate(Resources.Load("Enemies/enemy" + type),
            new Vector3(_movePathRoot.transform.position.x + Random.Range(-seed, seed), _movePathRoot.transform.position.y + Random.Range(-seed, seed), _movePathRoot.transform.position.z),
            Quaternion.identity) as GameObject;

        if (!transform.FindChild("Enemies"))
            new GameObject("Enemies").transform.parent = transform;

        Enemy.transform.parent = transform.FindChild("Enemies");
        PathFollower pathFollower = Enemy.GetComponent<PathFollower>();
        EnemyController EnemyController = Enemy.GetComponent<EnemyController>();
        Enemy.name = "Enemy" + count;
        pathFollower.path = path;
        pathFollower.speed = enemySpeed;
        EnemyController.life = enemyHelth;
        count++;
        _ememySpawnFlag = false;
        enemiesInWaves[wavesIndex]--;

        if (enemiesInWaves[wavesIndex] == 0)
        {
            wavePlaying = false;
            if (wavesIndex < enemiesInWaves.Length - 1)
                wavesIndex++;
        }
    }

    int GetPathSize(Transform pathRoot)
    {
        int i = 0;
        while (pathRoot.FindChild("a" + i))
        {
            i++;
        }
        i++;
        return i;
    }
}
