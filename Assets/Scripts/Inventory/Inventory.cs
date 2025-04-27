using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour {
	private Dictionary<ResourceType, int> inventory = new Dictionary<ResourceType, int>();

	void Start() {
		foreach( ResourceType resource in System.Enum.GetValues( typeof(ResourceType) ) ) {
			inventory[resource] = 0;
		}
	}

	public void AddResource( ResourceType resource, int amount ) {
		inventory[resource] += amount;
		Debug.Log( resource.ToString() + " : " + inventory[resource].ToString() );
	}

	public int GetAmount( ResourceType resource ) {
		return inventory[resource];
	}

	public bool TryRemoveResource( ResourceType resource, int amount ) {
		if( amount <= inventory[resource] ) {
			inventory[resource] -= amount;
			Debug.Log( resource.ToString() + " : " + inventory[resource].ToString() );
			return true;
		}
		return false;
	}
}