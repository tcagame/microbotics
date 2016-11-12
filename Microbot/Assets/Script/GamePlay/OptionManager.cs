using UnityEngine;
using System.Collections;

public class OptionManager : MonoBehaviour {
	private GameObject _window;
	private GameObject _bgm;
	bool _switch;

	// Use this for initialization
	void Start () {
		_window = GameObject.Find ("Window");
		_switch = false;
	}

	// Update is called once per frame
	void Update () {
		_window.SetActive (_switch);
	}

	public void isClick() {
		if (_switch) {
			_switch = false;
		} else {
			_switch = true;
		}
	}
}
