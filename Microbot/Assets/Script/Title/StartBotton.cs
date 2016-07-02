using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartBotton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoToSelect( ) {
		SceneManager.LoadScene ( "StageSelect" );
	}
}
