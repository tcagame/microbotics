using UnityEngine;
using System.Collections;

public class JackManager : MonoBehaviour {

	private float _scale_y;
	private float _pos_y;
	private float _gauge;
	private GameObject _set;
	public string TrapName = "JackSet";

	void Start( ) {
		_gauge = 100;
		_set = GameObject.Find( TrapName ).gameObject;
	}

	void Update( ) {
		_scale_y = _gauge * 1.0f / 50.0f;
	}

	public void playJack( ) {
		Vector3 scale = transform.localScale;
		scale.y = _scale_y;
		transform.localScale = scale;

		Vector3 s_pos = _set.transform.position;
		s_pos.y = _scale_y;
		_set.transform.position = s_pos;
	}

	public void giveGauge( float gauge ) {
		_gauge += gauge;
	}
}
