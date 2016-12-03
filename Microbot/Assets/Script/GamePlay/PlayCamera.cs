using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayCamera {

	private GameObject _player;
	private Vector3 _pos;
	private Vector3 _vec;
	private GameObject _mine;
	private Slider _camera_slider;

	public PlayCamera( ) {
		_mine = GameObject.FindGameObjectWithTag( "MainCamera" );//カメラの取得
		_player = GameObject.FindGameObjectWithTag( "Player" );
		GameObject sliderObject = GameObject.Find( "CameraSlider" );
		if (sliderObject == null) {
			Debug.Log ("NotFindSlider!!!!");
		} else {
			_camera_slider = sliderObject.GetComponent< Slider > ();
		}
		_pos = _player.transform.position;
		_vec = _mine.transform.position - _player.transform.position;
	}

	public void update( ) {
		{//カメラ移動
			_mine.transform.position = _pos;
			_mine.transform.position = _player.transform.position + _vec;
			_mine.transform.LookAt (_player.transform.position);
		}

		{//カメラ回転
			float value = _camera_slider.value;
			value -= 0.5f;
			float rotate_angle = 180 * value / 0.5f;
			_mine.transform.RotateAround( _player.transform.position, _player.transform.up, rotate_angle);
		}
		_pos = _player.transform.position;
	}
}