using UnityEngine;

public class EnemyMovement : MonoBehaviour {
	public GameObject player;
	public float speed;

	private Rigidbody rb;

	void Awake() {
		player = GameObject.Find( "Player" );
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
		Vector3 velocity = Vector3.zero;
		Vector3 distance = player.transform.position - transform.position;
		if( distance.magnitude > 20 ) {
			Vector3 dir = distance.normalized;
			velocity = dir * speed;
		}
		rb.velocity = new Vector3( velocity.x, rb.velocity.y, velocity.z );
	}
}
