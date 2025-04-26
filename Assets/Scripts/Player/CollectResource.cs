using UnityEngine;

public class CollectResource : MonoBehaviour {
	public float collectRange = 3f;
	[Range( 0, 360)] public float fieldOfView = 60f;
	public LayerMask resourceLayer;

	private ICollectable currentTarget = null;

	void Update() {
		UpdateTarget();

		if( Input.GetKeyDown( KeyCode.E ) && currentTarget != null ) {
			currentTarget.Collect();
			currentTarget = null;
		}
	}

	void UpdateTarget() {
		float bestScore = float.MinValue;

		Collider[] colliders = Physics.OverlapSphere( transform.position, collectRange, resourceLayer );
		foreach( Collider collider in colliders ) {
			Vector3 dirToTarget = ( collider.transform.position - transform.position ).normalized;
			Vector3 isoForward = Matrix4x4.Rotate( Quaternion.Euler( 0, 45, 0 ) ).MultiplyPoint3x4( Vector3.forward );
			float angle = Vector3.Angle( isoForward, dirToTarget );

			if( angle < fieldOfView / 2f ) {
                float distance = Vector3.Distance( transform.position, collider.transform.position );
                float centerScore = Mathf.Cos( angle * Mathf.Deg2Rad );
                float score = centerScore / distance;

                if( score > bestScore ) {
                    bestScore = score;
					currentTarget = collider.GetComponent<ICollectable>();
                }
            }
		}


	}
}
