using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayCamera {

	private GameObject _player;
	private Vector3 _pos;
	private Vector3 _vec;
	private GameObject _mine;
	private Slider _camera_slider;
	private float _befor_value;
	private Vector3 _before_player_pos;

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
		_befor_value = _camera_slider.value;
	}

	public void update( ) {
		_mine.transform.position = _pos;

		{//カメラ回転
			float value = _camera_slider.value - _befor_value;
			float rotate_angle = 180 * value / 0.5f;
			_mine.transform.RotateAround( _player.transform.position, _player.transform.up, rotate_angle);
		}

		{//カメラ移動
			float camera_min_height = _player.transform.position.y + CAMERA_HEIGHT;
			Vector3 vec = _player.transform.position - _mine.transform.position;
			if ( _before_player_pos != _player.transform.position ) {
				_befor_value = _camera_slider.value = 0.5f;
			}
			//カメラ位置の調整
			//レイキャストで前に壁があるか判定
			Ray ray = new Ray( );
			ray.origin = _mine.transform.position;
			ray.direction = vec.normalized;
			bool is_front_wall = Physics.Raycast( ray, vec.magnitude );
			if (is_front_wall) {
				while (is_front_wall) {
					//ある場合は一番近くまで移動して
					_mine.transform.position += new Vector3 (_mine.transform.forward.x, 0, _mine.transform.forward.z) * 0.1f;
					ray.origin = _mine.transform.position;
					ray.direction = vec.normalized;
					vec = _player.transform.position - _mine.transform.position;
					if (vec.magnitude < CAMERA_MIN_RANGE) {
						break;
					}
					is_front_wall = Physics.Raycast (ray, vec.magnitude);
				}
			} else {
				if (camera_min_height <= _mine.transform.position.y) {
					_mine.transform.position -= new Vector3( 0, 0.01f, 0);
				}
			}
			vec = _player.transform.position - _mine.transform.position;
			//ミンより近づいたら離れる
			if ( vec.magnitude < CAMERA_MIN_RANGE ) {
				_mine.transform.position = _player.transform.position + _vec.normalized * CAMERA_MIN_RANGE;
			}
			//マックスより離れたら近く
			if ( vec.magnitude > CAMERA_MAX_RANGE) {
				_mine.transform.position = _player.transform.position + _vec.normalized * CAMERA_MAX_RANGE;
			}
			if (camera_min_height >= _mine.transform.position.y) {
				_mine.transform.position = new Vector3 (_mine.transform.position.x, camera_min_height, _mine.transform.position.z);
			}
			_mine.transform.LookAt (_player.transform.position);
		}
		_vec = _mine.transform.position - _player.transform.position;
		_pos = _mine.transform.position;
		_before_player_pos = _player.transform.position;
		_befor_value = _camera_slider.value;
	}
}