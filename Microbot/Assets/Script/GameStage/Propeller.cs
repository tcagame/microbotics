using UnityEngine;
using System.Collections;

public class Propeller : MonoBehaviour {

	private GameObject _propeller;
	public string ObjectName;
	private GameObject _player;

	bool _tracking_flag;

	void Start ( ) {
		_propeller = GameObject.Find( ObjectName ).gameObject;
		_player = GameObject.Find ("Player").gameObject;
		_tracking_flag = false;
	}

	void Update ( ) {
		judgeTracking( );
	}

	private void judgeTracking( ) {
		Vector3 player_pos = _player.transform.position;
		Vector3 propeller_pos = _propeller.transform.position;

		if ( player_pos == propeller_pos ) {
			_tracking_flag = true;
		}
	}

	private void setPosition( ) {
		if ( _tracking_flag ) {
			Vector3 player_pos = _player.transform.position;
			player_pos.x += 0.5f;
			_propeller.transform.position = player_pos;
		}
	}
}
