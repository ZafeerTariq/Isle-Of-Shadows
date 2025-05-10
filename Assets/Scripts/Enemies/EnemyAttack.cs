using UnityEngine;

public class EnemyAttack : MonoBehaviour {
	public float engagementDistance = 20f;

	public GameObject player;
	public float speed;

	private Rigidbody rb;


	void Awake() {
		player = GameObject.Find( "Player" );
		rb = GetComponent<Rigidbody>();
	}

	void Update() {
		Vector3 distance = player.transform.position - transform.position;
		if( distance.magnitude <= engagementDistance ) {
			Vector3 dir = distance.normalized;
			Vector3 velocity = dir * speed;
			rb.velocity = new Vector3( velocity.x, rb.velocity.y, velocity.z );
		}
	}
}
