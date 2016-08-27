using UnityEngine;
using UnityEditor;
using System.Collections;

public class EventCamera : MonoBehaviour {
    public double EVENT_TIME;
    private double _event_time;
    private Camera main_camera;
    private Camera event_camera;

    // Use this for initialization
    void Start( ) {
        main_camera = Camera.main;
       //イベントカメラの生成
        event_camera.enabled = false;
        _event_time = 0;
	}
	// Update is called once per frame
	void Update( ) {
        _event_time += Time.deltaTime;
        if ( _event_time > EVENT_TIME ) {
            _event_time = 0;
            event_camera.enabled = false;
            main_camera.enabled = true;
        }
	}

    public void CallEventCamera( Vector3 pos, Vector3 target ) {
        if ( event_camera.enabled == true ) {
            return;
        }
        transform.position = pos;
        transform.LookAt( target );
        _event_time = 0;
        event_camera.enabled = true;
        main_camera.enabled = false;
    }
}
