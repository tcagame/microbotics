using UnityEngine;
using System.Collections;

public class Operation : MonoBehaviour {
	enum MODE {
		MODE_TOUCH,
		MOED_CLICK,
		MOED_ERROR
	};
	private MODE _mode = MODE.MOED_ERROR;

	private string _target_object_tag;
	private Vector3 _raycast_pos;

	void Awake( ) {
		//プラットフォームの確認
		if (isTouchPlatform( ) ) {
			_mode = MODE.MODE_TOUCH;
		} else if ( isClickPlatform( ) ) {
			_mode = MODE.MOED_CLICK;
		} else {
			_mode = MODE.MOED_ERROR;
		}
	}

	void Update( ) {
		if ( _mode == MODE.MOED_ERROR ) {
			return;
		}
		//基本的に入力された位置をプレイヤーに重ならないように取得している。
		if ( isSingleInput( ) && getTouchPhase( 0 ) == TouchPhase.Ended ) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay( getInputPosition( 0 ) );
			if ( Physics.Raycast( ray, out hit) ) {
				_target_object_tag = hit.collider.gameObject.tag;
				if ( _target_object_tag != null && _target_object_tag != "Player" ) {
					_raycast_pos = hit.point;	
				}
			}
		}
	}

	//タッチで動くプラットフォームの判定
	private bool isTouchPlatform( ) {
		bool result = false;
		if ( Application.platform == RuntimePlatform.Android ) {
			result = true;
		}
		if ( Application.platform == RuntimePlatform.IPhonePlayer ) {
			result = true;
		}
		return result;
	}

	//クリックで動くプラットフォームの判定
	//現状はUnityEditorのみ
	private bool isClickPlatform( ) {
		bool result = false;
		if ( Application.platform == RuntimePlatform.OSXEditor ) {
			result = true;
		}
		if ( Application.platform == RuntimePlatform.WindowsEditor ) {
			result = true;
		}
		return result;
	}

	//インプットされている数
	public int getInputCount( ) {
		int result = 0;
		if ( _mode == MODE.MODE_TOUCH ) {
			result = Input.touchCount;
		}
		if ( _mode == MODE.MOED_CLICK ) {
			if ( Input.GetMouseButtonUp( 0 ) ) {
				result = 1;
			}
		}
		return result;
	}

	//入力の画面ポジション取得
	public Vector3 getInputPosition( int id = 0 ) {
		Vector3 result = new Vector3( );
		if ( _mode == MODE.MODE_TOUCH ) {
			result = Input.GetTouch( id ).position;
		}
		if ( _mode == MODE.MOED_CLICK ) {
			result = Input.mousePosition;
		}
		return result;
	}

	//入力の状態取得
	public TouchPhase getTouchPhase( int id = 0 ) {
		TouchPhase result = TouchPhase.Canceled;
		if ( _mode == MODE.MOED_CLICK ) {
			result = TouchPhase.Ended;
		}
		if ( _mode == MODE.MODE_TOUCH ) {
			result = Input.GetTouch( id ).phase;
		}
		return result;
	}

	//レイキャストにヒットしたオブジェクトのポジション
	public Vector3 getHitRaycastPos( ) {
		return _raycast_pos;
	}

	//レイキャストにヒットしたオブジェクトのタグ
	public string getHitRaycastTag ( ) {
		return _target_object_tag;
	}

	//マルチインプットの判定
	public bool isMultiInput( ) {
		if ( getInputCount( ) > 1 ) {
			return true;
		}
		return false;
	}
	//レイキャストの初期化
	public void resetTargetPos( ) {
		_raycast_pos = new Vector3( );
	}

	public void resetTargetTag( ) {
		_target_object_tag = null;
	}

	//シングルインプットの判定
	public bool isSingleInput( ) {
		if ( getInputCount( ) == 1 ) {
			return true;
		}
		return false;
	}
}
