using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour {
	public GameObject inventoryUI;
	public GameObject textPrefab;
	private List<GameObject> texts = new List<GameObject>();

	private Dictionary<ResourceType, int> inventory = new Dictionary<ResourceType, int>();

	void Start() {
		foreach( ResourceType resource in System.Enum.GetValues( typeof(ResourceType) ) ) {
			inventory[resource] = 0;

			GameObject text = Instantiate( textPrefab, inventoryUI.transform );
			text.GetComponent<TextMeshProUGUI>().SetText( resource.ToString() + " : " + inventory[resource].ToString() );
			texts.Add( text );
		}
		UpdateUI();
	}

	private void UpdateUI() {
		int i = 0;
		foreach( ResourceType resource in System.Enum.GetValues( typeof(ResourceType) ) ) {
			texts[i].GetComponent<TextMeshProUGUI>().SetText( resource.ToString() + " : " + inventory[resource].ToString() );
			i++;
		}
	}

	public void AddResource( ResourceType resource, int amount ) {
		inventory[resource] += amount;
		Debug.Log( resource.ToString() + " : " + inventory[resource].ToString() );
		UpdateUI();
	}

	public int GetAmount( ResourceType resource ) {
		return inventory[resource];
	}

	public bool TryRemoveResource( ResourceType resource, int amount ) {
		if( amount <= inventory[resource] ) {
			inventory[resource] -= amount;
			Debug.Log( resource.ToString() + " : " + inventory[resource].ToString() );
			UpdateUI();
			return true;
		}
		return false;
	}
}