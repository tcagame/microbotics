using UnityEngine;
using System.Collections;

public class Touch : MonoBehaviour {
	private Vector3 _screen_to_world_pos;
	private Vector3 _target_pos;
	private Vector3 _touch_position;
	private float _time;
	private float _wait_time;
	public bool _is_touch_click;
	public bool _is_multi_touch;
	public bool _move_touch;
	void Awake(){
		
	}

	void Start () {
		_target_pos = new Vector3 ();
		_is_touch_click = false;
		_is_multi_touch = false;
		_move_touch = false;
		_time = 0;
		_wait_time = 20;
	}
	
	void Update () {

		if ( Input.touchCount == 0 ) {
			_is_multi_touch = false;
		}
		if ( Input.touchCount > 1 ) {
			_is_multi_touch = true;
			_time = 0;
			_move_touch = false;
		}
		if ( !_is_multi_touch ) {
			_time++;
		}
		if ( _time >= _wait_time ) {
			_move_touch = true;
		}

		if ( Input.touchCount == 1 && _move_touch ) {
			if ( Input.GetTouch( 0 ).phase == TouchPhase.Ended ) {
				_touch_position = Input.GetTouch( 0 ).position;
				_is_touch_click = true;
			}
		}
		if ( !_is_touch_click ) {
			return;
		}

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (_touch_position);
		if (Physics.Raycast (ray, out hit)) {
			tag = hit.collider.gameObject.tag;
			if ( tag != null && tag != "Player" ) {
				_target_pos = hit.point;	
				_target_pos = new Vector3 ( _target_pos.x, 0, _target_pos.z );
				_is_touch_click = false;
				_touch_position = new Vector3 ();
			}
		}
	}
	public Vector3 getTargetPos( ) {
		return _target_pos;
	}
	public void resetTargetPos( ) {
		_target_pos = new Vector3( );
	}
	public string getTouchTag ( ) {
		return tag;
	}
}
