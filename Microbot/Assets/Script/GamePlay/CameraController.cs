using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private Operation _operation;
	private GameObject _player;
	private Vector3 _start_pos1;

	private Vector3 _transfrom;
	private Vector3 _camera_vec;
	private Vector3 _camera_lock;
	private Vector3 _before_pos;
	private float _angle = 0;
	private float _angle_2 = 0;

	void Awake( ) {
		_operation = GameObject.Find( "Operation" ).GetComponent< Operation >( );
		if ( _player == null ) {
			GameObject nullplayer = GameObject.Find ("Player");
			if ( nullplayer == null ) {
				nullplayer = ( GameObject )Resources.Load ( "Player" );
			}
			_player = nullplayer;
		}
	}

	// Use this for initialization
	void Start( ){
		_camera_vec = transform.position - _player.transform.position;
		_transfrom = _player.transform.position + _camera_vec;
		_before_pos = _player.transform.position;
		_camera_lock = new Vector3 ( );
	}
	
	// Update is called once per frame
	void Update( ) {
		_transfrom = transform.position - _camera_vec;
		tracking( );
		turn( );

	}

	void turn( ) {

		Vector3 move_pos1 = new Vector3 ();
		if (_operation.getInputCount () >= 1) {
			Vector3 pos = _operation.getInputPosition (0);
			if (_operation.getTouchPhase () == TouchPhase.Began) {
				_start_pos1 = pos;
			}
			if (_operation.getTouchPhase () == TouchPhase.Moved) {
				move_pos1 = pos - _start_pos1;
			}
			if (pos.x > 1900 && pos.y > 100) {
				if (_angle > 10 || _angle < 0) {
					return;
				}
				_angle += move_pos1.y / 200;
			}
			if (pos.y < 100) {
				_angle_2 = move_pos1.x / 20;
				transform.RotateAround (_player.transform.position, Vector3.up, _angle_2);
			}
		} else {
			_angle = 0;
		}


		/*if ( Input.touchCount >= 2 ) {
			Vector2 center_pos = ( Input.GetTouch( 0 ).position + Input.GetTouch( 1 ).position ) / 2;
			Vector2 vec1 = Input.GetTouch( 0 ).position - center_pos;
			Vector2 vec2 = Input.GetTouch( 1 ).position - center_pos;
			float angle_1 = Vector2.Angle( vec1.normalized, Vector2.up );
			float angle_2 = Vector2.Angle( vec2.normalized, Vector2.down );

			bool first_touch = Input.GetTouch( 0 ).phase == TouchPhase.Began || Input.GetTouch( 1 ).phase == TouchPhase.Began;
			if ( first_touch ) {
				_before_angle[ 0 ] = angle_1;
				_before_angle[ 1 ] = angle_2;
			}

			bool multi_swipe = Input.GetTouch( 0 ).phase == TouchPhase.Moved && Input.GetTouch( 1 ).phase == TouchPhase.Moved;
			if ( multi_swipe ) {
				float angle1 = Mathf.DeltaAngle( _before_angle [ 0 ], angle_1 );
				float angle2 = Mathf.DeltaAngle( _before_angle [ 1 ], angle_2 );
				float rotate_angle;
				if ( Mathf.Abs( angle1 ) > Mathf.Abs( angle2 ) ) {
					rotate_angle = angle1;
				} else {
					rotate_angle = angle2;
				}
				transform.RotateAround( _player.transform.position, Vector3.up, rotate_angle );
				_camera_vec = transform.position - _player.transform.position;
				_before_angle[ 0 ] = angle_1;
				_before_angle[ 1 ] = angle_2;
			}
		}*/
	}

	void tracking( ) {
		transform.position += _player.transform.position - _before_pos;
		transform.LookAt(_player.transform.position + ( _player.transform.up * ( _player.transform.localScale.y / 2 + _angle  ) ) +_camera_lock );
		_before_pos = _player.transform.position;

	}
}
