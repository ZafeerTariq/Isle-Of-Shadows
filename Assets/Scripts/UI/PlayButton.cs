using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {
	public TMP_InputField seedInput;
	public NoiseDataScriptableObject terrainNoise;
	public NoiseDataScriptableObject regionNoise;

	public void StartGame() {
		int seed;
		if( int.TryParse( seedInput.text, out seed ) ) {}
		else seed = Random.Range( 0, int.MaxValue );

		System.Random prng = new System.Random( seed );

		terrainNoise.seed = prng.Next( 0, int.MaxValue );
		regionNoise.seed  = prng.Next( 0, int.MaxValue );

		SceneManager.LoadScene( "Game" );
	}
}
