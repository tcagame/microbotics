using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	
	public float WalkSpeed = 0.1f;

	private const float GAUGE_MAX = 1000.0f;
	private const int CHARING_TIME = 300;
	private const int DIS_CHARGE_TIME = 400;
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
	  //ポイント
    GameObject point;
	
    void Awake(){
		_animator = GetComponent<AnimatorController>( );
        point = ( GameObject )Resources.Load( "Prefab/Point" );
        point = Instantiate( point );
        point.SetActive( false );
	}

	// Use this for initialization
	void Start () {
		_operation = GameObject.Find( "Operation" ).GetComponent< Operation >( );
		_gauge = GAUGE_MAX;
		_gauge_speed = StandGaugeDrop;
	}
	
	// Update is called once per frame
	void Update () {
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

	void OnCollisionStay( Collision col ) {
		if (col.gameObject.tag == "Jack" && ( _operation.getHitRaycastTag( ) == "Jack" )) {
			if ( !col.collider.GetComponent<JackManager>( ).getPlay( ) ) {
				_animator.setRunning (false);
				_animator.playDisCharge (true);
				_gauge -= GaugeDischargeSpeed;
				_animation_time += DIS_CHARGE_TIME;
				col.collider.GetComponent< JackManager > ().play ();
			}
		}
		if (col.gameObject.tag == "FanSwitch" && ( _operation.getHitRaycastTag( ) == "FanSwitch" )) {
			if (!col.collider.GetComponent<FanSwitch> ().getFlag ( )) {
				_gauge -= GaugeDischargeSpeed;
				_animation_time += DIS_CHARGE_TIME;
				_animator.setRunning (false);
				_animator.playDisCharge(true);
				col.collider.GetComponent<FanSwitch> ().isPlay ();
			}
		}
        if (col.gameObject.tag == "Propeller" && ( _operation.getHitRaycastTag( ) == "Propeller" )) {
			if (!col.collider.GetComponent<Propellers> ().getFlag ()) {
				_gauge -= GaugeDischargeSpeed;
				_animation_time += DIS_CHARGE_TIME;
				_animator.setRunning (false);
				_animator.playDisCharge (true);
				col.collider.GetComponent<Propellers> ().isPlay ();
			} else {
				col.collider.GetComponent<Propellers> ().isOff ();
			}
		}

	}

	void OnCollisionEnter( Collision col ) {
		Vector3 pos = transform.position;
		Vector3 col_pos = col.gameObject.GetComponent<Transform> ().position;
		Vector3 col_scale = col.gameObject.GetComponent<Transform> ().localScale;
		if (col.gameObject.tag == "Stair") {
			pos.y = col_pos.y + col_scale.y / 2 + transform.localScale.y / 2;
			transform.position = pos;
			if (col_scale.y <= transform.localScale.y / 5) {
				_gauge -= SmallStairGaugeDrop;
			} else if (col_scale.y <= transform.localScale.y / 2) {
				_gauge -= HalfStairGaugeDrop;
			} else {
				_gauge -= FullStairGaugeDrop;
			}
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
