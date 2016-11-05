using UnityEngine;
using System.Collections;

public class PropellerManager : MonoBehaviour {
	private enum STATE {
		STATE_HIGH,
		STATE_LOW
	}

	private GameObject _propeller;
	public string PropellerName;
	private GameObject _player;
	public string PlayerName;

	public const float FLY_SPEED = 0.05f;

	public Vector3 PROPELLER_LOW_POS = new Vector3( -15.0f, 1.8f, 0.0f );
	public Vector3 PROPELLER_HIGH_POS = new Vector3 ( -15.0f, 6.0f, -13.0f);
	private float MAX_HEIGHT = 9.0f;

	private bool _flag;
	private STATE _state;



	void Awake( ) {
		_propeller = GameObject.Find (PropellerName).gameObject;
		_propeller.gameObject.transform.position = PROPELLER_LOW_POS;
	}
	// Use this for initialization
	void Start( ) {
		_player = GameObject.Find( PlayerName ).gameObject;
	}
	
	// Update is called once per frame
	void Update( ) {
		if ( _flag ) {
			//ロボットの頭上にプロペラ設置
			setPosition( );
			//飛ぶ操作
			flying( );
		}
	}

	private void setPosition( ) {
		Vector3 player_pos = _player.transform.position;
		Vector3 p_pos = _propeller.transform.position;
		player_pos = new Vector3( p_pos.x, p_pos.y - 1.3f, p_pos.z );
		_player.transform.position = player_pos;
	}

	private void flying( ) {
		switch ( _state ) {
		case STATE.STATE_LOW:
			//下から上に
			break;
		case STATE.STATE_HIGH:
			//上から下に
			break;
		}
	}



	//イベントマネージャーから操作
	public void action( ) {
		_flag = true;
	}
	public bool isActive( ) {
		return _flag;
	}
}
