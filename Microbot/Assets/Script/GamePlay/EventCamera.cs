using UnityEngine;
using UnityEditor;
using System.Collections;

public class EventCamera : MonoBehaviour {
    public double EVENT_TIME;
    private double _event_time;
    public Camera main_camera;
    public Camera event_camera;

    // Use this for initialization
    void Start( ) {
        event_camera.gameObject.SetActive( false );
        _event_time = 0;
	}
	// Update is called once per frame
	void Update( ) {
        _event_time += Time.deltaTime;
        if ( _event_time > EVENT_TIME ) {
            _event_time = 0;
            Time.timeScale = 1;
            event_camera.gameObject.SetActive( false );
            main_camera.gameObject.SetActive( true );
        }
	}

    public void CallEventCamera( Vector3 pos, Vector3 target ) {
        if ( event_camera.gameObject.active == true ) {
            return;
        }
        transform.position = pos;
        transform.LookAt( target );
        _event_time = 0;
        Time.timeScale = 0;
        event_camera.gameObject.SetActive( true );
        main_camera.gameObject.SetActive( false );
    }
}
