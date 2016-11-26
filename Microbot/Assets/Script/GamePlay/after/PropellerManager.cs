using UnityEngine;
using System.Collections;

public class PropellerManager : MonoBehaviour {
	private enum STATE {
		STATE_NONE,
		STATE_UP,
		STATE_DOWN,
		STATE_LEAVE
	}

	private GameObject _propeller;
	public string PropellerName;
	private GameObject _player;
	public string PlayerName;

	public const float FLY_SPEED = 0.05f;

	public Vector3 PROPELLER_LOW_POS = new Vector3( -15.0f, 1.8f, 0.0f );
	public Vector3 PROPELLER_HIGH_POS = new Vector3 ( -15.0f, 6.0f, -13.0f );
	private float MAX_HIGH = 8.0f;

	private bool _flag;
	private STATE _state;



	void Awake( ) {
		_propeller = GameObject.Find (PropellerName).gameObject;
		_propeller.gameObject.transform.position = PROPELLER_LOW_POS;
	}
	// Use this for initialization
	void Start( ) {
		_player = GameObject.Find( PlayerName ).gameObject;
		_state = STATE.STATE_NONE;
	}
	
	// Update is called once per frame
	void Update( ) {
		if ( _flag ) {
			//ロボットの頭上にプロペラ設置
			setPosition( );
			//飛ぶ操作
			flying( );
		}
	}

	private void setPosition( ) {
		Vector3 player_pos = _player.transform.position;
		Vector3 propeller_pos = _propeller.transform.position;
		player_pos = new Vector3( propeller_pos.x, propeller_pos.y - 1.3f, propeller_pos.z );
		_player.transform.position = player_pos;
	}

	private void flying( ) {
		//方向決め
		Vector3 dir;
		Vector3 mid_pos = ( PROPELLER_HIGH_POS - PROPELLER_LOW_POS ) / 2;
		mid_pos.y = MAX_HIGH;
		switch ( _state ) {
		case STATE.STATE_UP:
			dir = mid_pos - PROPELLER_LOW_POS;
			moveOnDir( dir );
			switchDir( mid_pos );
			break;
		case STATE.STATE_DOWN:
			dir = PROPELLER_HIGH_POS - mid_pos;
			moveOnDir( dir );
			switchDir( PROPELLER_HIGH_POS );
			break;
		case STATE.STATE_LEAVE:
			leavePropeller( );
			break;
		}
	}

	private void moveOnDir( Vector3 dir ) {
		Vector3 propeller_pos = _propeller.transform.position;
		propeller_pos += dir.normalized * FLY_SPEED;
		_propeller.transform.position = propeller_pos;
	}

	private void switchDir( Vector3 target_pos ) {
		Vector3 mid_pos = target_pos;
		Vector3 propeller_pos = _propeller.transform.position;
		float dist = Vector3.Distance( mid_pos, propeller_pos );
		if (dist < 1) {
			_state++;
		}
	}

	private void leavePropeller( ) {
		_propeller.transform.position = PROPELLER_LOW_POS;
		_player.transform.position = PROPELLER_LOW_POS;
		_flag = false;
	}

	//イベントマネージャーから操作
	public void action( ) {
		_flag = true;
		_state = STATE.STATE_UP;
	}
	public bool isActive( ) {
		return _flag;
	}
}
