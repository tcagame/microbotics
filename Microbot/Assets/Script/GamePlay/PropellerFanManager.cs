using UnityEngine;
using System.Collections;

public class PropellerFanManager : MonoBehaviour {

	bool _active;
	private Animator _animetor;
	// Use this for initialization
	void Start () {
        _animetor = GetComponent< Animator >( );
		_active = false;
	}

	// Update is called once per frame
	public void action( ) {
		_active = true;
        _animetor.SetBool( "Start", true );
	}

	public bool isActive( ) {
		return _active;
	}
}
