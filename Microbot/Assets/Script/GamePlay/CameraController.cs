﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private float[] _before_angle = new float[ 2 ];

	private GameObject _player;
	private Vector3 _camera_vec;
	private float _angle = 0;

	void Awake( ) {
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
	}
	
	// Update is called once per frame
	void Update( ) {
		tracking( );
		turn( );
	}

	void turn( ) {
		if ( Input.touchCount >= 2 ) {
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
		}
	}

	void tracking( ) {
		transform.position = new Vector3 ( _player.transform.position.x, _player.transform.position.y, _player.transform.position.z ) + _camera_vec;
		transform.LookAt(_player.transform.position + ( _player.transform.up * ( _player.transform.localScale.y / 2 + _angle ) ) );
	}
}
