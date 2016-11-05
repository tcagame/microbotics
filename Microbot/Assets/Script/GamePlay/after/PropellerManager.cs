using UnityEngine;
using System.Collections;

public class PropellerManager : MonoBehaviour {
	private enum STATE {
		STATE_UP,
		STATE_DOWN
	}

	private enum PLACE {
		PLACE_3
	}

	private GameObject _propeller;
	public string PropellerName;
	private GameObject _player;
	public string PlayerName;

	public const float FLY_SPEED = 0.05f;

	public Vector3 PROPELLER_START_POS = new Vector3( -15.0f, 1.8f, 0.0f );
	private float MAX_HEIGHT = 9.0f;

	private bool _flag;



	void Awake( ) {
		_propeller = GameObject.Find (PropellerName).gameObject;
		_propeller.gameObject.transform.position = PROPELLER_START_POS;
	}
	// Use this for initialization
	void Start( ) {
		_player = GameObject.Find( PlayerName ).gameObject;
	}
	
	// Update is called once per frame
	void Update( ) {
		
	}






	public void action( ) {
		_flag = true;
	}

	public bool isActive( ) {
		return _flag;
	}
}
