using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			SceneManager.LoadScene ( "Title" );
		}
		if (Input.GetMouseButtonDown (0)) {
			SceneManager.LoadScene ( "Title" );
		}
	}
}
