using UnityEngine;

public class Resource_Tree : MonoBehaviour, ICollectable {
	void ICollectable.Collect() {
		Destroy( gameObject );
	}
}
