using UnityEngine;
using System.Collections;

public class SmallFanManager : MonoBehaviour {
	private bool _flag;
	private Animator _animetor;

	void Start( ) {
		_flag = false;
		_animetor = GetComponent< Animator >( );
	}
	
	void Update( ) {
		
	}

	public void action( ) {
		if ( !_flag ) {
			_animetor.SetBool( "Start", true );
		}
		_flag = true;
	}

	public bool getFlag( ) {
		return _flag;
	}
}
