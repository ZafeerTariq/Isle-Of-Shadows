using UnityEngine;

[CreateAssetMenu( fileName = "Noise Data", menuName = "ScriptableObjects/Noise Data" )]
public class NoiseDataScriptableObject : ScriptableObject {
	public float scale;
	public float octaves;
	public float persistence;
	public float lacunarity;
	public int seed;
}
