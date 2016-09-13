using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartBotton : MonoBehaviour {
	/*public void GoToSelect( ) {
		SceneManager.LoadScene( "GamePlay" );
	}*/
	public void GoToSelect( ) {
		SceneManager.LoadScene( "GameInfo" );
	}
}
