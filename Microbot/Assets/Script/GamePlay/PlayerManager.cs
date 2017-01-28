using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public enum PLAYER_STATE {
	PLAYER_STATE_STAY,
	PLAYER_STATE_RUN,
	PLAYER_STATE_CHARGE,
	PLAYER_STATE_DISCHARGE,
	PLAYER_STATE_CLIMB,
	PLAYER_STATE_CLIMB_HIGH,
	PLAYER_STATE_CLEAR,
	PLAYER_STATE_DEAD,
}

public class PlayerManager : MonoBehaviour {

	const float POS_DIFF = 0.6f;
	public float WalkMaxSpeed = 10f;
	public float WalkMinSpeed = 0.2f;
	public int GAME_WAIT_TIME = 2;
	public float clim_speed = 0.3f;

	private const float GAUGE_MAX = 300.0f;
	private float _gauge;
	[SerializeField]private float _move_max_time = 1
		;
	//private Touch _touch;
	private Operation _operation;
	private float _move_time = 0;
	private float _walk_speed = 0.0f;
	private Vector3 _target_pos = new Vector3 ( );
	private int _check_first_touch = 0;
	private PLAYER_STATE _player_state;
	private bool _climbed_normal;
	private bool _climbed_high;
	private bool _discharge;
	private bool _clear;
	private int _game_end_time = 0;
	private Vector3 _last_ground_pos;
	[SerializeField]private string _hit_object_tag;
	[SerializeField]private GameObject _hit_object;

	//ポイント
	GameObject _point;
	//Dust
	private GameObject _dust;

	RaycastHit hit;
	private Animator _animator;
	[SerializeField]private int _animation_time;

	void Awake(){
		_dust = GameObject.Find ("Dust").gameObject;
		_animator = gameObject.GetComponent<Animator>( );
		_point = ( GameObject )Resources.Load( "Prefab/Point" );
		_point = Instantiate( _point );
		_point.SetActive( false );
		_climbed_normal = false;
		_clear = false;
	}

	// Use this for initialization
	void Start () {
		_hit_object_tag = "";
		_operation = GameObject.Find( "GameManager" ).GetComponent< Operation >( );
		_gauge = GAUGE_MAX;
		_dust.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		updateGauge ();
		setRunEffect ( );
		setAnimation ( );
		checkClimb ( );

		if ( _player_state == PLAYER_STATE.PLAYER_STATE_RUN || 
			_player_state == PLAYER_STATE.PLAYER_STATE_STAY ) {
			move ();
			_last_ground_pos = transform.position;
		}
		if ( _player_state == PLAYER_STATE.PLAYER_STATE_CLIMB ||
			_player_state == PLAYER_STATE.PLAYER_STATE_CLIMB_HIGH ) {
			climb ();
		}
		AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo( 0 );
		if ( _gauge <= 0 ) {
			_player_state = PLAYER_STATE.PLAYER_STATE_DEAD;
		}
		if ( _clear ) {
			_player_state = PLAYER_STATE.PLAYER_STATE_CLEAR;
		}
		if ( _clear || _gauge <= 0 ) {
			_game_end_time++;
		}
	}

	void OnCollisionEnter( Collision col ) {
		_hit_object = col.gameObject;
		_hit_object_tag = col.gameObject.tag;
	}

	void updateGauge( ) {
		discharge( );

		//GaugeのUpdate
		switch (_player_state) {
		case PLAYER_STATE.PLAYER_STATE_STAY:
			break;
		case PLAYER_STATE.PLAYER_STATE_RUN:
			_gauge -= 0.05f;
			break;
		case PLAYER_STATE.PLAYER_STATE_CLIMB:
			_gauge -= 0.1f;
			break;
		case PLAYER_STATE.PLAYER_STATE_CLIMB_HIGH:
			_gauge -= 0.15f;
			break;
		}

		//画像の更新
		for (float i = 0; i < 5f; i++) {
			string per_name = "Per" + ( i + 1f ).ToString ();
			if (_gauge < GAUGE_MAX * i * 0.2f ) {
				GameObject.Find ( per_name ).gameObject.GetComponent<Image> ().enabled = false;
			} else {
				GameObject.Find ( per_name ).gameObject.GetComponent<Image> ().enabled = true;
			}
		}
	}

	private void climb( ) {
		if ( transform.position.y > ( _hit_object.transform.position.y +  ( _hit_object.transform.localScale.y ) ) ) {
			if ( _player_state == PLAYER_STATE.PLAYER_STATE_CLIMB ) {
				_climbed_normal = true;
			}
			if ( _player_state == PLAYER_STATE.PLAYER_STATE_CLIMB_HIGH ) {
				_climbed_high = true;
			}
			gameObject.GetComponent<Rigidbody> ().useGravity = true;
			_player_state = PLAYER_STATE.PLAYER_STATE_STAY;

		}
		float move_y = ( ( _hit_object.transform.position.y + _hit_object.transform.localScale.y ) - ( _last_ground_pos.y - transform.localScale.y  ) ) / 100;
		move_y *= clim_speed;
		Vector3 pos = _hit_object.transform.position - gameObject.transform.position;
		pos.Normalize ();
		pos *= 0.03f;
		pos.y = move_y;
		pos += transform.position;
		transform.position = pos;
	}

