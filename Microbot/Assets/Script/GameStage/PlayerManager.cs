using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	
	public float WalkSpeed = 0.1f;

	private const float GAUGE_MAX = 1000.0f;
	private float _gauge;
	private float _gauge_speed;
	[SerializeField]private float GaugeChargeSpeed = 100.0f;
	[SerializeField]private float GaugeDischargeSpeed = 500.0f;
	[SerializeField]private float StandGaugeDrop = 50.0f;
	[SerializeField]private float WalkGaugeDrop = 200.0f;
	[SerializeField]private float FullStairGaugeDrop = 300.0f;
	[SerializeField]private float HalfStairGaugeDrop = 300.0f;
	[SerializeField]private float SmallStairGaugeDrop = 100.0f;
	private Touch _touch;

	[SerializeField]private float _move_max_time = 1;
	private float _move_time = 0;
	private AnimatorController _animator;

	void Awake(){
		_animator = GetComponent<AnimatorController> ();
	}
	// Use this for initialization
	void Start () {
		_touch = GameObject.Find ( "Touch" ).GetComponent< Touch >();

		_gauge = GAUGE_MAX;
		_gauge_speed = StandGaugeDrop;
	}
	
	// Update is called once per frame
	void Update () {
		if (_gauge > GAUGE_MAX) {
			_gauge = GAUGE_MAX;
			//_animator.playCharging (false);
		}
		if (_gauge > 0.0f) {
			_gauge -= Time.deltaTime * _gauge_speed;
		}
		Vector3 touch_traget_pos = _touch.getTargetPos ();

		Vector3 traget_touch = new Vector3 ( touch_traget_pos.x, transform.position.y, touch_traget_pos.z );

		if ( touch_traget_pos != new Vector3( ) ){
			_gauge_speed = WalkGaugeDrop;
			move ( traget_touch );
			//move ( traget_click );
			_animator.setRunning (true);
			_move_time++;
		} else {
			_gauge_speed = StandGaugeDrop;
			_animator.setRunning (false);
		}
		if ( ( transform.position == traget_touch ) || _move_time / 60 >= _move_max_time ) {
			_move_time = 0;
			_touch.resetTargetPos ( );
			//_click.resetTargetPos ( );
			_animator.setRunning (false);
		}
			
	}

	void OnTriggerStay( Collider col ) {
		if (col.gameObject.tag == "Charger" ) {
			_gauge += GaugeChargeSpeed;
			_animator.playCharging ( true );
		}
		if (col.gameObject.tag == "DisCharger" && ( _touch.getTouchTag( ) == "DisCharger" )) {
			_animator.playDisCharge (true);
			_gauge -= GaugeDischargeSpeed;
		}
	}

	void OnCollisionEnter( Collision col ) {
		Vector3 pos = transform.position;
		Vector3 col_pos = col.gameObject.GetComponent<Transform> ().position;
		if (col.gameObject.tag == "StairFull") {
			if (col_pos.y >= pos.y) {
				_gauge -= FullStairGaugeDrop;
			}
			if ( getStair (col_pos) - ( getStair (pos) - 0.5F ) <= 1.0f) {
				pos.y = col_pos.y + col.transform.localScale.y - 0.5f;
			}
			transform.position = pos;
		}

		if (col.gameObject.tag == "StairHalf") {
			if (col_pos.y + 0.5f >= pos.y) {
				_gauge -= HalfStairGaugeDrop;
			}
			if ( getStair (col_pos) - ( getStair (pos) ) <= 1.0f) {
				pos.y = col_pos.y ;
			}
			transform.position = pos;
		}

		if (col.gameObject.tag == "StairSmall") {
			if (col_pos.y + 0.4f >= pos.y) {
				_gauge -= SmallStairGaugeDrop;
			}
			if ( getStair (col_pos) - ( getStair (pos) ) <= 1.0f) {
				pos.y = col_pos.y;
			}
			transform.position = pos;
		}
	}
		
	int getStair( Vector3 pos ) {
		int i = 0;
		while (pos.y > 0.0f) {
			pos.y -= 1.0f;
			i++;
		}
		return i;
	}

	public float getGauge( ) {
		return _gauge;
	}
	public void move ( Vector3 pos ) {
		transform.position = Vector3.MoveTowards ( transform.position, pos, WalkSpeed );
		transform.LookAt ( pos );
	}
}
