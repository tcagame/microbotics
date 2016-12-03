using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {

	private enum CAMERA_MODE {
		PLAY,
		EVENT,
		STAGE_VIEW
	};

	private Vector3 STAGE_VIEW_START = new Vector3( 0, 0, 0 );
	public List<Vector3> STAGE_VIEW_LIST;

	private PlayCamera _play_camera;
	private EventCamera _event_camera;
	private StageViewCamera _stage_view_camera;
	private CAMERA_MODE _camera_mode; 
	private GameObject _camera_slider;

	void Awake( ) {
		_play_camera = new PlayCamera( );
		_event_camera = new EventCamera( );
		_stage_view_camera = new StageViewCamera( );
		_camera_mode = CAMERA_MODE.PLAY;
		_camera_slider = GameObject.Find( "CameraSlider" );
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		//カメラ切替
		_camera_slider.SetActive( false );
		switch ( _camera_mode ) {
		case CAMERA_MODE.PLAY:
			_camera_slider.SetActive (true);
			_play_camera.update( );
			break;
		case CAMERA_MODE.EVENT:
			_event_camera.update( );
			if ( _event_camera.isfinish( ) ) {
				_camera_mode = CAMERA_MODE.PLAY;
			}
			break;
		case CAMERA_MODE.STAGE_VIEW:
			if ( _stage_view_camera.isfinish( ) ) {
				_camera_mode = CAMERA_MODE.PLAY;
			}
			break;
		}
	}

	public void useEventCamera( Vector3 target, Vector3 pos ) {
		_event_camera.callEventCamera( target, pos );
		_camera_mode = CAMERA_MODE.EVENT;
	}

	public void useStageViewCamera( ) {
		_stage_view_camera.callStageViewCamera( STAGE_VIEW_START, STAGE_VIEW_LIST );
		_camera_mode = CAMERA_MODE.STAGE_VIEW;
	}

	public bool isPlayCamera( ) {
		return _camera_mode == CAMERA_MODE.PLAY;
	}
}
