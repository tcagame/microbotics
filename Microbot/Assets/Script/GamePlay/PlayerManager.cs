using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

	const float POS_DIFF = 0.9f;
	public float WalkMaxSpeed = 1.0f;

	private const float GAUGE_MAX = 1000000.0f;
	private const int CHARING_TIME = 200;
	private const int DIS_CHARGE_TIME = 200;
	private float _gauge;
	private float _gauge_speed;
	[SerializeField]private float GaugeChargeSpeed = 100.0f;
	[SerializeField]private float StandGaugeDrop = 0.0f;
	[SerializeField]private float WalkGaugeDrop = 0.0f;
	[SerializeField]private float _move_max_time = 2;
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
		_climbing_normal_flag = false;
		_climbing_high_flag = false;
	}

	// Use this for initialization
	void Start () {
		_hit_object_tag = "";
		_operation = GameObject.Find( "GameManager" ).GetComponent< Operation >( );
		_gauge = GAUGE_MAX;
		_gauge_speed = StandGaugeDrop;
		_dust.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		Debug.Log (gameObject.GetComponent<Rigidbody> ().useGravity);
		if (_animator.GetBool ("_is_running")) {
			_dust.SetActive (true);
		} else {
			_dust.SetActive (false);
		}
		//kaidan nobori
		if ( _hit_object_tag == "StairNormal" && !_climbing_normal_flag ) {
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
			Vector3 pos = transform.position;
			if (pos.y < 6.2f) {
				_animator.Play( "climb" );
				pos.y += 0.021f;
				pos.x -= 0.01f;
			} else {
				gameObject.GetComponent<Rigidbody> ().useGravity = true;
			}
			transform.position = pos;
		};
		if ( _hit_object_tag == "StairHigh" && !_climbing_high_flag) {
			_animator.SetBool ("_is_running", false);
			_animator.SetBool( "_is_climbing_high", _climbing_high_flag );
			gameObject.GetComponent<Rigidbody> ().useGravity = false;
			Vector3 pos = transform.position;
			if (pos.y < 9.7f) {
				_animator.Play( "descend_high" );
				pos.y += 0.021f;
				pos.x -= 0.01f;
			} else {
				gameObject.GetComponent<Rigidbody> ().useGravity = true;
			}
			transform.position = pos;
		};
		//

		_animation_time -= 1;
		if ( _animation_time > 0 ) {
			return;
		}
		_animation_time = 0;
		_animator.SetBool( "_is_discharging", false );

		if (_gauge > GAUGE_MAX) {
			_gauge = GAUGE_MAX;
			_animator.SetBool( "_is_charging", false );
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

		if ( _walk_speed > WalkMaxSpeed ) {
			_walk_speed = WalkMaxSpeed;
		}
		if ( _target_pos != new Vector3( ) ) {
			_gauge_speed = WalkGaugeDrop;
			move ( _target_pos, _walk_speed );
			_animator.SetBool( "_is_running", true );
			_move_time++;
			setPoint( _target_pos );
		} else {
			_gauge_speed = StandGaugeDrop;
			_animator.SetBool( "_is_running", false );
			deletePoint( );
		}
		Vector3 diff_pos = transform.position - _target_pos;
		diff_pos.y = 0;
		if ( diff_pos.magnitude < POS_DIFF  ) {
			_move_time = 0;
			_operation.resetTargetPos ( );
			_target_pos = new Vector3( );
			_animator.SetBool( "_is_running", false );
		}
	}

	void OnTriggerStay( Collider col ) {
		if (col.gameObject.tag == "Charger" ) {
			_gauge += GaugeChargeSpeed;
			_animator.SetBool( "_is_charging", true );
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

	void OnCollisionExit( Collision col ) {
		if (_hit_object_tag == "StairNormal") {
			col.gameObject.tag = "";
			_climbing_normal_flag = false;
			gameObject.GetComponent<Rigidbody> ().useGravity = true;
		}
		if (_hit_object_tag == "StairHigh") {
			col.gameObject.tag = "";
			_climbing_high_flag = false;
			gameObject.GetComponent<Rigidbody> ().useGravity = true;
		}
	}

	public string getTouchObjectTag( ){
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
		_point.SetActive( true );
		_point.transform.position = pos + ( _point.transform.up * 0.01f );
	}

	private void deletePoint( ) {
		_point.SetActive( false );
	}
}