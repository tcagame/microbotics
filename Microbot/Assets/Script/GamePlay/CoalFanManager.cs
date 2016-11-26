using UnityEngine;
using System.Collections;

public class CoalFanManager : MonoBehaviour {
	bool _active;
	private Animator _animetor;

	void Awake( ) {
		_active = false;
		_animetor = GetComponent< Animator >( );
	}

	void Start( ) {
		
	}

	public void action( ) {
		_active = true;
		_animetor.SetBool( "Start", true );
	}

	public bool isActive( ) {
		return _active;
	}
}
