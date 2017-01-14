using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour {
	void Update( ) {
		if ( Input.touchCount > 0 ) {
			SceneManager.LoadScene( "Title" );
		}
		if ( Input.GetMouseButtonDown( 0 ) ) {
			SceneManager.LoadScene ( "Title" );
		}
	}
}
