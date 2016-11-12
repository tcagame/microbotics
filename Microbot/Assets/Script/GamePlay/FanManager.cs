using UnityEngine;
using System.Collections;

public class FanManager : MonoBehaviour {

	public float Power = 1000;
	private bool _flag;
	private GameObject _trap;
	private Animator _animetor;
	public string TrapName = "SEKITAN";
    //private int timer;

	void Start( ) {
		_flag = false;
		_trap = GameObject.Find( TrapName ).gameObject;
		_animetor = GetComponent< Animator >( );
	}
	
	void Update( ) {
		if ( _flag ) {
			
			if ( _trap != null )
            {
                Vector3 pos = _trap.transform.position;
				pos.x++;
				_trap.transform.position = pos;
            }
		}
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
