using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GaugeManager : MonoBehaviour {
	private const float GAUGE_STEP = 200.0f;
	private GameObject[] _per = new GameObject[5];
	private GameObject _player;
	private float _gauge;
	private int _per_idx;

	void Start( ) {
		_per_idx = -1;
		_player = GameObject.Find ( "Player" ).gameObject;

		_per[ 0 ] = GameObject.Find ( "Per20" ).gameObject;
		_per[ 1 ] = GameObject.Find ( "Per40" ).gameObject;
		_per[ 2 ] = GameObject.Find ( "Per60" ).gameObject;
		_per[ 3 ] = GameObject.Find ( "Per80" ).gameObject;
		_per[ 4 ] = GameObject.Find ( "Per100" ).gameObject;

	}
	
	void Update( ) {
		_gauge = _player.GetComponent< PlayerManager >( ).getGauge( );
		/*for ( int i = 4; i >= 0; i-- ) {
			if ( _gauge < GAUGE_STEP * 2 ) {
				_per[ i ].GetComponent< Image >( ).material.color = Color.red;
			} else {
				_per[ i ].GetComponent< Image >( ).material.color = Color.blue;
			}
			if ( _gauge < i * GAUGE_STEP ) {
				_per[ i ].SetActive( false );
			} else {
				_per[ i ].SetActive( true );
			}
		}*/
		int per = 0;
		if (_gauge <= 0) {
			per = 0;
		} else {
			per = ( int )( _gauge / GAUGE_STEP);
		}
		if (per > 4) {
			per = 4;
		} 
		if ( per == _per_idx ) {
			return;
		}
		for ( int i = 0; i < 5; i++ ) {
			_per [i].SetActive (false);
		}


		_per [per].SetActive (true);
		_per_idx = per;
	}
}
