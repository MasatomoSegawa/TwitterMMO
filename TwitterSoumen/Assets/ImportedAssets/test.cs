using UnityEngine;
using System.Collections;
//using CoreTweet;

public class test : MonoBehaviour {

    public string consumerKey;
    public string consumerSecret;
    public string accessToken;
    public string accessSecret;

	// Use this for initialization
	void Start () {
	
		TwitterPluginManager.Instance.OnRegisterAccessTokenEndEvent += (bool success) => {

			Debug.Log(success);

		};		                                                              
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
