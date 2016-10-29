using UnityEngine;
using System.Collections;

public class PropellerManager : MonoBehaviour {
	private GameObject _propeller;
	public string PropellerName;
	private GameObject _player;

	public const float FLY_SPEED = 0.05f;

	private Vector3 PROPELLER_START_POS = new Vector3 ( -15.0f, 1.8f, 0.0f);
	private float MAX_HEIGHT = 9.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
