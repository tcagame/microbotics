using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	private GameObject _player;
	private GameObject _goal;

	private float _gauge;

	void Start( ) {
		_player = GameObject.Find( "Player" ).gameObject;
		_goal = GameObject.Find( "Goal" ).gameObject;
	}
	
	void Update( ) {
		_gauge = _player.GetComponent< PlayerManager >( ).getGauge( );
		if ( _gauge < 0 ) {
			SceneManager.LoadScene( "GameOver" );
		}
		if ( Vector3.Distance( _player.transform.position, _goal.transform.position ) < 2.0f ) {
			SceneManager.LoadScene( "GameClear" );
		}
	}
}
