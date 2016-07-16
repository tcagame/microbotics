using UnityEngine;
using System.Collections;

public class YesButton : MonoBehaviour {
	public void GoToGame( ) {
		Application.LoadLevel( "GamePlay" );
	}
}
