using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JackManager : MonoBehaviour {

	public float MaxScaleY = 0.0032f;
	public float JackSpeed = 0.00035f;

	public List<GameObject> Part;
	private float _scale_y;
	private GameObject _set;
	private bool _flag;
	public string TrapName = "JackSet";

	void Start( ) {
		_flag = false;
		_scale_y = 0;
		_set = GameObject.Find( TrapName ).gameObject;
	}

	void Update( ) {
		if ( _flag && _scale_y <= MaxScaleY ) {
			_scale_y += JackSpeed;
			playJack ( );
		}
	}

	private void playJack( ) {
		Vector3 col = GetComponent<BoxCollider>( ).size;
		Vector3 center = GetComponent<BoxCollider>().center;
		center.y += ( _scale_y / 2 ) * ( _scale_y / 2 );
		col.y += _scale_y / 2;
		GetComponent<BoxCollider>( ).size = col;
		GetComponent<BoxCollider>().center = center;
		for (int i = 0; i < Part.Count; i++) {
			Part[ i ].GetComponent<Renderer> ().material.SetColor ("_RimColor", new Color (0, 0, 0));
		}

		Vector3 s_pos = _set.transform.position;
		s_pos.y += (Mathf.Abs( _scale_y ) ) / 2;
		_set.transform.position = s_pos;

		gameObject.GetComponent<Animator> ().SetTrigger ("_jack_play");
	}

	public void action( ) {
		_flag = true;
	}

	public bool isActive () {
		return _flag;
	}
}