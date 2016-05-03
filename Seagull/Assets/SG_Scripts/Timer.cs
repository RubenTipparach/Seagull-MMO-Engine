using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Timer : NetworkBehaviour {

    float destroyTimer = 5;

    [SerializeField]
    int damage = 10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        destroyTimer -= Time.deltaTime;

        if (destroyTimer <= 0)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Called when [collision enter].
    /// </summary>
    /// <param name="collision">The collision.</param>
    [Server]
    void OnCollisionEnter(Collision collision)
    {
        BirdNetwork birdNet = collision.gameObject.GetComponent<BirdNetwork>();

        if (birdNet != null)
        {
            birdNet.TakeDamage(damage);
            Debug.Log("BirdHHIT! " + birdNet.Health);

            NetworkServer.Destroy(this.gameObject);
        }
    }
}
