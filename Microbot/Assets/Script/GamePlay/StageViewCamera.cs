using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageViewCamera {
	GameObject _mine;

	private int MOVE_COUNT = 10;
	private List<Vector3> _target_list;
	private Vector3 _move_vec;
	private int _target_key;
	private int _count;

	public StageViewCamera( ) {
		_mine = GameObject.FindGameObjectWithTag( "MainCamera" );//カメラの取得
	}

	public void initialize( ) {
		
	}

	public void update( ) {
		_mine.transform.position += _move_vec;
		_count++;
		if ( _count == MOVE_COUNT ) {
			changeTarget( );
		}
	}

	public void callStageViewCamera( Vector3 start_pos, List< Vector3 > list ) {
		//リストの設定
		_target_list = list;
		_target_key = 0;
		if (list.Count > 0) {
			//カメラ位置の設定
			_mine.transform.position = start_pos;
			_mine.transform.LookAt (_target_list [0]);
			//移動設定
			Vector3 vec = _mine.transform.position - _target_list [_target_key];
			_move_vec = vec / MOVE_COUNT;
			_count = 0;
		}
	}

	public bool isfinish( ) {
		bool result = false;
		if (_target_list.Count <= _target_key) {
			result = true;
		}
		return result;
	}

	void changeTarget( ) {
		_target_key++;
		Vector3 vec = _mine.transform.position - _target_list[ _target_key ];
		_move_vec = vec / MOVE_COUNT;
		_count = 0;
	}
}
