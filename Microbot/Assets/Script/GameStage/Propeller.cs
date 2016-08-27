using UnityEngine;
using System.Collections;

public class Propellers : MonoBehaviour {

	private GameObject _propeller;
	private GameObject _fan;
	public string PropellerName;
	public string FanName;

	private GameObject _player;
	private float _length;

	bool _tracking_flag;
	bool _flying_flag;

	void Start ( ) {
		_propeller = GameObject.Find( PropellerName ).gameObject;
		_fan = GameObject.Find( FanName ).gameObject;
		_player = GameObject.Find ("Player").gameObject;
		_tracking_flag = false;
		_length = 1.0f;
	}

	void Update ( ) {
		judgeTracking( );
		if (_tracking_flag) {
			setPosition();
		}
		if (_flying_flag) {
			playerOnFlying ();
		}
	}

	private void judgeTracking( ) {
		Vector3 player_pos = _player.transform.position;
		player_pos.y = 0.0f;
		Vector3 propeller_pos = _propeller.transform.position;
		propeller_pos.y = 0.0f;

		float distance = Vector3.Distance( player_pos, propeller_pos );

		if ( distance < _length ) {
			_tracking_flag = true;
		}
	}

	private void setPosition( ) {
		Vector3 player_pos = _player.transform.position;
		player_pos.y += 1.5f;
		_propeller.transform.position = player_pos;
	}

	private void playerOnFlying( ) {
		Vector3 player_pos = _player.transform.position;
		player_pos.y += 1.0f * Time.deltaTime;
		if (isFanRange ()) {
			player_pos.x += 1.0f * Time.deltaTime;
		}
		_player.transform.position = player_pos;
	}

	private bool isFanRange( ) {
		Vector3 fan_pos = _fan.transform.position;
		Vector3 player_pos = _player.transform.position;

		if ( (player_pos.x > fan_pos.x ) && ( player_pos.x < fan_pos.x + 3.0f ) &&
			( player_pos.y > fan_pos.y ) && ( player_pos.y < fan_pos.y + 3.0f ) ) {
			return true;
		}
		return false;
	}
}
