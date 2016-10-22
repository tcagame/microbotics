using UnityEngine;
using System.Collections;

public class CoalFanManager : MonoBehaviour {
	bool _active;
	private Animator _animetor;
	// Use this for initialization
	void Start () {
		_active = false;
		_animetor = GetComponent< Animator >( );
	}

	public void action( ) {
		_active = true;
		_animetor.SetBool( "Start", true );
	}
	public void isActive( ) {
		return _active;
	}
}
