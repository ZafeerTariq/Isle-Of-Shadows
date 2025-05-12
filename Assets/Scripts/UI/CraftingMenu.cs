using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour {
	public GameObject menuUI;
	[SerializeField] private Inventory inventory;
	[SerializeField] private GameObject weaponButton;

	public List<WeaponScriptableObject> weapons;

	void Start() {
		menuUI.SetActive( false );
		PopulateMenu();
	}

	void Update() {
		if( Input.GetKeyDown( KeyCode.Tab ) ) {
			menuUI.SetActive( !menuUI.activeSelf );
		}
	}

	private void PopulateMenu() {
		foreach( var weapon in weapons ) {
			GameObject button = Instantiate( weaponButton, menuUI.transform );

			string requirements = "";
			foreach( var requirement in weapon.requirements ) {
				requirements += requirement.amount.ToString() + " " + requirement.resource.ToString() + " ";
			}

			button.GetComponentInChildren<TextMeshProUGUI>().text = weapon.weaponName + " Cost " + requirements;
			button.GetComponent<Button>().onClick.AddListener( () => TryCraftWeapon( weapon ) );
		}
	}

	private void TryCraftWeapon( WeaponScriptableObject weapon ) {
		bool canCraft = false;
		foreach( var requirement in weapon.requirements ) {
			if( requirement.amount > inventory.GetAmount( requirement.resource ) ) {
				canCraft = false;
				break;
			}
			else {
				canCraft = true;
			}
		}

		if( canCraft ) {
			foreach( var requirement in weapon.requirements ) {
				inventory.TryRemoveResource( requirement.resource, requirement.amount );
			}
			inventory.AddWeapon( weapon );
		}
	}
}
