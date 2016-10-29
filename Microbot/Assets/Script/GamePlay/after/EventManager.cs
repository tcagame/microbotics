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



	// Use this for initialization
	void Start ( ) {
		
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
