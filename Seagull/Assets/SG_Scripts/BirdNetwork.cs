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

    [SyncVar]
    public int Health = 100;

    [SerializeField]
    Renderer birdColor;

	public short serverId;

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
		// The player can trigger messages, and the engine should sync....[crap] (•_•) ( •_•)>⌐■-■ (⌐■_■) YEEEAAAAHHH!!!!!!
		if (isLocalPlayer)
		{
			if (Input.GetKeyDown(KeyCode.C))
			{
				CmdPoop();
			}

            CmdCheckTerminatePlayer();
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
        poopy.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity + -Vector3.up *10;

        NetworkServer.Spawn(poopy);
    }

	/// <summary>
	/// Client - Server command. Allows the server to terminate player.
	/// This just also terminates the connection whether its Host or Client lol.
	/// </summary>
	[Command]
    void CmdCheckTerminatePlayer()
    {
        if(Health <= 0)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }

	/// <summary>
	/// Server - Client. When a bird gets hit or whatever, a bird tells the server that
	/// it requests this method be called to the other bird.
	/// </summary>
	/// <param name="amount">The amount.</param>
	[Server]
    public void TakeDamage(int amount)
    {
        // will only work on server
        Health -= amount;

        // birdColor.material.color = ColorSwitch();
        RpcPushClientColor();
    }

	/// <summary>
	/// The Server - Client remotely calls this method on a client to change the color.
	/// Oddly the bird colors or slightly out of sync, I wonder why.
	/// </summary>
	[ClientRpc]
    public void RpcPushClientColor()
    {
        var colorPicked = ColorSwitch();
        birdColor.material.color = colorPicked;
        CmdChangeColor(colorPicked);
    }

	/// <summary>
	/// Client - Server commands to change the color.
	/// </summary>
	/// <param name="colorPicked"></param>
	[Command]
    public void CmdChangeColor(Color colorPicked)
    {
        birdColor.material.color = colorPicked;
    }

	/// <summary>
	/// Colors the switch.
	/// </summary>
	/// <returns></returns>
	private Color ColorSwitch()
    {
        switch ( Health)
        {
            case 90:
                return Color.blue;
            case 80:
                return Color.green;
            case 70:
                return Color.cyan;
            case 60:
                return Color.yellow;
            case 50:
                return new Color(1, .41f, 0);
            case 40:
                return Color.red;
            case 30:
                return Color.magenta;
            case 20:
                return Color.white;
            case 10:
                return Color.white;
            default:
                return Color.white;
        }
    }
}
