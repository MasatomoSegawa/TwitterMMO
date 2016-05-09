using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// 暮らしに役立つメソッド/プロパティを実装してあるよ.
/// </summary>
public class Utillity  {

	/// <summary>
	/// Application.dataPath + "/Resources"を返すよ.
	/// </summary>
	/// <value>The resources path.</value>
	public static string resourcesPath{
		get{

			if (Application.isEditor == false) {
				return Application.persistentDataPath + "/Resources/";
			} else {
				return Application.dataPath + "/Resources/";
			}
		}
	}

	/// <summary>
	/// json形式で保存する.
	/// </summary>
	/// <param name="path">Path.</param>
	public static void SaveJsonFile(string path, string fileName, string jsonString){

		fileName += ".json";

		Debug.Log ("SaveFile: " + path + fileName);

		if (string.IsNullOrEmpty (jsonString) == true) {
			Debug.Log (path + fileName + " is null or empty");
		}

		if (Directory.Exists (path) == false) {
			Debug.Log ("(" + path + ")" + " Directory not found.");
			return;
		}

		StreamWriter sw = new StreamWriter (path + fileName, false);
		sw.WriteLine (jsonString);
		sw.Close ();

		UnityEditor.AssetDatabase.Refresh ();

	}

	/// <summary>
	/// urlにアクセス出来るかどうかを試す.(非同期のがよい？)
	/// </summary>
	/// <returns><c>true</c>, if test was connected, <c>false</c> otherwise.</returns>
	/// <param name="url">URL.</param>
	public static bool ConnectTest(string url){

		System.Net.HttpWebRequest webreq = null;
		System.Net.HttpWebResponse webres = null;
		try {
			//HttpWebRequestの作成
			webreq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
			//メソッドをHEADにする
			webreq.Method = "HEAD";
			//受信する
			webres = (System.Net.HttpWebResponse)webreq.GetResponse();
			//応答ステータスコードを表示
			//Debug.Log(webres.StatusCode);
			return true;
		} catch {
			return false;
		} finally {
			if (webres != null)
				webres.Close();
		}


	}

	/// <summary>
	/// そのディレクトリにファイルああるかどうかしらべる.
	/// </summary>
	/// <returns><c>true</c>, if exist file was ised, <c>false</c> otherwise.</returns>
	/// <param name="path">Path.</param>
	public static bool isExistFile(string path){
	
		string[] strs = Directory.GetFiles(path);

		if (strs.Length == 0) {
			return false;
		}

		return true;

	}
		
}
