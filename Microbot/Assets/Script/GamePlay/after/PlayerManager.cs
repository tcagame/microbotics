using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	public float WalkSpeed = 0.1f;

	private const float GAUGE_MAX = 1000000.0f;
	private const int CHARING_TIME = 200;
	private const int DIS_CHARGE_TIME = 200;
	private float _gauge;
	private float _gauge_speed;
	[SerializeField]private float GaugeChargeSpeed = 100.0f;
	[SerializeField]private float GaugeDischargeSpeed = 500.0f;
	[SerializeField]private float StandGaugeDrop = 0.0f;
	[SerializeField]private float WalkGaugeDrop = 0.0f;
	[SerializeField]private float _move_max_time = 2;
	private float FullStairGaugeDrop = 300.0f;
	private float HalfStairGaugeDrop = 200.0f;
	private float SmallStairGaugeDrop = 100.0f;
	//private Touch _touch;
	private Operation _operation;
	private float _move_time = 0;
	private float _walk_speed = 0.0f;
	private Vector3 _target_pos = new Vector3 ( );
	private int _check_first_touch = 0;
	private bool _climbing_normal_flag = false;
	private bool _climbing_high_flag = false;
	[SerializeField]private string _hit_object_tag;
	//ポイント
	GameObject point;
	GameObject _nevy;
	//イベントカメラ
	EventCamera event_camera;

	RaycastHit hit;

	void Awake(){
		_animator = GetComponent<AnimatorController>( );
		_nevy = GameObject.Find("Nevy").gameObject;
		point = ( GameObject )Resources.Load( "Prefab/Point" );
		point = Instantiate( point );
		point.SetActive( false );
		_climbing_normal_flag = false;
		_climbing_high_flag = false;
	}

	// Use this for initialization
	void Start () {
		_hit_object_tag = "";
		_operation = GameObject.Find( "Operation" ).GetComponent< Operation >( );
		_gauge = GAUGE_MAX;
		_gauge_speed = StandGaugeDrop;
		event_camera = GameObject.Find ("GameManager").GetComponent< EventCamera >( );
	}

	// Update is called once per frame
	void Update () {
		//kaidan nobori
		if ( _climbing_high_flag) {
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
			Vector3 pos = transform.position;
			pos.y += 0.021f;
			pos.x -= 0.01f;
			transform.position = pos;
			if (pos.y > 9.75f) {
				_climbing_high_flag = false;
				_animator.playClimbHigh( false );
				gameObject.GetComponent<Rigidbody> ().useGravity = true;
			}
		};
		if ( _climbing_normal_flag) {
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
			Vector3 pos = transform.position;
			pos.y += 0.01f;
			pos.z -= 0.01f;
			transform.position = pos;
			if (pos.y > 6.2f) {
				_climbing_normal_flag = false;
				_animator.playClimbNormal( false );
				gameObject.GetComponent<Rigidbody> ().useGravity = true;
			}
		};
		//

		_animation_time -= 1;
		if ( _animation_time > 0 ) {
			return;
		}
		_animation_time = 0;
		_animator.playDisCharge (false);

		if (_gauge > GAUGE_MAX) {
			_gauge = GAUGE_MAX;
			_animator.playCharging (false);
		}
		if (_gauge > 0.0f) {
			_gauge -= Time.deltaTime * _gauge_speed;
		}

		Vector3 rayhit_pos = _operation.getHitRaycastPos( );
		if ( rayhit_pos != new Vector3( ) ) {
			_target_pos = new Vector3 ( rayhit_pos.x, transform.position.y, rayhit_pos.z );
			if ( _check_first_touch == 0 ) {
				_walk_speed = Vector3.Distance( _target_pos, transform.position ) / ( _move_max_time * 60 ) ;	
			}
			_check_first_touch++;
		} else {
			_check_first_touch = 0;	
		}

		if ( _target_pos != new Vector3( ) ) {
			_gauge_speed = WalkGaugeDrop;
			move ( _target_pos, _walk_speed );
			_animator.setRunning (true);
			_move_time++;
			setPoint( _target_pos );
		} else {
			_gauge_speed = StandGaugeDrop;
			_animator.setRunning (false);
			deletePoint( );
		}

		if ( transform.position == _target_pos || _move_time / 60 == _move_max_time ) {
			_move_time = 0;
			_operation.resetTargetPos ( );
			_target_pos = new Vector3( );
			_animator.setRunning (false);
		}
	}

	void OnTriggerStay( Collider col ) {
		if (col.gameObject.tag == "Charger" ) {
			_gauge += GaugeChargeSpeed;
			_animator.playCharging ( true );
		}
	}

	void OnTriggerEnter( Collider col ) {
		if (col.gameObject.tag == "Goal") {
			SceneManager.LoadScene( "GameClear" );
		}
	}

	void OnCollisionEnter( Collision col ) {
		_hit_object_tag = col.gameObject.tag;
	}

	public void getTouchObjectTag( ){
		return _hit_object_tag;
	}

	public float getGauge( ) {
		return _gauge;
	}

	private void move ( Vector3 pos, float walk_speed ) {
		transform.position = Vector3.MoveTowards ( transform.position, pos, walk_speed );
		transform.LookAt ( pos );
	}

	private void setPoint( Vector3 pos ) {
		point.SetActive( true );
		point.transform.position = pos + ( point.transform.up * 0.01f );
	}

	private void deletePoint( ) {
		point.SetActive( false );
	}
}


