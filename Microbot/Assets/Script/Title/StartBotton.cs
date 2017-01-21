using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartBotton : MonoBehaviour {
	public float Timer;
	private bool _is_click = false;
	public GameObject Gear;
	void Start( ) {
		if ( Timer <= 0 ) {
			Timer = 1;
		}
		Timer *= 60;
	
	}

	void Update( ) {
		if( !_is_click ){
			return;
		}
		Timer--;
		if ( Timer <= 0 ) {
			SceneManager.LoadScene( "GameInfo" );
		}
	}
	public void GoToSelect( ) {
		_is_click = true;
		Gear.AddComponent< TrunGear >( ).RotateAngle = 1f;
	}
}
