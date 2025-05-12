using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public GameObject player;
	private PlayerCombat playerCombatScript;
	private PlayerController playerControllerScript;

	public GameObject gameOverScreen;

	void Start() {
		playerCombatScript = player.GetComponent<PlayerCombat>();
		playerControllerScript = player.GetComponent<PlayerController>();

		playerCombatScript.onDeath += GameOver;
		gameOverScreen.SetActive( false );
	}

	private void GameOver() {
		gameOverScreen.SetActive( true );
	}

	void OnDestroy() {
        if ( playerCombatScript )
            playerCombatScript.onDeath -= GameOver;
    }

	public void LoadMainMenu() {
		SceneManager.LoadScene( "Main Menu" );
	}
}
