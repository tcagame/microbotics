using UnityEngine;
using System.Collections;

public class JackManager : MonoBehaviour {

    public float MaxScaleY = 10;
	private float _scale_y;
	private float _old_scale_y;
	private float _gauge;
	private GameObject _set;
	public string TrapName = "JackSet";

	void Start( ) {
        _old_scale_y = transform.localScale.y;
		_gauge = 100;
		_set = GameObject.Find( TrapName ).gameObject;
	}

	void Update( ) {
		_scale_y = _gauge * 1.0f / 50.0f;

	}

	public void playJack( ) {
        Vector3 col = GetComponent<BoxCollider>( ).size;
        Vector3 center = GetComponent<BoxCollider>().center;
        center.y += ( Mathf.Abs( _old_scale_y - _scale_y) ) / 2;
        col.y += ( Mathf.Abs( _old_scale_y - _scale_y ) ) / 2;
        GetComponent<BoxCollider>( ).size = col;
        GetComponent<BoxCollider>().center = center;

     
        if ( _old_scale_y != _scale_y ) {
		    Vector3 s_pos = _set.transform.position;
            s_pos.y += (Mathf.Abs( _old_scale_y - _scale_y) ) / 2;
		    _set.transform.position = s_pos;
        }
        _old_scale_y = _scale_y;
	}

	public void giveGauge( float gauge ) {
        if (_scale_y < MaxScaleY) {
			_gauge += gauge;
		}
	}
}