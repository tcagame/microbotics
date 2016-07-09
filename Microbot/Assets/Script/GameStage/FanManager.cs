using UnityEngine;
using System.Collections;

public class FanManager : MonoBehaviour {

	private bool _flag;
	private GameObject _trap;
	public string TrapName = "SEKITAN";
	// Use this for initialization
	void Start () {
		_flag = false;
		_trap = GameObject.Find (TrapName).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (_flag) {
			Destroy (_trap);
		}
	}

	public void isPlay( ) {
		_flag = true;
	}

	public bool getFlag( ) {
		return _flag;
	}
}
