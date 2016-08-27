using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	
	public float WalkSpeed = 0.1f;

	private const float GAUGE_MAX = 100000.0f;
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
			_animator.playDisCharge (true);
			_gauge -= GaugeDischargeSpeed;
			col.collider.GetComponent<JackManager> ().giveGauge (GaugeDischargeSpeed);
			col.collider.GetComponent<JackManager> ().playJack();
			_animator.playDisCharge (false);
		}
		if (col.gameObject.tag == "Fan" && ( _operation.getHitRaycastTag( ) == "Fan" )) {
			if (!col.collider.GetComponent<FanManager> ().getFlag ()) {
				_gauge -= GaugeDischargeSpeed;
				col.collider.GetComponent<FanManager> ().isPlay ();
			}
		}
	}

	void OnCollisionEnter( Collision col ) {
		Vector3 pos = transform.position;
		Vector3 col_pos = col.gameObject.GetComponent<Transform> ().position;
		Vector3 col_scale = col.gameObject.GetComponent<Transform> ().localScale;
		if (col.gameObject.tag == "Stair") {
			pos.y = col_pos.y + col_scale.y / 2;
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
