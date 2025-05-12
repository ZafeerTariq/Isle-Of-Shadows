using UnityEngine;

public class Resource : MonoBehaviour, ICollectable, IHighlightable {
	public Material highlightMat;
	public Material originalMat;

	private Inventory inventory;
	protected int amount;
	protected ResourceType type;

	protected virtual void Start() {
		inventory = FindObjectOfType<Inventory>();
	}

	void ICollectable.Collect() {
		inventory.AddResource( type, amount );
		Destroy( gameObject );
	}

	void IHighlightable.Highlight() {
		gameObject.GetComponentInChildren<Renderer>().material = highlightMat;
		// gameObject.GetComponent<Renderer>().material = highlightMat;
	}

	void IHighlightable.Unhighlight() {
		gameObject.GetComponentInChildren<Renderer>().material = originalMat;
		// gameObject.GetComponent<Renderer>().material = originalMat;
	}
}