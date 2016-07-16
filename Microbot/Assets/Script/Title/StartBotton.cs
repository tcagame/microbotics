using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartBotton : MonoBehaviour {
	public void GoToSelect( ) {
		SceneManager.LoadScene( "StageSelect" );
	}
}
