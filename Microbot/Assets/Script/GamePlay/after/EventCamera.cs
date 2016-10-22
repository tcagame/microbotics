using UnityEngine;
using System.Collections;

public class EventCamera : Monobehavior {
	private double WAIT_TIME = 5 * 60;
	private double _time;
	private Camera _mine;

	void Awake( ) {
		_time = 0;
		_mine = gameObject.GetComponent< Camera >( );
	}

	void Update( ) {
		_time += Time.deltaTime;
	}

	public void CallEventCamera( Vector3 pos, Vector3 target ) {
		if ( _time != 0 ) {
			return;
		}
		_mine.transform.position = pos;
		_mine.transform.LookAt( target );
	}

	public bool finish( ) {
		bool result = false;
		if ( _time > WAIT_TIME ) {
			result = true;
		}
		return result;
	}
}
