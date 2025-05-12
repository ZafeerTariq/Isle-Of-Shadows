using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public GameObject player;
	private PlayerCombat playerCombatScript;

	public GameObject gameOverScreen;
	public GameObject mainMenuButton;

	void Start() {
		playerCombatScript = player.GetComponent<PlayerCombat>();

		playerCombatScript.onDeath += GameOver;
		gameOverScreen.SetActive( false );
		mainMenuButton.SetActive( false );
	}

	void Update() {
		if( Input.GetKeyDown( KeyCode.Escape ) ) {
			mainMenuButton.SetActive( !mainMenuButton.activeSelf );
		}
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
