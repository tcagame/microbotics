using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Touch : MonoBehaviour {
	void Update( ) {
		if ( Input.touchCount > 0 ) {
			SceneManager.LoadScene( "GamePlay" );
		}
		if ( Input.GetMouseButtonDown( 0 ) ) {
			SceneManager.LoadScene ( "GamePlay" );
		}
	}
}
