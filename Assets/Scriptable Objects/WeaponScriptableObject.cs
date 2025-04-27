using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Weapon", menuName = "ScriptableObjects/Weapon" )]
public class WeaponScriptableObject : ScriptableObject {
	public string weaponName;
	public GameObject prefab;
	public List<ResourceRequirements> requirements;
}

[System.Serializable]
public class ResourceRequirements {
	public ResourceType resource;
	public int amount;
}