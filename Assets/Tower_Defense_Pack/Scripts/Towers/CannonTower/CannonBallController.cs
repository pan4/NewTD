using UnityEngine;
using System.Collections;

public class CannonBallController : MonoBehaviour {


    public Transform Target;
    public float Damage = 1;

    private Transform _transform;


	void Start ()
    {
        _transform = transform;
    }
	

	void Update ()
    {
        if(Target != null)
            transform.position = Vector3.MoveTowards(_transform.position, Target.position, Time.deltaTime * 4);
        else
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("enemies") &&
            other.transform == Target)
        {
            EnemyController enemyController = other.GetComponent<EnemyController>();
            enemyController.reduceLife(Damage);
            Destroy(gameObject);
        }           
    }
}
