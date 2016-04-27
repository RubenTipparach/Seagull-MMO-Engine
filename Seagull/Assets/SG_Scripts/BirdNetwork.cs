using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BirdNetwork : NetworkBehaviour {

    [SerializeField]
    Bird bird;

    [SerializeField]
    Camera cameraMain;

    [SerializeField]
    AudioListener audioListener;

    [SerializeField]
    NetworkAnimator networkAnimator;

    private Animator animator;

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    void Start () {
	    if(isLocalPlayer)
        {
            bird.enabled = true;
            cameraMain.enabled = true;
            audioListener.enabled = true;

            
        }
        else
        {
            animator = bird.Anim;
        }
	}

    /// <summary>
    /// Called when the local player object has been set up.
    /// </summary>
    public override void OnStartLocalPlayer()
    {
        // base.OnStartLocalPlayer();
        networkAnimator.SetParameterAutoSend(0, true); 
    }

    public override void PreStartClient()
    {
        // base.PreStartClient();
        networkAnimator.SetParameterAutoSend(0, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            bool landed = true;

            RaycastHit hit;
            
            if (Physics.Raycast(new Ray(bird.transform.position, -bird.transform.up), out hit))
            {
                //Debug.Log(hit.distance);
                if (hit.distance > 1)
                {
                    landed = false;
                }
            }

            if (!landed)
            {
                animator.Play("Flap");
                animator.SetFloat("Flap", 1);
            }
            else if (landed)
            {
                //animator.SetFloat("TakeOff", 0);
                animator.SetFloat("Walk", 0);
                animator.Play("Idle");
            }
        }
    }
}
