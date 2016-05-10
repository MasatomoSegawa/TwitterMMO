using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TitleLogic : MonoBehaviour {

	public InputField inputField;

    void Start()
    {
		// ログインした事があるかどうか.
		if (Utillity.isExistFile (Utillity.resourcesPath + "/UserInfo/") == true) {		

			// ロード.
			LoadTwitterData ();


		} else {

			// ないなら登録ぅ.
			TwitterPluginManager.Instance.OnGetRequestToken ();
		}
			        
	}
		
	/// <summary>
	/// Jsonからユーザー情報をロードする.
	/// </summary>
	private void LoadTwitterData(){

		TextAsset ta = Resources.Load("UserInfo/shibainu_bot114") as TextAsset; 

		TwitterUserInfo tUI = JsonUtility.FromJson<TwitterUserInfo> (ta.text);

		TwitterPluginManager.Instance.LoadTwitterUserInfo (tUI);

		FadeManager.Instance.LoadLevel ("Main", 1.0f);

	}

	public void OnEndPin(bool isSucces){

		if (isSucces == false) {
			Debug.Log ("失敗");
		}
	}

	public void OnEndEdit(){
	
		TwitterPluginManager.Instance.OnPinInput (inputField.text, OnEndPin);

	}
		
    void OnHomeGetTimeLine(bool success, string data)
    {

        if (success == true)
        {
            Debug.Log("Success - GetTimeLine");
            Debug.Log(data);

		}
        else
        {
            Debug.Log("Failed - GetTimeLine");
        }

    }

}
