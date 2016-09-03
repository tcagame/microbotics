using UnityEngine;
using System.Collections;

public class FanSwitch : MonoBehaviour {

	private bool _flag;
	private GameObject _fan;
	public string FanName = "Fan";

	void Start( ) {
		_flag = false;
		_fan = GameObject.Find (FanName);
	}

	void Update( ) {
		if ( _flag ) {
			_fan.GetComponent< FanManager > ().action ();
		}
	}
	public void isPlay( ) {
		_flag = true;
	}

	public bool getFlag( ) {
		return _flag;
	}
}
