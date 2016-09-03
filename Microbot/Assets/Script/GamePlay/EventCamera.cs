using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventCamera : MonoBehaviour {
    public double EVENT_TIME;
    private double _event_time;
    public Camera main_camera;
    public Camera event_camera;
	private Operation _operation;

	public List<Vector3> target_list;
	public float MOVE_SPEED;
	private int _target_key;
	private float _move_speed;

	enum EVENT_MODE {
		EVENT_MODE_NONE,
		EVENT_MODE_POINT,
		EVENT_MODE_STAGE,
	};

	EVENT_MODE _event_mode;

    // Use this for initialization
    void Start( ) {
        event_camera.gameObject.SetActive( false );
        _event_time = 0;
		_operation = GameObject.Find ("Operation").GetComponent<Operation>( );
		_target_key = 0;
		_event_mode = EVENT_MODE.EVENT_MODE_NONE;
	}
	// Update is called once per frame
	void Update( ) {
		if (_event_mode == EVENT_MODE.EVENT_MODE_POINT) {
			_event_time += Time.deltaTime;
			if (_event_time > EVENT_TIME) {
				StartMainCamera ();
			}
		}
		if ( _event_mode == EVENT_MODE.EVENT_MODE_STAGE ) {
			Vector3 vec;
			float length = 0.0f;
			vec = target_list[ _target_key ] - event_camera.transform.position;
			length = vec.magnitude;
			vec = vec.normalized * _move_speed;
			event_camera.transform.position += vec;
			if (length < ( target_list[ _target_key ] - target_list[ _target_key - 1 ] ).magnitude  * 1 / 10 ) {
				if (_target_key + 1 < target_list.Count) {
					vec = target_list [ _target_key + 1 ] - event_camera.transform.position;
				}
			}
			if ( length < 1.0f ) {
				_target_key++;
				if (target_list.Count == _target_key) {
					StartMainCamera ();
					return;
				}
				_move_speed = ( event_camera.transform.position - target_list[ _target_key + 1 ] ).magnitude;
				_move_speed = _move_speed / MOVE_SPEED;
			}
			event_camera.transform.LookAt( event_camera.transform.position + vec.normalized * _move_speed );
		}
	}

    public void CallEventCamera( Vector3 pos, Vector3 target ) {
		if ( event_camera.gameObject.activeSelf ) {
            return;
        }
		event_camera.transform.position = pos;
		event_camera.transform.LookAt( target );
		_event_mode = EVENT_MODE.EVENT_MODE_POINT;
		StopMainCamera ();
    }

	public void StageView( ) {
		event_camera.transform.position = target_list[ _target_key ];
		event_camera.transform.LookAt( target_list[ _target_key ] );
		_target_key++;
		_move_speed = ( event_camera.transform.position - target_list[ _target_key ] ).magnitude;
		_move_speed = _move_speed / MOVE_SPEED;
		_event_mode = EVENT_MODE.EVENT_MODE_STAGE;
		StopMainCamera ();
	}

	private void StopMainCamera( ) {
		_event_time = 0;
		_operation.enabled = false; 
		event_camera.gameObject.SetActive( true );
		main_camera.gameObject.SetActive( false );
	}

	private void StartMainCamera( ) {
		_event_time = 0;
		_target_key = 0;
		_operation.enabled = true;
		event_camera.gameObject.SetActive( false );
		main_camera.gameObject.SetActive( true );
		_event_mode = EVENT_MODE.EVENT_MODE_NONE;
	}
}
