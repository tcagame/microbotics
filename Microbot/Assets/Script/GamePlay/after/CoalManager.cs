using UnityEngine;
using System.Collections;

public class CoalManager : MonoBehaviour {

	bool _action_flag = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( !_action_flag ) {
			return;
		}
		transform.position.x++;
		if (transform.position.x > 100) {
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
