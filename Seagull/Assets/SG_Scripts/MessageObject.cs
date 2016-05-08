using UnityEngine;
using System.Collections;

public class MessageObject : MonoBehaviour {

	/// <summary>
	/// The contents message.
	/// </summary>
	public string message;

	/// <summary>
	/// The left by user. Who did this?
	/// </summary>
	public string leftByUser;

	/// <summary>
	/// The text.
	/// </summary>
	private TextMesh _text;

	/// <summary>
	/// The default message.
	/// </summary>
	private string defaultMessage = "{...}";

	// Use this for initialization
	void Start () {
		_text = GetComponent<TextMesh>();
		_text.text = defaultMessage;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other)
	{
		if (!string.IsNullOrEmpty(message)
			&& !string.IsNullOrEmpty(leftByUser))
		{
			_text.text = string.Format( "{0} \n-{1}",message,leftByUser);
			// This message thing is to be displayed when client is close enough.
		}
	}

	void OnTriggerExit(Collider other)
	{
		_text.text = "{...}";
    }
}
