using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NoButton : MonoBehaviour {
	public void goToTitle( ) {
		SceneManager.LoadScene( "GameTitle" );
	}
}
