using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	
	public float WalkSpeed = 0.1f;

	private const float GAUGE_MAX = 100000.0f;
	private float _gauge;
	private float _gauge_speed;
	[SerializeField]private float GaugeChargeSpeed = 100.0f;
	[SerializeField]private float GaugeDischargeSpeed = 500.0f;
	[SerializeField]private float StandGaugeDrop = 50.0f;
	[SerializeField]private float WalkGaugeDrop = 200.0f;
	[SerializeField]private float FullStairGaugeDrop = 300.0f;
	[SerializeField]private float HalfStairGaugeDrop = 300.0f;
	[SerializeField]private float SmallStairGaugeDrop = 100.0f;
	//private Touch _touch;
	private Operation _operation;
	[SerializeField]private float MoveMaxTime = 1;
	private float _move_time = 0;
	private AnimatorController _animator;
	private Vector3 _target_pos = new Vector3( );

	void Awake( ) {
		_animator = GetComponent< AnimatorController >( );
	}

	void Start( ) {
		_operation = GameObject.Find( "Operation" ).GetComponent< Operation >( );
		_gauge = GAUGE_MAX;
		_gauge_speed = StandGaugeDrop;
	}
	
	void Update( ) {
		if ( _gauge > GAUGE_MAX ) {
			_gauge = GAUGE_MAX;
			//_animator.playCharging (false);
		}
		if ( _gauge > 0.0f ) {
			_gauge -= Time.deltaTime * _gauge_speed;
		}
			
		Vector3 rayhit_pos = _operation.getHitRaycastPos( );
		if ( rayhit_pos != new Vector3( ) ) {
			_target_pos = new Vector3 ( rayhit_pos.x, transform.position.y, rayhit_pos.z );
		}
		if ( _target_pos != new Vector3( ) ){
			_gauge_speed = WalkGaugeDrop;
			move ( _target_pos );
			_animator.setRunning( true );
			_move_time++;
		} else {
			_gauge_speed = StandGaugeDrop;
			_animator.setRunning( false );
		}
		if ( ( transform.position == _target_pos ) || _move_time / 60 >= MoveMaxTime ) {
			_move_time = 0;
			_operation.resetTargetPos ( );
			_target_pos = new Vector3( );
			_animator.setRunning( false );
		}
			
	}

	void OnTriggerStay( Collider col ) {
		if (col.gameObject.tag == "Charger" ) {
			_gauge += GaugeChargeSpeed;
			_animator.playCharging ( true );
		}
	}

	void OnCollisionStay( Collision col ) {
		if ( col.gameObject.tag == "Jack" && ( _operation.getHitRaycastTag( ) == "Jack" ) ) {
			//_animator.playDisCharge (true);
			_gauge -= GaugeDischargeSpeed;
			col.collider.GetComponent< JackManager >( ).giveGauge( GaugeDischargeSpeed );
			col.collider.GetComponent< JackManager >( ).playJack( );
			//_animator.playDisCharge (false);
		}
		if ( col.gameObject.tag == "Fan" && ( _operation.getHitRaycastTag( ) == "Fan" ) ) {
			if (!col.collider.GetComponent< FanManager >( ).getFlag( ) ) {
				_gauge -= GaugeDischargeSpeed;
				col.collider.GetComponent< FanManager >( ).isPlay( );
			}
		}
	}

	void OnCollisionEnter( Collision col ) {
		Vector3 pos = transform.position;
		Vector3 col_pos = col.gameObject.GetComponent< Transform >( ).position;
		if (col.gameObject.tag == "StairFull") {
			if (col_pos.y >= pos.y) {
				_gauge -= FullStairGaugeDrop;
			}
			if ( getStair( col_pos ) - ( getStair( pos ) - 0.5F ) <= 1.0f ) {
				pos.y = col_pos.y + col.transform.localScale.y - 0.5f;
			}
			transform.position = pos;
		}

		if ( col.gameObject.tag == "StairHalf" ) {
			if ( col_pos.y + 0.5f >= pos.y ) {
				_gauge -= HalfStairGaugeDrop;
			}
			if ( getStair( col_pos ) - ( getStair( pos ) ) <= 1.0f ) {
				pos.y = col_pos.y ;
			}
			transform.position = pos;
		}

		if ( col.gameObject.tag == "StairSmall" ) {
			if ( col_pos.y + 0.4f >= pos.y ) {
				_gauge -= SmallStairGaugeDrop;
			}
			if ( getStair( col_pos ) - ( getStair( pos ) ) <= 1.0f ) {
				pos.y = col_pos.y;
			}
			transform.position = pos;
		}
	}
		
	int getStair( Vector3 pos ) {
		int i = 0;
		while ( pos.y > 0.0f ) {
			pos.y -= 1.0f;
			i++;
		}
		return i;
	}

	public float getGauge( ) {
		return _gauge;
	}

	public void move( Vector3 pos ) {
		transform.position = Vector3.MoveTowards ( transform.position, pos, WalkSpeed );
		transform.LookAt ( pos );
	}
}
