using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {
	//ギミックのマネージャーをかく
	string _propeller_fan_switch_tag;
	string _coal_fan_switch_tag;
	CameraManager _camara_mgr;
	CoalManager _coal_mgr;
	CoalFanManager _coal_fan_mgr;
	PlayerManager _player_mgr;
	PropellerManager _propeller_mgr;
	PropellerFanManager _propeller_fan_mgr;
	JackManager _jack_mgr;
	Operation _operation;
	//ナビゲーションマーカー
	GameObject _navi;

	void Awake ( ) {
		_navi = ( GameObject )Resources.Load( "Prefab/Navi" );
		_navi = Instantiate( _navi );
		_operation = GameObject.Find( "Operation" ).GetComponent< Operation >( );
		_player_mgr = GameObject.Find( "Player" ).GetComponent< PlayerManager >( );
		_camara_mgr = gameObject.GetComponent< CameraManager >( );
		_coal_mgr = GameObject.Find( "SEKITAN" ).GetComponent< CoalManager >( );
		_coal_fan_mgr = GameObject.Find( "BigFanSwitch" ).GetComponent< CoalFanManager >( );
		_propeller_mgr = GameObject.Find( "Propera" ).GetComponent< PropellerManager >( );
		_propeller_fan_mgr = GameObject.Find( "FabSwitch" ).GetComponent< PropellerFanManager >( );
		_jack_mgr = GameObject.Find( "Jack" ).GetComponent< JackManager >( );
	}

	void Start( ) {
		_navi.transform.position = _jack_mgr.transform.position + Vector3.left * 2;
	}
	
	// Update is called once per frame
	void Update () {
		string tag = _operation.getHitRaycastTag ( );
		string player_touch_tag = _player_mgr.getTouchObjectTag( );
		Vector3 camera_pos = new Vector3 (0, 0, 0);
		Vector3 camera_vie_pos = new Vector3 (1, 1, 1);
		if ( !_camara_mgr.isPlayCamera( ) ) {
			return;
		}
		if ( tag == _propeller_fan_switch_tag && 
			 player_touch_tag == _propeller_fan_switch_tag &&
			 !_propeller_fan_mgr.isActive( ) ){
			_propeller_fan_mgr.action ( );
			_camara_mgr.useEventCamera( camera_pos, camera_vie_pos );
			return;
		}
		if ( tag == _coal_fan_switch_tag && 
			 player_touch_tag == _coal_fan_switch_tag &&
			 !_coal_fan_mgr.isActive( ) ) {

			_coal_fan_mgr.action( );
			_camara_mgr.useEventCamera( camera_pos, camera_vie_pos );
			return;
		}
		if ( tag == _jack_mgr.tag && 
			 player_touch_tag == _jack_mgr.tag &&
			 !_jack_mgr.isActive( ) ) {

			_jack_mgr.action( );
			_camara_mgr.useEventCamera( camera_pos, camera_vie_pos );
			return;
		}
		if ( tag == _propeller_mgr.tag && 
			 player_touch_tag == _propeller_mgr.tag &&
			 !_propeller_mgr.isActive( ) ){

			_propeller_mgr.action( );
			_camara_mgr.useEventCamera( camera_pos, camera_vie_pos );
			return;
		}
		if ( _coal_fan_mgr.isActive( ) &&
			!_coal_mgr.isActive( ) ) {

			_coal_mgr.action( );
			_camara_mgr.useEventCamera( camera_pos, camera_vie_pos );
			return;
		}
	}
}
