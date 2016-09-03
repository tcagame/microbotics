using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class YesButton : MonoBehaviour {
	public void GoToGame( ) {
		SceneManager.LoadScene( "GamePlay" );
	}
}
