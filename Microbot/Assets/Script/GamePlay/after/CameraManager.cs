using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private enum CAMERA_MODE {
		PLAY,
		EVENT,
		STAGE_VIEW
	};
	private PlayCamera _play_camera;
	private EventCamera _event_camera;
	private StageViewCamera _stage_view_camera;
	private CAMERA_MODE _camera_mode; 

	void Awake( ) {
		_play_camera = new PlayCamera( );
		_event_camera = new EventCamera( );
		_stage_view_camera = new StageViewCamera( );
		_camera_mode = CAMERA_MODE.PLAY;
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		switch ( _camera_mode ) {
		case CAMERA_MODE.PLAY:
			_play_camera.update( );
			break;
		case CAMERA_MODE.EVENT:
			_event_camera.update( );
			if ( _event_camera.finish( ) ) {
				_camera_mode = CAMERA_MODE.PLAY;
			}
			break;
		case CAMERA_MODE.STAGE_VIEW:
			_stage_view_camera.update( );
			if ( _stage_view_camera.finish( ) ) {
				_camera_mode = CAMERA_MODE.PLAY;
			}
			break;
		}
	}

	public void callEventCamera( Vector3 target, Vector3 pos ) {
		_event_camera.callEnventCamera( target, pos );
		_camera_mode = CAMERA_MODE.EVENT;
	}

	public void callStageViewCamera( ) {
		_stage_view_camera.initialize( );
		_camera_mode = CAMERA_MODE.STAGE_VIEW;
	}
}
