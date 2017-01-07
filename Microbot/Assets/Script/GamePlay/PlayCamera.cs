using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayCamera {

	private GameObject _player;
	private Vector3 _pos;
	private Vector3 _vec;
	private GameObject _mine;
	private Slider _camera_slider;

	private float CAMERA_MAX_RANGE = 7.0f;
	private float CAMERA_MIN_RANGE = 5.0f;
	private float CAMERA_HEIGHT = 3;

	public PlayCamera( ) {
		_mine = GameObject.FindGameObjectWithTag( "MainCamera" );//カメラの取得
		_player = GameObject.FindGameObjectWithTag( "Player" );
		GameObject sliderObject = GameObject.Find( "CameraSlider" );
		if (sliderObject == null) {
			Debug.Log ("NotFindSlider!!!!");
		} else {
			_camera_slider = sliderObject.GetComponent< Slider > ();
		}
		_vec = _mine.transform.position - _player.transform.position;
		_pos = _player.transform.position + _vec;
		_pos = new Vector3( _mine.transform.position.x, _player.transform.position.y + CAMERA_HEIGHT, _mine.transform.position.z );
	}

	public void update( ) {
		_mine.transform.position = _pos;
		{//カメラ移動
			float camera_min_height = _player.transform.position.y + CAMERA_HEIGHT;
			Vector3 vec = _player.transform.position - _mine.transform.position;
			//カメラ位置の調整
			//レイキャストで前に壁があるか判定
			Ray ray = new Ray( );
			ray.origin = _mine.transform.position;
			ray.direction = vec.normalized;
			bool is_front_wall = Physics.Raycast( ray, vec.magnitude );
			if( is_front_wall ) {
				while( is_front_wall ) {
					//ある場合は一番近くまで移動して
					_mine.transform.position += new Vector3( _mine.transform.forward.x, 0, _mine.transform.forward.z ) * 0.01f;
					ray.origin = _mine.transform.position;
					ray.direction = vec.normalized;
					vec = _player.transform.position - _mine.transform.position;
					is_front_wall = Physics.Raycast( ray, vec.magnitude );
				}
				//長さがそのままになるように高さを変更する。
				vec = _player.transform.position - _mine.transform.position;
				while( vec.magnitude < CAMERA_MIN_RANGE ) {
					_mine.transform.position += Vector3.up * 0.01f;
					vec = _player.transform.position - _mine.transform.position;
				}
			}

			//長さがそのままになるように高さを変更する。
			vec = _player.transform.position - _mine.transform.position;
			if( _mine.transform.position.y > camera_min_height ) {
				while( vec.magnitude > CAMERA_MIN_RANGE && camera_min_height < _mine.transform.position.y ) {
					_mine.transform.position -= Vector3.up * 0.01f;
					vec = _player.transform.position - _mine.transform.position;
				}
				if( camera_min_height >= _mine.transform.position.y ) {
					_mine.transform.position = new Vector3( _mine.transform.position.x, camera_min_height, _mine.transform.position.z );
				}
			} else {
				//ミンより近づいたら離れる
				if ( vec.magnitude < CAMERA_MIN_RANGE ) {
					_mine.transform.position = _player.transform.position + _vec.normalized * CAMERA_MIN_RANGE;
				}
				//マックスより離れたら近く
				if ( vec.magnitude > CAMERA_MAX_RANGE) {
					_mine.transform.position = _player.transform.position + _vec.normalized * CAMERA_MAX_RANGE;
				}
				if( camera_min_height >= _mine.transform.position.y ) {
					_mine.transform.position = new Vector3( _mine.transform.position.x, camera_min_height, _mine.transform.position.z );
				}
			}
			_mine.transform.LookAt (_player.transform.position);
		}

		{//カメラ回転
			float value = _camera_slider.value;
			value -= 0.5f;
			float rotate_angle = 180 * value / 0.5f;
			_mine.transform.RotateAround( _player.transform.position, _player.transform.up, rotate_angle);
		}
		_vec = _mine.transform.position - _player.transform.position;
		_pos = _mine.transform.position;
	}
}