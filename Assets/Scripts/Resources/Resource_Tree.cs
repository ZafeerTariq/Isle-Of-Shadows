using UnityEngine;

public class Resource_Tree : Resource {
	protected override void Start() {
		base.Start();
		amount = 10;
		type = ResourceType.Wood;
	}
}