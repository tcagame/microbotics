using UnityEngine;
using System.Collections;

public class Propeller : MonoBehaviour {

	private GameObject _propeller;
	public string ObjectName;
	private GameObject _player;

	bool _tracking_flag;

    private const float CONNECT_RANGE = 2.0f;

	void Start ( ) {
		_propeller = GameObject.Find( ObjectName ).gameObject;
		_player = GameObject.Find ( "Player" ).gameObject;
		_tracking_flag = false;
	}

	void Update ( ) {
		judgeTracking( );
		if ( _tracking_flag ) {
			setPosition ( );
		}
	}

	private void judgeTracking( ) {
		Vector3 player_pos = _player.transform.position;
		Vector3 propeller_pos = _propeller.transform.position;


		float distance = Vector3.Distance( player_pos, propeller_pos  );
		if ( distance < CONNECT_RANGE ) {
			_tracking_flag = true;
		} else {
			_tracking_flag = false;
		}
	}

	private void setPosition( ) {
		if ( _tracking_flag ) {
			Vector3 player_pos = _player.transform.position;
			player_pos.y += 1.5f;
			_propeller.transform.position = player_pos;
		}
	}
}
