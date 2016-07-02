using UnityEngine;
using System.Collections;

public class Click : MonoBehaviour {
	private Vector3 _mouse_position;
	private bool _is_mouse_click;

	private Vector3 _screen_to_world_pos;
	private Vector3 _target_pos;

	void Awake(){

	}

	void Start () {
		_target_pos = new Vector3 ( );
		_is_mouse_click = false;
	}
	
	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			_mouse_position = Input.mousePosition;
			_is_mouse_click = true;
		}
		if ( !_is_mouse_click ) {
			return;
		}
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (_mouse_position);
		if (Physics.Raycast (ray, out hit)) {
			tag = hit.collider.gameObject.tag;
			if ( tag != null && tag != "Player" ) {
				_target_pos = hit.point;	
				_target_pos = new Vector3 ( _target_pos.x, 0, _target_pos.z );
				_is_mouse_click = false;
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


