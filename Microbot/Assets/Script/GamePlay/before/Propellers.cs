using UnityEngine;
using System.Collections;

public class Propellers : MonoBehaviour {

	private GameObject _propeller;
	public string PropellerName;
	public string FanName;
	private GameObject _player;
	bool _flying_flag;

    public const float FLY_SPEED = 0.05f;
	public const float DOWN_SPEED = 0.05f;
	public float POWER = 10;

	private Vector3 STAGE_POS = new Vector3( -17.5f, 13.7f, 1.53f );
	private Vector3 PROPELLER_START_POS = new Vector3 ( -15.0f, 1.8f, 0.0f);
	private Vector3 PROPELLER_GOAL_POS = new Vector3 ( -15.0f, 6.0f, -13.0f);
	private float MAX_HEIGHT = 9.0f;
	private float MIN_START_HEIGHT = 1.0f;
	private float MIN_GOAL_HEIGHT = 7.5f;
	private bool _on_fly;
	private bool _on_down;

	private bool _start_flag;

	void Start ( ) {
		_propeller = GameObject.Find( PropellerName ).gameObject;
		_propeller.transform.position = PROPELLER_START_POS;
		_player = GameObject.Find ( "Player" ).gameObject;
		_flying_flag = false;
		_start_flag = true;
		_on_fly = false;
		_on_down = false;
	}

	void Update ( ) {
		if ( _flying_flag ) {
			flying( );
			setPosition( );
		}
	}

	private void setPosition( ) {
		Vector3 player_pos = _player.transform.position;
		Vector3 p_pos = _propeller.transform.position;
		player_pos = new Vector3( p_pos.x, p_pos.y - 1.3f, p_pos.z );
		_player.transform.position = player_pos;
	}

	public void isPlay( ) {
		_flying_flag = true;
		_on_fly = false;
	}

	public bool getFlag( ) {
		return _flying_flag;
	}

	private void flying( ) {
		//上昇
		if ( _propeller.transform.position.y < MAX_HEIGHT && !_on_fly ) {
			float length = MAX_HEIGHT - _propeller.transform.position.y;
			if (length > FLY_SPEED) {
				_propeller.transform.position += Vector3.up * FLY_SPEED;
			} else {
				_propeller.transform.position += Vector3.up * length;
			}
			return;
		}

		if ( _propeller.transform.position.y == MAX_HEIGHT ) {
			_on_fly = true;
		}

		//ここでスタートゴール間のベクトルをとる。
		Vector3 vec;
		Vector3 target_pos; 
		if ( _start_flag ) {
			target_pos = new Vector3( PROPELLER_GOAL_POS.x, MAX_HEIGHT, PROPELLER_GOAL_POS.z );
			vec = target_pos - new Vector3( _propeller.transform.position.x, MAX_HEIGHT, _propeller.transform.position.z );
		} else {
			target_pos = new Vector3( PROPELLER_START_POS.x, MAX_HEIGHT, PROPELLER_START_POS.z );
			vec = target_pos - new Vector3( _propeller.transform.position.x, MAX_HEIGHT, _propeller.transform.position.z );
		}

		//ここでファンが動いているかみる
		SmallFanManager fun = GameObject.Find( FanName ).GetComponent< SmallFanManager >();
		bool awake_fan = fun.getFlag( );
		if ( awake_fan ) {
			//動いていたら並行移動する
			if ( vec.magnitude < FLY_SPEED && !_on_down ) {
				_propeller.transform.position += vec.normalized * FLY_SPEED;
			} else if ( !_on_down ) {
				_propeller.transform.position += vec * FLY_SPEED;
			}
		}

		//動いていなかったらそのまま下に降りる
		//もしくは移動し行ったら下に降りる
		if ( !awake_fan || vec.magnitude <= 0.15f ) {
			_on_down = true;
			//目標の高さを求める
			float target_height;
			float dist = Vector3.Distance( new Vector3( PROPELLER_GOAL_POS.x, MAX_HEIGHT, PROPELLER_GOAL_POS.z ), _propeller.transform.position  );
			if (  dist < 0.3f  ) {
				target_height = PROPELLER_GOAL_POS.y;
			} else {
				target_height = PROPELLER_START_POS.y;
			}
			//降りていく
			if ( _propeller.transform.position.y > target_height ) {
				float length = _propeller.transform.position.y - target_height;
				if ( length > DOWN_SPEED) {
					_propeller.transform.position += Vector3.down * DOWN_SPEED;
				} else {
					_propeller.transform.position += Vector3.down * length;
				}
			}
		}

		//移動終了処理
	
		float dist_start = Vector3.Distance( new Vector3( PROPELLER_START_POS.x, 0, PROPELLER_START_POS.z ),
											 new Vector3( _propeller.transform.position.x, 0, _propeller.transform.position.z ) );
		if ( dist_start < 0.3f && _player.transform.position.y <= MIN_START_HEIGHT ) {
			_start_flag = true;
			_flying_flag = false;
			_on_fly = false;
			_on_down = false;
			_propeller.transform.position = PROPELLER_START_POS;
			_player.transform.position = _propeller.transform.position + Vector3.left;
			return;
		}
		float dist_goal = Vector3.Distance( new Vector3( PROPELLER_GOAL_POS.x, 0, PROPELLER_GOAL_POS.z ),
											new Vector3( _propeller.transform.position.x, 0, _propeller.transform.position.z ) );
		if ( dist_goal < 0.3f && _player.transform.position.y  <= MIN_GOAL_HEIGHT ) {
			_start_flag = false;
			_flying_flag = false;
			_on_fly = false;
			_on_down = false;
			_propeller.transform.position = PROPELLER_GOAL_POS;
			_player.transform.position = _propeller.transform.position + Vector3.left;
			return;
		}
	}
}


