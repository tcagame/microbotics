using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class testlog : MonoBehaviour {
	private Touch touch;

	// Use this for initialization
	void Start () {
		touch = GameObject.Find ("Touch").GetComponent<Touch> ();
	}
	
	// Update is called once per frame
	void Update () {
		string move;
		if (touch._move_touch) {
			move = " move:true ";
		} else {
			move = " move:false ";
		}
		string multi;
		if (touch._is_multi_touch) {
			multi = " multi:true ";
		} else {
			multi = " multi:false ";
		}
		string click;
		if (touch._is_touch_click) {
			click = " click:true ";
		} else {
			click = " click:false ";
		}

		GetComponent< Text > ().text = Input.touchCount.ToString () + move + multi + click;
	}
}
