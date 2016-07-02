using UnityEngine;
using System.Collections;

public class NoButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void goToTitle( ) {
		Application.LoadLevel ("Title");
	}
}
