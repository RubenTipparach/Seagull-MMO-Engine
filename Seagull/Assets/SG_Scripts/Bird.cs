using UnityEngine;
using System.Collections;


public class Bird : MonoBehaviour {

    [SerializeField]
    Animator animator;

    public Animator Anim
    {
        get
        {
            return animator;
        }
    }

    Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        bool landed = true;

        RaycastHit hit;
        if(Physics.Raycast(new Ray(transform.position, -transform.up), out hit))
        {
            //Debug.Log(hit.distance);
            if(hit.distance > 1)
            {
                landed = false;
            }
        }

        if (Input.GetKey(KeyCode.Space) && landed)
        {
            animator.Play("TakeOff");
            animator.SetFloat("TakeOff", 1);
            rigidBody.AddForce(transform.up * Time.deltaTime * 1000);
        }
        else if (Input.GetKey(KeyCode.Space) && !landed)
        {
            animator.Play("Flap");
            animator.SetFloat("Fly", 1);
            rigidBody.AddForce(transform.up * Time.deltaTime * 1000);
        }
        //else
        //{
        if (landed)
        {
            //animator.SetFloat("TakeOff", 0);
            bool isIdle = true;

            if (Input.GetKey(KeyCode.W))
            {
                animator.Play("Walk");
                animator.SetFloat("Walk", 1);
                rigidBody.AddForce(transform.forward * Time.deltaTime * 1000);
                isIdle = false;
            }

			if (Input.GetKey(KeyCode.S))
			{
				animator.Play("Walk");
				animator.SetFloat("Walk", -1);
				rigidBody.AddForce(transform.forward * Time.deltaTime * -1000);
				isIdle = false;
			}

			if (isIdle)
            {
                animator.SetFloat("Walk", 0);
                animator.Play("Idle");
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                rigidBody.AddForce(transform.forward * Time.deltaTime * 1000);
            }

			if (Input.GetKey(KeyCode.S))
			{
				rigidBody.AddForce(transform.forward * Time.deltaTime * -100);
			}
			//Debug.Log("We're flying!");
		}
       // }

        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * 100 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        }
    }
}
