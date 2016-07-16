using UnityEngine;
using System.Collections;

public class AnimatorController : MonoBehaviour {

	private Animator _playerAnimator;

	void Start( ) {
		_playerAnimator = GameObject.Find( "Player" ).GetComponent< Animator >( );
	}

	public void setRunning( bool play_flag ) {
		_playerAnimator.SetBool( "_is_running", play_flag );
	}

	public void playCharging( bool charge_flag ) {
		_playerAnimator.SetBool( "_is_charging", charge_flag );
	}

	public void playDisCharge( bool discharge_flag ) {
		_playerAnimator.SetBool( "_is_discharging", discharge_flag );
	}
}
