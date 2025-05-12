using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour {
	[SerializeField] private int health;
	[SerializeField] private int damage;
	public int attackRange;
	public LayerMask enemyLayer;

	private bool isAlive;

	public float attackCooldown;
	private float timeSinceLastAttack;

	public Inventory inventory;

	public RawImage healthBarImage;
	private Material healthBarMat;

	private Animator animator;

	public Transform weaponSlot;
	private GameObject equippedWeapon = null;
	private float weaponMultiplier = 1f;

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

			Vector3 position	= new Vector3( 0.074f, 0.068f, 0.074f );
			Quaternion rotation	= Quaternion.Euler( 0, -45, -62.5f );
			Vector3 scale		= new Vector3( 0.5f, 0.5f, 0.5f );

			if( equippedWeapon ) Destroy( equippedWeapon );
			equippedWeapon = Instantiate( weapon.prefab, weaponSlot );
			equippedWeapon.transform.localPosition	= position;
			equippedWeapon.transform.localRotation	= rotation;
			equippedWeapon.transform.localScale		= scale;
		}
	}
}