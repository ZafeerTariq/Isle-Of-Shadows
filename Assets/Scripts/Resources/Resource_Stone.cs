using UnityEngine;

public class Resource_Stone : MonoBehaviour, ICollectable {
	void ICollectable.Collect() {
		Destroy( gameObject );
	}
}