	private void discharge ( ) {
		if ( _hit_object_tag == "Jack" && !_discharge ) {
			_gauge -= 10f;
			_discharge = true;
		}
		if ( _hit_object_tag == "Propera" && !_discharge ) {
			_gauge -= 10f;
			_discharge = true;
		}
		if ( _hit_object_tag == "ProperaFunSwitch" && !_discharge ) {
			_gauge -= 10f;
			_discharge = true;
		}
		if ( _hit_object_tag == "CoalFunSwitch" && !_discharge ) {
			_gauge -= 10f;
			_discharge = true;
		}
		if ( _hit_object_tag == "Charger" && _gauge < 100f ) {
			_gauge += 1f;
		}
		if ( _hit_object_tag == "Untagged" ) {
			_discharge = false;
		}
	}

	private void checkClimb ( ) {
		if (_hit_object_tag != "CanClimb") {
			gameObject.GetComponent<Rigidbody> ().useGravity = true;
			_climbed_high = false;
			_climbed_normal = false;
			return;
		}
		if (_hit_object.transform.localScale.y < 2 && !_climbed_normal) {
			_player_state = PLAYER_STATE.PLAYER_STATE_CLIMB;
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
		} else if (_hit_object.transform.localScale.y > 2 && !_climbed_high) {
			_player_state = PLAYER_STATE.PLAYER_STATE_CLIMB_HIGH;
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
		} 
	}

	private void setAnimation() {
		switch ( _player_state ) {
		case PLAYER_STATE.PLAYER_STATE_STAY:
			aniamationReset ();
			break;
		case PLAYER_STATE.PLAYER_STATE_RUN:
			if ( !_animator.GetBool( "_is_running" ) ) {
				aniamationReset ( );
			}
			_animator.SetBool ("_is_running", true);
			break;
		case PLAYER_STATE.PLAYER_STATE_CLIMB:
			if ( !_animator.GetBool ("_is_climbing_normal")) {
				aniamationReset ();
			}
			_animator.SetBool ("_is_climbing_normal", true);
			break;
		case PLAYER_STATE.PLAYER_STATE_CLIMB_HIGH:
			if ( !_animator.GetBool ("_is_climbing_high")) {
				aniamationReset ();
			}
			_animator.SetBool ("_is_climbing_high", true);
			break;
		case PLAYER_STATE.PLAYER_STATE_CLEAR:
			if( !_animator.GetBool( "_clear" ) ) {
				aniamationReset( );
			}
			_animator.SetBool( "_clear", true );
			break;
		case PLAYER_STATE.PLAYER_STATE_DEAD:
			if( !_animator.GetBool( "_dead" ) ) {
				aniamationReset( );
			}
			_animator.SetBool( "_dead", true );
			break;
		default:
			break;
		}
	}

	private void aniamationReset( ) {
		_animator.SetBool ("_is_running", false);
		_animator.SetBool ("_is_charging", false);
		_animator.SetBool ("_is_discharging", false);
		_animator.SetBool ("_is_climbing_high", false);
		_animator.SetBool ("_is_climbing_normal", false);
	}

	private void setRunEffect( ) {
		if (_player_state == PLAYER_STATE.PLAYER_STATE_RUN) {
			_dust.SetActive (true);
		} else {
			_dust.SetActive (false);
		}
	}
		

	public string getTouchObjectTag( ){
		return _hit_object_tag;
	}

	public float getGauge( ) {
		return _gauge;
	}

	public void setPlayerState( PLAYER_STATE state ) {
		_player_state = state;
	}

	private void moveToTarget ( Vector3 pos, float walk_speed ) {
		transform.position = Vector3.MoveTowards ( transform.position, pos, walk_speed );
		transform.LookAt ( pos );
	}

	private void move( ) {
		Vector3 rayhit_pos = _operation.getHitRaycastPos( );
		if ( rayhit_pos != new Vector3( ) ) {
			_target_pos = new Vector3 ( rayhit_pos.x, transform.position.y, rayhit_pos.z );
			_walk_speed = Vector3.Distance( _target_pos, transform.position ) / ( _move_max_time * 60 ) ;	
			_check_first_touch++;
		} else {
			_check_first_touch = 0;	
		}

		if ( _walk_speed > WalkMaxSpeed ) {
			_walk_speed = WalkMaxSpeed;
		}
		if ( _walk_speed < WalkMinSpeed ) {
			_walk_speed = WalkMinSpeed;
		}
		if ( _target_pos != new Vector3( ) ) {
			moveToTarget ( _target_pos, _walk_speed );
			_player_state = PLAYER_STATE.PLAYER_STATE_RUN;
			_move_time++;
			setPoint( _target_pos );
		} else {
			_player_state = PLAYER_STATE.PLAYER_STATE_STAY;
			deletePoint( );
		}
		Vector3 diff_pos = transform.position - _target_pos;
		diff_pos.y = 0;
		if ( diff_pos.magnitude < POS_DIFF  ) {
			_move_time = 0;
			_operation.resetTargetPos ( );
			_target_pos = new Vector3( );
			_player_state = PLAYER_STATE.PLAYER_STATE_STAY;
		}
	}

	private void setPoint( Vector3 pos ) {
		_point.SetActive( true );
		_point.transform.position = pos + ( _point.transform.up * 0.01f );
	}

	private void deletePoint( ) {
		_point.SetActive( false );
	}

	public void SetClear( ) {
		_clear = true;
	}
	public  bool isDead( ) {
		return _player_state == PLAYER_STATE.PLAYER_STATE_DEAD;
	}
	public bool isDeadMotionEnd( ) {
		return _game_end_time >= ( GAME_WAIT_TIME * 60 ) && _player_state == PLAYER_STATE.PLAYER_STATE_DEAD;
	}
	public bool isClearMotionEnd( ) {
		return _game_end_time >= ( GAME_WAIT_TIME * 60 ) && _player_state == PLAYER_STATE.PLAYER_STATE_CLEAR;
	}
}
