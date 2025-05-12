using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour {
	public GameObject menuUI;
	public GameObject weaponButton;
	public List<WeaponScriptableObject> availableWeapons;

	[SerializeField] private Inventory inventory;
	[SerializeField] private PlayerCombat player;

	private int equippedWeapon = -1;
	private List<GameObject> buttons = new List<GameObject>();

	public Color equippedColor = new Color( 0.5656322f, 1f, 0.4858491f, 1f );
	public Color notAvailableColor;

	void Start() {
		menuUI.SetActive( false );

		foreach( WeaponScriptableObject weapon in availableWeapons ) {
			GameObject button = Instantiate( weaponButton, menuUI.transform );
			button.GetComponentInChildren<TextMeshProUGUI>().text = weapon.weaponName;
			button.GetComponent<Button>().onClick.AddListener( () => EquipWeapon( weapon ) );
			if( !inventory.Check( weapon ) ) button.GetComponent<Image>().color = notAvailableColor;
			buttons.Add( button );
		}
	}

	void Update() {
		if( Input.GetKeyDown( KeyCode.Q ) ) {
			menuUI.SetActive( !menuUI.activeSelf );
			if( equippedWeapon != -1 )
				buttons[equippedWeapon].GetComponent<Image>().color = equippedColor;
		}
	}

	public void UpdateUI() {
		int i = 0;
		foreach( WeaponScriptableObject weapon in availableWeapons ) {
			buttons[i].GetComponent<Image>().color = new Color( 1, 1, 1, 1 );
			if( !inventory.Check( weapon ) ) buttons[i].GetComponent<Image>().color = notAvailableColor;
			i++;
		}
	}

	private void EquipWeapon( WeaponScriptableObject weapon ) {
		if( inventory.Check( weapon ) ) {
			player.EquipWeapon( weapon );
			equippedWeapon = availableWeapons.IndexOf( weapon );
			UpdateUI();
		}
	}
}
