using UnityEngine;
using System.Collections;

public class Propellers : MonoBehaviour {

	private GameObject _propeller;
	private GameObject _fan;
	private GameObject _fan_range;
	public string PropellerName;
	public string FanName;
	public string RangeName;
	private GameObject _player;

	bool _tracking_flag;
	bool _flying_flag;

    private const float CONNECT_RANGE = 2.0f;
	private const float FLY_SPEED = 1.5f;


	void Start ( ) {
		_propeller = GameObject.Find( PropellerName ).gameObject;
		_fan = GameObject.Find( FanName ).gameObject;
		_fan_range = GameObject.Find (RangeName).gameObject;
		_player = GameObject.Find ( "Player" ).gameObject;
		_tracking_flag = false;
		_flying_flag = false;
	}

	void Update ( ) {
		judgeTracking( );
		if ( _tracking_flag ) {
			setPosition( );
		}
		if (_flying_flag) {
			setFlying();
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
			player_pos.y += 1.3f;
			_propeller.transform.position = player_pos;
		}
	}

	private void setFlying( ) {
		Vector3 player_pos = _player.transform.position;
		player_pos.y += FLY_SPEED;
		if (isFindPos()) {
			player_pos.z += FLY_SPEED;
		}
		_player.transform.position = player_pos;
	}

	private bool isFindPos( ) {
		Vector3 fan_range_pos = _fan_range.transform.position;
		Vector3 fan_range_scale = _fan_range.transform.localScale;
		if ( ( ( fan_range_pos.x - fan_range_scale.x / 2 ) < _fan.transform.position.x ) &&
			 ( ( fan_range_pos.x + fan_range_scale.x / 2 ) > _fan.transform.position.x ) &&
			 ( ( fan_range_pos.y - fan_range_scale.y / 2 ) < _fan.transform.position.y ) &&
			 ( ( fan_range_pos.y + fan_range_scale.y / 2 ) > _fan.transform.position.y ) &&
			 ( ( fan_range_pos.z - fan_range_scale.z / 2 ) < _fan.transform.position.z ) &&
			 ( ( fan_range_pos.z + fan_range_scale.z / 2 ) > _fan.transform.position.z ) ) {
			return true;
		}
		return false;
	}
	public void isPlay( ) {
		_flying_flag = true;
	}

	public bool getFlag( ) {
		return _flying_flag;
	}
}
