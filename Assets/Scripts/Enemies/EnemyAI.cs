using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
	public GameObject player;

	public float patrolDistance = 10f;
	public float patrolSpeed = 3;

	public float chaseSpeed = 3;
	public float chaseDistance = 30f;

	public float engageSpeed = 5;
	public float engagementDistance = 20f;

	public float attackDistance = 2f;

	private Rigidbody rb;
	private Vector3 startPos;
	private Vector3 targetPos;
	private bool movingToTarget = true;

	public float attackCooldown;
	private float timeSinceLastAttack = 0;
	public int health;
	public int damage;

	private Animator animator;

	private PlayerCombat playerCombat;

	void Awake() {
		player = GameObject.Find( "Player" );
		playerCombat = player.GetComponent<PlayerCombat>();
		rb = GetComponentInChildren<Rigidbody>();
		animator = GetComponentInChildren<Animator>();

		Vector3 patrolDir = new Vector3( Random.Range( 0f, 1f), 0, Random.Range( 0f, 1f ) ).normalized;
		startPos  = transform.position;
		targetPos = transform.position + patrolDir * patrolDistance;
	}

	void Update() {
		timeSinceLastAttack += Time.deltaTime;

		Vector3 velocity = Vector3.zero;
		Vector3 distance = player.transform.position - transform.position;

		if( distance.magnitude <= attackDistance ) {
			TryAttack();
		}
		else if( distance.magnitude <= engagementDistance ) {
			Vector3 dir = distance.normalized;
			velocity = dir * engageSpeed;
		}
		else if( distance.magnitude <= chaseDistance ) {
			Vector3 dir = distance.normalized;
			velocity = dir * chaseSpeed;
		}
		else {
			if( movingToTarget ) {
				Vector3 current = new Vector3( transform.position.x, 0, transform.position.z );
				Vector3 target  = new Vector3( targetPos.x, 0, targetPos.z );

				Vector3 dir = ( target - current ).normalized;
				velocity = dir * patrolSpeed;

				if( Vector3.Distance( current, target ) < 0.5f ) {
					movingToTarget = false;
				}
			}
			else {
				Vector3 current = new Vector3( transform.position.x, 0, transform.position.z );
				Vector3 start   = new Vector3( startPos.x, 0, startPos.z );

				Vector3 dir = ( start - current ).normalized;
				velocity = dir * patrolSpeed;

				if( Vector3.Distance( current, start ) < 0.5f ) {
					movingToTarget = true;
				}
			}
		}

		rb.velocity = new Vector3( velocity.x, rb.velocity.y, velocity.z );
		Vector3 flatVelocity = new Vector3( rb.velocity.x, 0, rb.velocity.z );
		if( flatVelocity.sqrMagnitude > 0.01f ) {
			Quaternion targetRotation = Quaternion.LookRotation( flatVelocity );
			transform.rotation = Quaternion.Slerp( transform.rotation, targetRotation, Time.deltaTime * 5f );
		}
	}

	void OnCollisionEnter( Collision collision ) {
		if( collision.gameObject.layer == LayerMask.NameToLayer( "Resource" ) ) {
			movingToTarget = !movingToTarget;
		}
	}

	private void TryAttack() {
		if( timeSinceLastAttack >= attackCooldown ) {
			playerCombat.TakeDamage( damage );
			timeSinceLastAttack = 0f;
			animator.SetTrigger( "Attack" );
		}
	}

	public void TakeDamage( int damage ) {
		health -= damage;
		if( health <= 0 ) Destroy( gameObject );
	}
}