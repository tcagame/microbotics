﻿using UnityEngine;
using System.Collections;

public class YesButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoToGame( ) {
		Application.LoadLevel ("GamePlay");
	}
}
