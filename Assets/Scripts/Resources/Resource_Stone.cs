using UnityEngine;

public class Resource_Stone : MonoBehaviour, ICollectable, IHighlightable {
	public Material highlightMat;
	public Material originalMat;

	void ICollectable.Collect() {
		Destroy( gameObject );
	}

	void IHighlightable.Highlight() {
		gameObject.GetComponent<Renderer>().material = highlightMat;
	}

	void IHighlightable.Unhighlight() {
		gameObject.GetComponent<Renderer>().material = originalMat;
	}
}