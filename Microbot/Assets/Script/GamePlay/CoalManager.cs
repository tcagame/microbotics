using UnityEngine;
using System.Collections;

public class CoalManager : MonoBehaviour {

	bool _action_flag = false;
	int _time = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( !_action_flag ) {
			_time = 0;
			return;
		}
		//石炭飛ぶ
		transform.position += Vector3.right;
		_time++;
		if ( _time > 100 ) {
			Destroy(gameObject);
		}
	}
	public void action( ) {
		_action_flag = true;
	}
	public bool isActive( ) {
		return _action_flag;
	}
}
