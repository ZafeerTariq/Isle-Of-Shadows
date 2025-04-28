using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 10f;
    private Rigidbody rb;
    private Animator animator;

    void Start() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        Vector3 dir = Vector3.zero;
        if( Input.GetKey( KeyCode.W ) ) {
            dir += Vector3.forward;
        }
        if( Input.GetKey( KeyCode.S ) ) {
            dir += Vector3.back;
        }
        if( Input.GetKey( KeyCode.A ) ) {
            dir += Vector3.left;
        }
        if( Input.GetKey( KeyCode.D ) ) {
            dir += Vector3.right;
        }

        Vector3 rotatedDir = Matrix4x4.Rotate( Quaternion.Euler( 0, 45, 0 ) ).MultiplyPoint3x4( dir.normalized );
        Vector3 velocity = rotatedDir * speed;
        rb.velocity = new Vector3( velocity.x, rb.velocity.y, velocity.z );

        animator.SetBool( "isWalking", rb.velocity.x != 0 || rb.velocity.z != 0 );
    }
}
