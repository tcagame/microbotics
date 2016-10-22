using UnityEngine;
using System.Collections;

public class PropellerFanManager : MonoBehaviour {

	bool _active;
	private Animator _animetor;
	// Use this for initialization
	void Start () {
		_active = false;
		_animetor = GetComponent< Animator >( );
	}
	
	// Update is called once per frame
	public void action( ) {
		_active = true;
		_animetor.SetBool( "Start", true );
	}
	public void isActive( ) {
		return _active;
	}
}
