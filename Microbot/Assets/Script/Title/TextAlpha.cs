using UnityEngine;
using System.Collections;

public class TextAlpha : MonoBehaviour {
	public float FadeTime = 1.0f;
	public float MinAlpah = 0.0f;
	private float fade = 0.0f;
	private float alpha = 0.0f;
	private SpriteRenderer sprender;
	// Use this for initialization
	void Start () {
		fade = FadeTime;
		sprender = GetComponent< SpriteRenderer >( );
		alpha = sprender.color.a;
	}
	
	// Update is called once per frame
	void Update () {
		if ( alpha >= 1.0f || alpha <= ( MinAlpah / 100 ) ) {
			fade *= -1;
		} 
		alpha += fade / 100;
		Color color = sprender.color;
		color.a = alpha;
		sprender.color = color;
	}
}
