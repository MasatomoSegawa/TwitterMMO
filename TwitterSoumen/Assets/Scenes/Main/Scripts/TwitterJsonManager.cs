using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using SimpleJSON;
using UnityEditor;

/// <summary>
/// APIから取ってきたツイートをTweetObjectに変換するクラス.
/// </summary>
public class TwitterJsonManager : Singleton<TwitterJsonManager> {

    private JSONNode json;

    public delegate void CreateTweetObjectCallBack(TweetObject[] tweetSoumens);

    public void CreateTweetSoumen(string data, CreateTweetObjectCallBack callback)
    {
        StartCoroutine(Task(data,callback));
    }

    public IEnumerator Task(string data, CreateTweetObjectCallBack callback)
    {

        if (String.IsNullOrEmpty(data) != false)
        {
            Debug.Log("Data is null or empty.");
            yield return null;
        }
        else
        {

            // jsonとして保存する.
            string name = SaveJson(data);

            // jsonをリードする.
            StartCoroutine(ReadJson(name,callback));
        }

    }

    /*
	 * jsonファイルを読み込む. 
	 */
    IEnumerator ReadJson(string jsonFileName, CreateTweetObjectCallBack callback)
    {

        string filePath = "TimeLine/" + jsonFileName;



        // リソースの非同期読込開始
        ResourceRequest resReq = Resources.LoadAsync<TextAsset>(filePath);
        // 終わるまで待つ
        while (resReq.isDone == false)
        {
            Debug.Log("Loading progress:" + resReq.progress.ToString());
            yield return 0;
        }

        json = JSONNode.Parse((resReq.asset as TextAsset).text);

        TweetObject[] tweetSoumens = new TweetObject[json.Count];
        for (var i = 0; i < json.Count; i++)
        {

            // 領域確保.
            tweetSoumens[i] = new TweetObject();

            tweetSoumens[i].tweetText = json[i]["text"];
            tweetSoumens[i].screenName = json[i]["user"]["screen_name"];

        }

        callback(tweetSoumens);

    }

    public static TweetObject[] ReadJson(string data)
    {

        JSONNode json = JSONNode.Parse(data);

        TweetObject[] tweetObjects = new TweetObject[json.Count];
        for (var i = 0; i < json.Count; i++)
        {

            // 領域確保.
            tweetObjects[i] = new TweetObject();

            tweetObjects[i].tweetText = json[i]["text"];
            tweetObjects[i].screenName = json[i]["user"]["screen_name"];

        }

        return tweetObjects;

    }

    /// <summary>
    /// データをjsonとして保存する.
    /// </summary>
    /// <param name="data"></param>
    string SaveJson(string data)
    {
        string SaveName = "myHomeTimeLine";
        StreamWriter sw;
        FileInfo fi;
        fi = new FileInfo(Application.dataPath + "/Scenes/Main/Resources/TimeLine/" + "myHomeTimeLine" + ".json");
        sw = fi.AppendText();
        sw.WriteLine(data);
        sw.Flush();
        sw.Close();
        AssetDatabase.Refresh();
        
        return SaveName;
    }

}
