using UnityEngine;
using System.Collections;

public class FanManager : MonoBehaviour {

	public float Power = 10;
	private bool _flag;
	private GameObject _trap;
	private Animator _animetor;
	public string TrapName = "SEKITAN";

	void Start( ) {
		_flag = false;
		_trap = GameObject.Find( TrapName ).gameObject;
		_animetor = GetComponent< Animator >( );
	}
	
	void Update( ) {
		if ( _flag ) {
			Vector3 pos = transform.position - _trap.transform.position ;
			_trap.GetComponent< Rigidbody >( ).AddForce( pos.x * Power, pos.y * Power, pos.z * Power );
			_animetor.SetBool( "Start", true );
		}
	}

	public void isPlay( ) {
		_flag = true;
	}

	public bool getFlag( ) {
		return _flag;
	}
}
