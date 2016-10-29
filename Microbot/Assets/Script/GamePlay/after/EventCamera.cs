using UnityEngine;
using System.Collections;

public class EventCamera{
	private double WAIT_TIME = 5 * 60;
	private double _time;
	private GameObject _mine;

	public EventCamera( ) {
		_time = 0;
	}

	public void update( ) {
		_time += Time.deltaTime;
	}

	public void callEventCamera( Vector3 pos, Vector3 target ) {
		if ( _time != 0 ) {
			return;
		}
		_mine = GameObject.FindGameObjectWithTag( "MainCamera" );
		if ( _mine == null ) {
			_time = WAIT_TIME;
			return;
		}
		_mine.transform.position = pos;
		_mine.transform.LookAt( target );
	}

	public bool isfinish( ) {
		bool result = false;
		if ( _time > WAIT_TIME ) {
			result = true;
		}
		return result;
	}
}
