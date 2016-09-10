using UnityEngine;
using System.Collections;

public class Propellers : MonoBehaviour {

	private GameObject _propeller;
	public string PropellerName;
	public string FanName;
	private GameObject _player;
	bool _flying_flag;

    public const float FLY_SPEED = 0.3f;
	public float POWER = 10;

	private Vector3 PROPELLER_START_POS = new Vector3 ( -15.0f, 1.0f, 0.0f);
	private Vector3 PROPELLER_GOAL_POS = new Vector3 ( -15.0f, 3.5f, -13.0f);
	private float MAX_HEIGHT = 5.0f;
	private bool _on_fly;

	private bool _start_flag;

	void Start ( ) {
		_propeller = GameObject.Find( PropellerName ).gameObject;
		_propeller.transform.position = PROPELLER_START_POS;
		_player = GameObject.Find ( "Player" ).gameObject;
		_flying_flag = false;
		_start_flag = true;
		_on_fly = false;
	}

	void Update ( ) {
		if (_flying_flag ) {
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
		if (_start_flag) {
			target_pos = new Vector3( PROPELLER_GOAL_POS.x, MAX_HEIGHT, PROPELLER_GOAL_POS.z );
			vec = target_pos - _propeller.transform.position;
		} else {
			target_pos = new Vector3( PROPELLER_START_POS.x, MAX_HEIGHT, PROPELLER_START_POS.z );
			vec = target_pos - _propeller.transform.position;
		}

		//ここでファンが動いているかみる
		FanManager fun = GameObject.Find( FanName ).GetComponent< FanManager >();
		bool awake_fan = fun.getFlag( );
		if ( awake_fan ) {
			//動いていたら並行移動する
			if (vec.magnitude < FLY_SPEED) {
				_propeller.transform.position += vec.normalized * FLY_SPEED;
			} else {
				_propeller.transform.position += vec;
			}
			return;
		}

		//動いていなかったらそのまま下に降りる
		//もしくは移動し行ったら下に降りる
		if ( !awake_fan || vec.magnitude == 0 ) {
			//目標の高さを求める
			float target_height;
			if ( _propeller.transform.position == new Vector3( PROPELLER_GOAL_POS.x, MAX_HEIGHT, PROPELLER_GOAL_POS.z ) ) {
				target_height = PROPELLER_GOAL_POS.y;
			} else {
				target_height = PROPELLER_START_POS.y;
			}
			//降りていく
			if (_propeller.transform.position.y > target_height ) {
				float length = _propeller.transform.position.y - target_height;
				if (length > FLY_SPEED) {
					_propeller.transform.position += Vector3.down * FLY_SPEED;
				} else {
					_propeller.transform.position += Vector3.down * length;
				}
			}
		}

		//移動終了処理
		if ( PROPELLER_GOAL_POS == _propeller.transform.position || PROPELLER_START_POS == _propeller.transform.position ) {
			_start_flag = !_start_flag;
			_flying_flag = false;
			_on_fly = false;
		}
	}
}


