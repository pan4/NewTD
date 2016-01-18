using UnityEngine;
using System.Collections;

public class CannonBallController : MonoBehaviour {


    public Transform Target;
    public int Damage = 1;

    private Transform _transform;


	void Start ()
    {
        _transform = transform;
    }
	

	void Update ()
    {
        transform.position = Vector3.MoveTowards(_transform.position, Target.position, Time.deltaTime * 4);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("enemies") &&
            other.transform == Target)
        {
            Enemies_Controller enemyController = other.GetComponent<Enemies_Controller>();
            enemyController.reduceLife(Damage);
            Destroy(gameObject);
        }           
    }
}
