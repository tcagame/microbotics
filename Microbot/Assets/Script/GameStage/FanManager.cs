using UnityEngine;
using System.Collections;

public class FanManager : MonoBehaviour {

	private bool _flag;
	private GameObject _trap;
	public string TrapName = "SEKITAN";

	void Start( ) {
		_flag = false;
		_trap = GameObject.Find( TrapName ).gameObject;
	}
	
	void Update( ) {
		if ( _flag ) {
			Destroy ( _trap );
		}
	}

	public void isPlay( ) {
		_flag = true;
	}

	public bool getFlag( ) {
		return _flag;
	}
}
