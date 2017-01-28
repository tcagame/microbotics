using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour {
	//ギミックのマネージャーをかく
	private string _propeller_fan_switch_tag = "ProperaFunSwitch" ;
	private string _coal_fan_switch_tag = "CoalFunSwitch";
	private CameraManager _camara_mgr;
	private CoalManager _coal_mgr;
	private CoalFanManager _coal_fan_mgr;
	private PlayerManager _player_mgr;
	private PropellerManager _propeller_mgr;
	private PropellerFanManager _propeller_fan_mgr;
	private JackManager _jack_mgr;
	private Operation _operation;
	public GameObject Goal;
	//ナビゲーションマーカー
	private GameObject _navi;
	[SerializeField]
	private GameObject[ ] _navigate;
	private int _total_object_play_count;
	//イベントカメラ
	[SerializeField]
	private GameObject[ ] _event_camera_pos;
	[SerializeField]
	private GameObject[ ] _event_target_pos;
	private int _event_camera_move_num;

	void Awake ( ) {
		_operation = GameObject.Find( "GameManager" ).GetComponent< Operation >( );
		_player_mgr = GameObject.Find( "Player" ).GetComponent< PlayerManager >( );
		_camara_mgr = gameObject.GetComponent< CameraManager >( );
		_coal_mgr = GameObject.Find( "SEKITAN" ).GetComponent< CoalManager >( );
		_coal_fan_mgr = GameObject.Find( "BigFan" ).GetComponent< CoalFanManager >( );
		_propeller_mgr = GameObject.Find( "Propera" ).GetComponent< PropellerManager >( );
		_propeller_fan_mgr = GameObject.Find( "SmallFan" ).GetComponent< PropellerFanManager >( );
		_jack_mgr = GameObject.Find( "Jack" ).GetComponent< JackManager >( );
		_total_object_play_count = 0;
	}

	void Start( ) {
		_camara_mgr.useStageViewCamera( );
	}
	
	// Update is called once per frame
	void Update () {
		if ( Time.timeSinceLevelLoad == 0 ) {
			_camara_mgr.useStageViewCamera ();
		}
		if ( _total_object_play_count >= _navigate.Length ) {
			_total_object_play_count = _navigate.Length - 1;
		}
		if ( _event_camera_move_num >= _event_camera_pos.Length ) {
			_event_camera_move_num = _event_camera_pos.Length - 1;
		}
		moveNavigate ( );
		string tag = _operation.getHitRaycastTag ( );
		string player_touch_tag = _player_mgr.getTouchObjectTag( );
		Vector3 camera_pos = _event_camera_pos[ _event_camera_move_num ].transform.position;
		Vector3 camera_vie_pos = _event_target_pos[ _event_camera_move_num ].transform.position;
		if ( !_camara_mgr.isPlayCamera( ) ) {
			return;
		}
		if ( tag == _propeller_fan_switch_tag && 
			 player_touch_tag == _propeller_fan_switch_tag &&
			 !_propeller_fan_mgr.isActive( ) ){
			_propeller_fan_mgr.action ( );
			_camara_mgr.useEventCamera( camera_pos, camera_vie_pos );
			_event_camera_move_num++;
			_total_object_play_count++;
			GameObject.Find( "FanSwitch" ).GetComponentInChildren<Renderer>().material.SetColor("_RimColor", new Color (0, 0, 0));
			return;
		}
		if ( tag == _coal_fan_switch_tag && 
			 player_touch_tag == _coal_fan_switch_tag &&
			 !_coal_fan_mgr.isActive( ) ) {
			_coal_fan_mgr.action( );
			_camara_mgr.useEventCamera( camera_pos, camera_vie_pos );
			_event_camera_move_num++;
			_total_object_play_count++;
			GameObject.Find( "BigFanSwitch" ).GetComponentInChildren<Renderer>().material.SetColor("_RimColor", new Color (0, 0, 0));

			return;
		}
		if ( tag == _jack_mgr.tag && 
			 player_touch_tag == _jack_mgr.tag &&
			 !_jack_mgr.isActive( ) ) {
			_jack_mgr.action( );
			_camara_mgr.useEventCamera( camera_pos, camera_vie_pos );
			_event_camera_move_num++;
			_total_object_play_count++;
			return;
		}
		if ( tag == _propeller_mgr.tag && 
			 player_touch_tag == _propeller_mgr.tag &&
			 !_propeller_mgr.isActive( ) ){
			_propeller_mgr.action( );
			_total_object_play_count++;
			return;
		}
		if ( _coal_fan_mgr.isActive( ) &&
			!_coal_mgr.isActive( ) ) {
			_coal_mgr.action( );
			_camara_mgr.useEventCamera( camera_pos, camera_vie_pos );
			_event_camera_move_num++;
			_total_object_play_count++;
			return;
		}
		if ( ( Goal.transform.position - _player_mgr.transform.position ).magnitude < 3) {
			_player_mgr.SetClear( );
		}
		if ( _player_mgr.isDeadMotionEnd( ) ) {
			SceneManager.LoadScene( "GameOver" );
		}
		if ( _player_mgr.isClearMotionEnd( ) ) {
			SceneManager.LoadScene( "GameClear" );
		}
	}

	private void moveNavigate( ) {
		switch (_total_object_play_count) {
		case 1:
			GameObject.Find( "FanSwitch" ).GetComponentInChildren<Renderer>().material.SetColor("_RimColor", new Color (0, 1, 0));
			break;
		case 2:
			for (int i = 0; i < _propeller_mgr.Part.Count; i++) {
				_propeller_mgr.Part[ i ].GetComponent<Renderer>().material.SetColor("_RimColor", new Color (0, 1, 0));
			}
			break;
		case 3:
			GameObject.Find( "BigFanSwitch" ).GetComponentInChildren<Renderer>().material.SetColor("_RimColor", new Color (0, 1, 0));
			break;
		}
	}
}
