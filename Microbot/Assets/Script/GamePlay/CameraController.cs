using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private Operation _operation;
	private GameObject _player;
	private Vector3 _start_pos1;
	private Vector3 _start_pos2;

	private Vector3 _transfrom;
	private Vector3 _camera_vec;
	private Vector3 _camera_lock;
	private float _angle = 0;

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
		_camera_lock = new Vector3 ( );
	}
	
	// Update is called once per frame
	void Update( ) {
		_transfrom = _player.transform.position + _camera_vec;
		tracking( );
		turn( );

	}

	void turn( ) {
		_angle = 0;
		Vector3 move_pos1 = new Vector3 ();
		Vector3 move_pos2 = new Vector3 ();
		if ( _operation.getInputCount( ) == 1 ) {
			Vector3 pos = _operation.getInputPosition( 0 );
			if ( pos.x > 50 && pos.x < 400 ) {
				return;
			}
			if ( _operation.getTouchPhase( ) == TouchPhase.Began ) {
				_start_pos1 = pos;
			}
			if ( _operation.getTouchPhase( ) == TouchPhase.Moved ) {
				move_pos1 = pos - _start_pos1;
			}
			if ( move_pos1.y == 0 ) {
				return;
			}
			_angle = move_pos1.y / 5;
			transform.RotateAround (_player.transform.position , Vector3.up, _angle);
		}
		if ( _operation.getInputCount( ) == 2 ) {
			Vector3 pos1 = _operation.getInputPosition( 0 );
			Vector3 pos2 = _operation.getInputPosition( 1 );
			if ( pos1.x > 50 && pos1.x < 400 ) {
				return;
			}
			if ( _operation.getTouchPhase( 0 ) == TouchPhase.Began ) {
				_start_pos1 = pos1;
			}
			if ( _operation.getTouchPhase( 0 ) == TouchPhase.Moved ) {
				move_pos1 = pos1 - _start_pos1;
			}
			_camera_lock += move_pos1;
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
		transform.position = _transfrom;

		transform.LookAt(_player.transform.position + ( _player.transform.up * ( _player.transform.localScale.y / 2 + _angle  ) ) +_camera_lock );
	}
}
