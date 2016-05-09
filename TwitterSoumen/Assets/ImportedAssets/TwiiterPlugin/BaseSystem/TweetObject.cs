using UnityEngine;
using System.Collections;

public class TweetObject : MonoBehaviour{

    public string tweetText;
    public string screenName;

}

/// <summary>
/// ユーザーの情報を記録する.
/// </summary>
[System.Serializable]
public class TwitterUserInfo{

	public string userId;
	public string screenName;
	public string token;
	public string tokenSecret;

}