/*旧コード
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {
	
	public float WalkSpeed = 0.1f;

	private const float GAUGE_MAX = 1000000.0f;
	private const int CHARING_TIME = 200;
	private const int DIS_CHARGE_TIME = 200;
	private float _gauge;
	private float _gauge_speed;
	[SerializeField]private float GaugeChargeSpeed = 100.0f;
	[SerializeField]private float GaugeDischargeSpeed = 500.0f;
	[SerializeField]private float StandGaugeDrop = 0.0f;
	[SerializeField]private float WalkGaugeDrop = 0.0f;
	private float FullStairGaugeDrop = 300.0f;
	private float HalfStairGaugeDrop = 200.0f;
	private float SmallStairGaugeDrop = 100.0f;
	//private Touch _touch;
	private Operation _operation;
	[SerializeField]private float _move_max_time = 2;
	private float _move_time = 0;
	private float _walk_speed = 0.0f;
	private AnimatorController _animator;
	private int _animation_time = 0;
	private Vector3 _target_pos = new Vector3 ( );
	private int _check_first_touch = 0;
	private bool _climbing_normal_flag = false;
	private bool _climbing_high_flag = false;
	  //ポイント
    GameObject point;
    GameObject _nevy;
	//イベントカメラ
	EventCamera event_camera;

	RaycastHit hit;

    void Awake(){
		_animator = GetComponent<AnimatorController>( );
        _nevy = GameObject.Find("Nevy").gameObject;
        point = ( GameObject )Resources.Load( "Prefab/Point" );
        point = Instantiate( point );
        point.SetActive( false );
		_climbing_normal_flag = false;
		_climbing_high_flag = false;
	}

	// Use this for initialization
	void Start () {
		_operation = GameObject.Find( "Operation" ).GetComponent< Operation >( );
		_gauge = GAUGE_MAX;
		_gauge_speed = StandGaugeDrop;
		event_camera = GameObject.Find ("GameManager").GetComponent< EventCamera >( );
	}
	
	// Update is called once per frame
	void Update () {

		//kaidan nobori
		if ( _climbing_high_flag) {
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
			Vector3 pos = transform.position;
			pos.y += 0.021f;
			pos.x -= 0.01f;
			transform.position = pos;
			if (pos.y > 9.75f) {
				_climbing_high_flag = false;
				_animator.playClimbHigh( false );
				gameObject.GetComponent<Rigidbody> ().useGravity = true;
			}
		};
		if ( _climbing_normal_flag) {
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
			Vector3 pos = transform.position;
			pos.y += 0.01f;
			pos.z -= 0.01f;
			transform.position = pos;
			if (pos.y > 6.2f) {
				_climbing_normal_flag = false;
				_animator.playClimbNormal( false );
				gameObject.GetComponent<Rigidbody> ().useGravity = true;
			}
		};
		//

		_animation_time -= 1;
		if ( _animation_time > 0 ) {
			return;
		}
		_animation_time = 0;
		_animator.playDisCharge (false);

		if (_gauge > GAUGE_MAX) {
			_gauge = GAUGE_MAX;
			_animator.playCharging (false);
		}
		if (_gauge > 0.0f) {
			_gauge -= Time.deltaTime * _gauge_speed;
		}
			
		Vector3 rayhit_pos = _operation.getHitRaycastPos( );
		if ( rayhit_pos != new Vector3( ) ) {
			_target_pos = new Vector3 ( rayhit_pos.x, transform.position.y, rayhit_pos.z );
			if ( _check_first_touch == 0 ) {
				_walk_speed = Vector3.Distance( _target_pos, transform.position ) / ( _move_max_time * 60 ) ;	
			}
			_check_first_touch++;
		} else {
			_check_first_touch = 0;	
		}

		if ( _target_pos != new Vector3( ) ) {
			_gauge_speed = WalkGaugeDrop;
			move ( _target_pos, _walk_speed );
			_animator.setRunning (true);
			_move_time++;
            setPoint( _target_pos );
		} else {
			_gauge_speed = StandGaugeDrop;
			_animator.setRunning (false);
             deletePoint( );
		}

		if ( transform.position == _target_pos || _move_time / 60 == _move_max_time ) {
			_move_time = 0;
			_operation.resetTargetPos ( );
			_target_pos = new Vector3( );
			_animator.setRunning (false);
		}
	}

	void OnTriggerStay( Collider col ) {
		if (col.gameObject.tag == "Charger" ) {
			_gauge += GaugeChargeSpeed;
			_animator.playCharging ( true );
		}
	}

	void OnTriggerEnter( Collider col ) {
		if (col.gameObject.tag == "Goal") {
			SceneManager.LoadScene( "GameClear" );
		}
	}

	void OnCollisionStay( Collision col ) {
		if (col.gameObject.tag == "Jack" && ( _operation.getHitRaycastTag( ) == "Jack" )) {
			if ( !col.collider.GetComponent<JackManager>( ).getPlay( ) ) {
				_animator.setRunning (false);
				_animator.playDisCharge (true);
				_gauge -= GaugeDischargeSpeed;
				_animation_time += DIS_CHARGE_TIME;
				col.collider.GetComponent< JackManager > ().play ();
				event_camera.CallEventCamera( col.transform.position + new Vector3( -4, 0.5f, 0 ), new Vector3( 1, 4, 1 ) );
				_nevy.transform.position = new Vector3(-10.85f, 0.45f, 9.56f);
			}
		}
		if (col.gameObject.tag == "FanSwitch" && ( _operation.getHitRaycastTag( ) == "FanSwitch" )) {
			if (!col.collider.GetComponent<FanSwitch> ().getFlag ( )) {
				_gauge -= GaugeDischargeSpeed;
				_animation_time += DIS_CHARGE_TIME;
				_animator.setRunning (false);
				_animator.playDisCharge(true);
				col.collider.GetComponent<FanSwitch> ().isPlay ();
				Vector3 pos = new Vector3 (-12, 7, 3);
				event_camera.CallEventCamera( pos, pos + new Vector3( 0, 0, 5 ) );
				_nevy.transform.position = new Vector3(-14.93f, 0.53f, 1.03f);
			}
		}
        if (col.gameObject.tag == "BigFanSwitch" && (_operation.getHitRaycastTag() == "BigFanSwitch"))
        {
            if (!col.collider.GetComponent<BigFanSwitch>().getFlag())
            {
                _gauge -= GaugeDischargeSpeed;
                _animation_time += DIS_CHARGE_TIME;
                _animator.setRunning(false);
                _animator.playDisCharge(true);
                col.collider.GetComponent<BigFanSwitch>().isPlay();
                Vector3 pos = new Vector3(10, 40,0);
                event_camera.CallEventCamera(pos, pos + new Vector3(-5, -30, -10));
                _nevy.transform.position = new Vector3(31.12f, 1.51f, -8.94f);
            }
        }
        if (col.gameObject.tag == "Propeller" && ( _operation.getHitRaycastTag( ) == "Propeller" )) {
			if (!col.collider.GetComponent<Propellers> ().getFlag ()) {
				_gauge -= GaugeDischargeSpeed;
				_animation_time += DIS_CHARGE_TIME;
				_animator.setRunning (false);
				_animator.playDisCharge (true);
				col.collider.GetComponent<Propellers> ().isPlay ();
				_operation.resetTargetTag( );
				_nevy.transform.position = new Vector3(-17.12f, 6.66f, -18.01f);
			}
		}
	}

	void OnCollisionEnter( Collision col ) {
		if (col.gameObject.tag == "StairHigh") {
			_climbing_high_flag = true;
			_animator.setRunning (false);
			_animator.playClimbNormal( false );
			_animator.playClimbHigh( true );
			_nevy.transform.position = new Vector3(-26.9f, 8.07f, -14.02f);
		}
		if (col.gameObject.tag == "StairNormal") {
			_climbing_normal_flag = true;
			_animator.setRunning (false);
			_animator.playClimbHigh( false );
			_animator.playClimbNormal( true );
			_nevy.transform.position = new Vector3(-20.64f, 6.18f, -18.34f);
		}
	}

	public float getGauge( ) {
		return _gauge;
	}

	private void move ( Vector3 pos, float walk_speed ) {
		transform.position = Vector3.MoveTowards ( transform.position, pos, walk_speed );
		transform.LookAt ( pos );
	}

    private void setPoint( Vector3 pos ) {
        point.SetActive( true );
        point.transform.position = pos + ( point.transform.up * 0.01f );
    }

	private void deletePoint( ) {
        point.SetActive( false );
    }
}
*/