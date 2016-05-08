using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainLogic : MonoBehaviour {

    public int count = 2;

    Dictionary<string, string> parameter;

	public InputField inputField;

    void Start()
    {

		parameter = new Dictionary<string, string>();

		parameter.Add("count", count.ToString());


		TwitterPluginManager.Instance.OnGetRequestToken ();
        
    }

	public void OnEndPin(bool isSucces){

		if (isSucces == true) {
			OnGetHomeTimeLineButton ();
		} else {
			Debug.Log ("失敗");
		}
	}

	public void OnEndEdit(){

		Debug.Log ("あれ〜？");

		TwitterPluginManager.Instance.OnPinInput (inputField.text, OnEndPin);

	}

    /// <summary>
    /// HomeTimeLineを取得するボタンを押された時に呼び出される.
    /// </summary>
    public void OnGetHomeTimeLineButton()
    {

        parameter.Clear();
        parameter.Add("count", count.ToString());

        TwitterPluginManager.Instance.OnGet_Home_TimeLine(OnHomeGetTimeLine,parameter);

    }

    /// <summary>
    /// ここにツイートそうめんが入ってくる.
    /// </summary>
    /// <param name="tweetSoumens"></param>
    void GetSoumenTweetObjects(TweetObject[] tweetSoumens)
    {

          
    }

    void OnHomeGetTimeLine(bool success, string data)
    {

        if (success == true)
        {
            Debug.Log("Success - GetTimeLine");
            Debug.Log(data);

            TwitterJsonManager.Instance.CreateTweetSoumen(data, GetSoumenTweetObjects);
        }
        else
        {
            Debug.Log("Failed - GetTimeLine");
        }

    }

}
