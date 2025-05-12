using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour {
	[SerializeField] private int health;
	[SerializeField] private int damage;
	private float weaponMultiplier = 1f;
	public int attackRange;
	public LayerMask enemyLayer;

	private bool isAlive;

	public float attackCooldown;
	private float timeSinceLastAttack;

	public Inventory inventory;

	public RawImage healthBarImage;
	private Material healthBarMat;

	private Animator animator;

	void Start() {
		isAlive = true;
		timeSinceLastAttack = 0f;
		healthBarMat = healthBarImage.material;
		healthBarMat.SetFloat( "_Health", health );
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
			timeSinceLastAttack = 0f;
			animator.SetTrigger( "Attack" );
			Collider[] colliders = Physics.OverlapSphere( transform.position, attackRange, enemyLayer );
			foreach( Collider collider in colliders ) {
				collider.gameObject.GetComponent<EnemyAI>().TakeDamage( damage );
			}
		}
	}

	public void EquipWeapon( WeaponScriptableObject weapon ) {
		if( inventory.Check( weapon ) ) {
			weaponMultiplier = weapon.multiplier;
		}
	}
}