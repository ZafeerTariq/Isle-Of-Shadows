using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 10f;
    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
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

        Vector3 velocity = dir.normalized * speed;
        rb.velocity = new Vector3( velocity.x, rb.velocity.y, velocity.z );
    }
}
