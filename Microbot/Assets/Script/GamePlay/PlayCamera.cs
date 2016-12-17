using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayCamera {

	private GameObject _player;
	private Vector3 _pos;
	private Vector3 _vec;
	private GameObject _mine;
	private Slider _camera_slider;

	private float CAMERA_MAX_RANGE = 8.0f;
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
	}

	public void update( ) {
		_mine.transform.position = _pos;
		{//カメラ移動
			if ( _vec.magnitude > CAMERA_MAX_RANGE) {
				_mine.transform.position = _player.transform.position + _vec.normalized * CAMERA_MAX_RANGE;
			}
			if ( _vec.magnitude < CAMERA_MIN_RANGE) {
				_mine.transform.position = _player.transform.position + _vec.normalized * CAMERA_MIN_RANGE;
			}
			float camera_height = _player.transform.position.y + CAMERA_HEIGHT;
			_mine.transform.position = new Vector3 (_mine.transform.position.x, camera_height, _mine.transform.position.z);
			//カメラ位置の調整
			//レイキャストで前に壁があるか判定
			//ある場合は一番近くまで移動して
			//長さがそのままになるように高さを変更する。
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