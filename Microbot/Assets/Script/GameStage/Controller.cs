using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	private Animator animator;


	void Start () {
		animator = GetComponent<Animator>();
	}

	void Update () {
        if (Input.GetKey("up")){
            transform.position += transform.forward * 0.03f;
            animator.SetBool("_is_running", true);
        }else{
            animator.SetBool("_is_running", false);
        }

        if ( Input.GetKey( "right" ) ){
            transform.Rotate(0, 2f, 0); 
        }
        if (Input.GetKey("left"))
        {
            transform.Rotate(0, -2f, 0);
        }
        if (Input.GetKey("down")) {
            animator.SetTrigger("_jump_trigger");
            transform.position += transform.up * 0.03f;

        }



    }
}
