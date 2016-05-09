using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// TwitterPluginのマネージャークラス.
/// このクラスを介してTwitterAPIを叩く
/// 
/// -使い方-
/// 認証を一度するとアクセストークンがこのクラスに保存される.
/// アクセストークンが保存された状態でこのクラスに実装されている各APIメソッドを呼び出して使ってね.
/// 
/// -ユーザーの認証方法-
/// 1. OnGetRequestTokenメソッドをボタンなどのトリガーから呼び出す.
/// 2. ブラウザの認証画面が開かれるので、認証ピンをユーザーにInputFieldなどのUIから入力してもらう.
/// 3. OnPinInputメソッドをInputFieldなどのトリガーから呼び出す.
/// 4. OnAccessTokenCallbackが呼び出されてアクセストークンがPlayerPrefsに登録される.
/// 5. 以降PlayerPrefsから設定されたユーザーのアクセストークンが読み込まれるようになる.
/// 6. あとはお好きに.
/// </summary>
public class TwitterPluginManager : Singleton<TwitterPluginManager>
{

	[Header("ユーザー情報を保存するResourcesからのパス.")]
	public string userInfoSavePath;

    // 認証が終わった際に呼び出されるイベント.
    public delegate void OnRegisterAccessTokenEnd(bool success);
    public event OnRegisterAccessTokenEnd OnRegisterAccessTokenEndEvent;

    // API登録時のコンシュームキー
    public string CONSUMER_KEY;
    public string CONSUMER_SECRET;

    public string AccessToken;
    public string AccessTokenSecret;

    #region ユーザープロパティ.

    // ユーザー情報.
    const string PLAYER_PREFS_TWITTER_USER_ID = "TwitterUserID";
    const string PLAYER_PREFS_TWITTER_USER_SCREEN_NAME = "TwitterUserScreenName";
    const string PLAYER_PREFS_TWITTER_USER_TOKEN = "TwitterUserToken";
    const string PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET = "TwitterUserTokenSecret";

    // リクエストトークン.
    private Twitter.RequestTokenResponse m_RequestTokenResponse;
    public Twitter.RequestTokenResponse requestTokenResponse
    {
        get
        {
            return m_RequestTokenResponse;
        }
    }

    // アクセストークン.
    private Twitter.AccessTokenResponse m_AccessTokenResponse;
    public Twitter.AccessTokenResponse accessTokenResponse
    {
        get
        {
            return m_AccessTokenResponse;
        }
    }

    #endregion

    void Start()
    {
		//LoadTwitterUserInfo();
    }

    /// <summary>
    /// ユーザーデータをロードする.
    /// </summary>
	public void LoadTwitterUserInfo(TwitterUserInfo twitterUserInfo)
    {
        m_AccessTokenResponse = new Twitter.AccessTokenResponse();

		m_AccessTokenResponse.UserId = twitterUserInfo.userId;
		m_AccessTokenResponse.ScreenName = twitterUserInfo.screenName;
		m_AccessTokenResponse.Token = twitterUserInfo.token;
		m_AccessTokenResponse.TokenSecret = twitterUserInfo.tokenSecret;

		/*
        m_AccessTokenResponse.UserId = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_ID);
        m_AccessTokenResponse.ScreenName = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_SCREEN_NAME);
        m_AccessTokenResponse.Token = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_TOKEN);
        m_AccessTokenResponse.TokenSecret = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET);
		*/

        AccessToken = m_AccessTokenResponse.Token;
        AccessTokenSecret = m_AccessTokenResponse.TokenSecret;

        if (!string.IsNullOrEmpty(m_AccessTokenResponse.Token) &&
            !string.IsNullOrEmpty(m_AccessTokenResponse.ScreenName) &&
            !string.IsNullOrEmpty(m_AccessTokenResponse.Token) &&
            !string.IsNullOrEmpty(m_AccessTokenResponse.TokenSecret))
        {
            string log = "LoadTwitterUserInfo - succeeded";
            log += "\n    UserId : " + m_AccessTokenResponse.UserId;
            log += "\n    ScreenName : " + m_AccessTokenResponse.ScreenName;
            log += "\n    Token : " + m_AccessTokenResponse.Token;
            log += "\n    TokenSecret : " + m_AccessTokenResponse.TokenSecret;
            Debug.Log(log);
        }
    }

