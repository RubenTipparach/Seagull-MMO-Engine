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
            if(hit.distance > 2)
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

        PlayGestures();

        // Landed on the ground.
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
				animator.SetFloat("Walk", 1);
				rigidBody.AddForce(transform.forward * Time.deltaTime * -1000);
				isIdle = false;
			}
           
        }
        else
        {
            // Do this while walking around
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

        // Rotate around and stuff
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * 100 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Plays the set of bird gestures.
    /// </summary>
    /// <returns></returns>
    bool PlayGestures()
    {
        //animator.Play("Idle");
        //Debug.Log("Peck pressed");
        //Peck
        if (Input.GetKeyDown(KeyCode.P))
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(transform.position + Vector3.up, transform.forward + Vector3.up), out hit))
            {
                Debug.Log(hit.distance);
                if (hit.distance < 7)
                {
                    var bn = hit.transform.gameObject.GetComponent<BirdNetwork>();
                    if (bn != null)
                    {
                        bn.TakeDamage(10);
                        Debug.Log("Seagull hit!");
                    }
                }
            }

            animator.Play("Peck");
        }

        //Small Bow
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.Play("Small_Bow");
        }

        //Big bow
        if (Input.GetKeyDown(KeyCode.T))
        {
            animator.Play("Full_Bow");
        }

        // Wave
        if (Input.GetKeyDown(KeyCode.Y))
        {
            animator.Play("Wave");
        }

        //Lean Right
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.Play("LeanLeft");
        }

        //Lean Left
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.Play("LeanRight");
        }

        return true;
    }
}
