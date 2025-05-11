using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour {
	[SerializeField] private int health;
	[SerializeField] private int damage;
	private bool isAlive;

	public float attackCooldown;
	private float timeSinceLastAttack;

	public RawImage healthBarImage;
	private Material healthBarMat;

	private Animator animator;

	void Start() {
		isAlive = true;
		timeSinceLastAttack = 0f;
		healthBarMat = healthBarImage.material;
        animator = GetComponent<Animator>();
	}

	void Update() {
		timeSinceLastAttack += Time.deltaTime;
		if( Input.GetMouseButtonDown( 0 ) ) {
			TryAttack();
		}
	}

	public void TakeDamage( int damage ) {
		health -= damage;
		if( health <= 0 ) isAlive = false;
		else healthBarMat.SetFloat( "_Health", health );
	}

	private void TryAttack() {
		if( timeSinceLastAttack >= attackCooldown ) {
			Debug.Log( "Player attack" );
			timeSinceLastAttack = 0f;
			animator.SetTrigger( "Attack" );
		}
	}
}
