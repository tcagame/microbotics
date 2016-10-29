using UnityEngine;
using System.Collections;

public class JackManager : MonoBehaviour {
	
}

/*旧コード
using UnityEngine;
using System.Collections;

public class JackManager : MonoBehaviour {

    public float MaxScaleY = 0.5f;
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
			_scale_y += 0.01f;
			playJack ( );
		}
	}

	private void playJack( ) {
        Vector3 col = GetComponent<BoxCollider>( ).size;
        Vector3 center = GetComponent<BoxCollider>().center;
        center.y += _scale_y / 2;
        col.y += _scale_y / 2;
        GetComponent<BoxCollider>( ).size = col;
        GetComponent<BoxCollider>().center = center;

     
		Vector3 s_pos = _set.transform.position;
        s_pos.y += (Mathf.Abs( _scale_y ) ) / 2;
		_set.transform.position = s_pos;

		gameObject.GetComponent<Animator> ().SetTrigger ("_jack_play");
	}

	public void play( ) {
		_flag = true;
	}
	public bool getPlay () {
		return _flag;
	}
}
*/