    /// <summary>
    /// リクエストトークンを受け取ったら呼び出されるコールバック.
    /// </summary>
    /// <param name="success">リクエストトークンを受け取れたかどうか</param>
    /// <param name="response">リクエストトークンのレスポンス</param>
    void OnRequestTokenCallback(bool success, Twitter.RequestTokenResponse response)
    {
        if (success)
        {
            string log = "OnRequestTokenCallback - succeeded";
            log += "\n    Token : " + response.Token;
            log += "\n    TokenSecret : " + response.TokenSecret;
            print(log);

            m_RequestTokenResponse = response;

            Twitter.API.OpenAuthorizationPage(response.Token);
        }
        else
        {
            print("OnRequestTokenCallback - failed.");
        }
    }

    /// <summary>
    /// アクセストークンを受けったら呼び出されるコールバック.
    /// </summary>
    /// <param name="success">アクセストークンを受け取れたかどうか</param>
    /// <param name="response">アクセストークンのレスポンス</param>
    void OnAccessTokenCallback(bool success, Twitter.AccessTokenResponse response)
    {
        if (success)
        {
            string log = "OnAccessTokenCallback - succeeded";
            log += "\n    UserId : " + response.UserId;
            log += "\n    ScreenName : " + response.ScreenName;
            log += "\n    Token : " + response.Token;
            log += "\n    TokenSecret : " + response.TokenSecret;
            Debug.Log(log);

            m_AccessTokenResponse = response;

			TwitterUserInfo twitterUserInfo = new TwitterUserInfo ();
			twitterUserInfo.userId = response.UserId;
			twitterUserInfo.screenName = response.ScreenName;
			twitterUserInfo.token = response.Token;
			twitterUserInfo.tokenSecret = response.TokenSecret;

			string json = JsonUtility.ToJson (twitterUserInfo);

			Utillity.SaveJsonFile (
				Utillity.resourcesPath + userInfoSavePath,
				twitterUserInfo.screenName,
				json
			);

			/*
            PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_ID, response.UserId);
            PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_SCREEN_NAME, response.ScreenName);
            PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_TOKEN, response.Token);
            PlayerPrefs.SetString(PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET, response.TokenSecret);
			*/

        }
        else
        {
            print("OnAccessTokenCallback - failed.");
        }

        if (OnRegisterAccessTokenEndEvent != null)
        {
            OnRegisterAccessTokenEndEvent(success);
        }

    }

    /// <summary>
    /// リクエストトークンを受け付ける.
    /// </summary>
    public void OnGetRequestToken()
    {
        StartCoroutine(Twitter.API.GetRequestToken(CONSUMER_KEY, CONSUMER_SECRET,
                                                           new Twitter.RequestTokenCallback(this.OnRequestTokenCallback)));
    }

    /// <summary>
    /// Pinの入力を受け付ける.
    /// </summary>
    /// <param name="m_Pin"></param>
    public void OnPinInput(string m_Pin, TwitterPluginManager.OnRegisterAccessTokenEnd callback)
    {
        if (string.IsNullOrEmpty(m_Pin) != true)
        {

            StartCoroutine(Twitter.API.GetAccessToken(CONSUMER_KEY, CONSUMER_SECRET, m_RequestTokenResponse.Token, m_Pin,
                               new Twitter.AccessTokenCallback(this.OnAccessTokenCallback)));
        }

    }

    #region statuses

    /// <summary>
    /// GET - 認証ユーザーのホーム・タイムライン(ツイート一覧)を取得する。
    /// </summary>
    /// <param name="callback">コールバック</param>
    /// <param name="parameter">パラメータ</param>
    public void OnGet_Home_TimeLine(Twitter.GetTweetCallback callback, Dictionary<string,string> parameter)
    {

        Debug.Log(parameter.Count);

        StartCoroutine(Twitter.API.GetHomeTimeLine(CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse, parameter,callback));
    }

    /// <summary>
    /// GET - 対象ユーザーのタイムライン(ツイート一覧)を取得する。
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="parameter"></param>
    public void OnGet_User_TimeLine(Twitter.GetTweetCallback callback, Dictionary<string, string> parameter)
    {
        StartCoroutine(Twitter.API.GetUserTimeLine(CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse, parameter,callback));
    }

    /// <summary>
    /// POST - ツイートを投稿する.
    /// リクエストに成功すると投稿したツイートのjsonが手に入る.
    /// </summary>
    /// <param name="text">投稿内容</param>
    /// <param name="callback">コールバック関数</param>
    /// <param name="parameter">パラメータ</param>
    public void OnPost_Update(string text, Twitter.PostTweetCallback callback, Dictionary<string, string> parameter)
    {
        StartCoroutine(Twitter.API.PostTweet(text, CONSUMER_KEY, CONSUMER_SECRET, m_AccessTokenResponse, parameter, callback));
    }

    #endregion
}