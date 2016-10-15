using UnityEngine;
using System.Collections;

public class NevyManager : MonoBehaviour {

    private Vector3 _pos;

	// Use this for initialization
	void Start () {
        _pos = new Vector3(-20.65f, 2.74f, 13.1f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = _pos;
	}

    public void setPos(Vector3 pos)
    {
        pos = _pos;
    }
}
