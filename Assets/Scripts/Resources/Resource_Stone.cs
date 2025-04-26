using UnityEngine;

public class Resource_Stone : Resource {
	protected override void Start() {
		base.Start();
		amount = 5;
		type = ResourceType.Stone;
	}
}