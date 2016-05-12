using UnityEngine;
using System.Collections;

public delegate void OnUserGetKeyDown();

public class UserControll : MonoBehaviour {

	public  UnityEngine.Events.UnityEvent enterKeyInput;

	// Update is called once per frame
	void Update () {
	
		KeyInput ();


	}
		
	/// <summary>
	/// チャットフィールドを選択する.
	/// </summary>
	private void SelectChatField(){



	}

	/// <summary>
	/// キーのインプット入力を受け取る.
	/// </summary>
	private void KeyInput(){
	
		// Enterキーが押されたら.
		if (Input.GetKeyDown (KeyCode.Return)) {
			enterKeyInput.Invoke();
		}


	}
		
}
