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

    //[SerializeField]
    //NetworkAnimator networkAnimator;

	//[SyncVar]
	//public int health;

	[SerializeField]
	GameObject poopObject;

	[SerializeField]
	Transform poopHole;

	[SerializeField]
	NetworkInitializer talkToServer;

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
        //networkAnimator.SetParameterAutoSend(0, true); 
    }

    public override void PreStartClient()
    {
      //  base.PreStartClient();
       // networkAnimator.SetParameterAutoSend(0, true);
    }

    // Update is called once per frame
    void Update()
    {
		// Automate animations on other client sides.
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
                animator.SetFloat("Fly", 1);
            }
            else if (landed)
            {
                //animator.SetFloat("TakeOff", 0);
                animator.SetFloat("Walk", 0);
                animator.Play("Idle");
            }
        }

		// The player can trigger messages, and the engine should sync....[crap] (•_•) ( •_•)>⌐■-■ (⌐■_■) YEEEAAAAHHH!!!!!!
		if (isLocalPlayer)
		{
			if (Input.GetKeyDown(KeyCode.C))
			{
				CmdPoop();
			}
		}
    }

	/// <summary>
	/// Stores this message in some database stuff to be retrieved later by some other client. Or when the server boots.
	/// </summary>
	void CmdMessagePoop()
	{
		// Ray cast to the ground, instantiate decal lol.
		// Give it ID so that it can be posted.
		// Every bird should get a max of like 5 messages or something.
		// How the F do I get message?
		string message = "";
		talkToServer.WritePoopMessage(message);
    }

	/// <summary>
	/// Commands the poop. lol wut?
	/// </summary>
	[Command]
	void CmdPoop()
	{
		var poopy = (GameObject)Instantiate(poopObject, poopHole.position, poopHole.rotation);
		NetworkServer.Spawn(poopy);
    }
}
