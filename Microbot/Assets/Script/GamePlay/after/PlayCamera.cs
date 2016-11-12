using UnityEngine;
using System.Collections;

public class PlayCamera {

	private GameObject _player;
	private Vector3 _pos;
	private Vector3 _vec;
	private GameObject _mine;

	public PlayCamera( ) {
		_mine = GameObject.FindGameObjectWithTag( "MainCamera" );//カメラの取得
		_player = GameObject.FindGameObjectWithTag( "Player" );
		_pos = _player.transform.position;
		_vec = _mine.transform.position - _player.transform.position;
	}

	public void update( ) {
		_mine.transform.position = _pos;
		_mine.transform.position = _player.transform.position + _vec;
		_mine.transform.LookAt (_player.transform.position);
		_pos = _player.transform.position;
	}

	public void ratatePlayerCamera( float angle ) {
		_mine.transform.RotateAround( _player.transform.position, _player.transform.up, angle );
		_pos = _mine.transform.position;
	}
}