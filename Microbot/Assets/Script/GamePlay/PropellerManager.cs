using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PropellerManager : MonoBehaviour {
	private enum STATE {
		STATE_NONE,
		STATE_UP,
		STATE_LEAVE
	}
	public List<GameObject> Part;

	private GameObject _propeller;
	public string PropellerName;
	private GameObject _player;
	public string PlayerName;

	public const float FLY_SPEED = 0.1f;
    public float CURVE = -0.2f;

	public Vector3 PROPELLER_LOW_POS = new Vector3( -15.0f, 1.8f, 0.0f );
    public Vector3 TARGET_POS = new Vector3 ( -15.0f, 5.18f, -11.36f );

    private bool _flag = false;
    private STATE _state;
    public float _z_buffer = 0;
    public float _senter_z = 0;
    public float _before_y = 0;



	void Awake( ) {
		_propeller = GameObject.Find (PropellerName).gameObject;
		_propeller.gameObject.transform.position = PROPELLER_LOW_POS;
	}
	// Use this for initialization
	void Start( ) {
		_player = GameObject.Find( PlayerName ).gameObject;
        _state = STATE.STATE_NONE;
        Vector3 propeller_pos = _propeller.transform.position;
        _senter_z = ( propeller_pos.z - TARGET_POS.z ) / 2;
        _z_buffer = -_senter_z;
	}
	
	// Update is called once per frame
    void Update( ) {
		if ( _flag ) {
			//飛ぶ操作
			flying( );

		}
	}

	private void setPosition( ) {
		Vector3 player_pos = _player.transform.position;
		Vector3 propeller_pos = _propeller.transform.position;
		player_pos = new Vector3( propeller_pos.x, propeller_pos.y - 1.0f, propeller_pos.z );
		_player.transform.position = player_pos;
	}

	private void flying( ) {
		//方向決め
		switch ( _state ) {
		case STATE.STATE_UP:
			setPosition ();
            flyToCurve ( );
            switchDir ( TARGET_POS );
			break;
		case STATE.STATE_LEAVE:
			leavePropeller( );
			break;
		}
	}

	private void flyToCurve( ) {
        Vector3 propeller_pos = _propeller.transform.position;
        _z_buffer += FLY_SPEED;
        float y = _z_buffer * _z_buffer * CURVE;
        if ( _before_y != 0 ) {
            float diff_y = y - _before_y;
            propeller_pos.z += -FLY_SPEED;
            propeller_pos.y += diff_y;
            _propeller.transform.position = propeller_pos;
        }
        _before_y = y;
	}

	private void switchDir( Vector3 target_pos ) {
		Vector3 mid_pos = target_pos;
		Vector3 propeller_pos = _propeller.transform.position;
        float dist = propeller_pos.z - mid_pos.z;
		if (dist < 3 ) {
			_state = STATE.STATE_LEAVE;
		}
	}

	private void leavePropeller( ) {
        Vector3 propeller_pos = _propeller.transform.position;
        _z_buffer -= FLY_SPEED;
        float y = _z_buffer * _z_buffer * CURVE;
        if ( _before_y != 0 ) {
            float diff_y = y - _before_y;
            propeller_pos.z -= -FLY_SPEED;
            propeller_pos.y += diff_y;
            _propeller.transform.position = propeller_pos;
        }
        _before_y = y;
        if ( Vector3.Distance( PROPELLER_LOW_POS , propeller_pos ) < 1 ) {
            _state = STATE.STATE_NONE;
            _propeller.transform.position = PROPELLER_LOW_POS;
            _flag = false;
        }
	}

	//イベントマネージャーから操作
	public void action( ) {
		_flag = true;
		_state = STATE.STATE_UP;
		for (int i = 0; i < Part.Count; i++) {
			Part[ i ].GetComponent<Renderer>().material.SetColor("_RimColor", new Color (0, 0, 0));
		}
	}
	public bool isActive( ) {
		return _flag;
	}
}
