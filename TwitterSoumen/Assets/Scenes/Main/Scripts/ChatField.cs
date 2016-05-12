using UnityEngine;
using System.Collections;

public class ChatField : MonoBehaviour {

	// 自分自身のキャンバスグループ.
	private CanvasGroup canvasGroup;

	// アクティブかどうか.
	private bool isActive;

	// Use this for initialization
	void Start () {
	
		canvasGroup = this.GetComponent<CanvasGroup> ();

		// アクティブにしない.
		isActive = false;

	}

	/// <summary>
	/// アクティブ/非アクティブを変える
	/// </summary>
	public void SwitchActive(){
		isActive = !isActive;

		if (isActive) {
			SetActive ();
		} else {
			SetUnActive ();
		}

	}

	/// <summary>
	/// アクティブにする.
	/// </summary>
	private void SetActive(){

		this.canvasGroup.interactable = true;
		this.canvasGroup.alpha = 1.0f;
		this.canvasGroup.blocksRaycasts = true;

	}

	/// <summary>
	/// 非アクティブにする.
	/// </summary>
	private void SetUnActive(){

		this.canvasGroup.interactable = false;
		this.canvasGroup.alpha = 0.0f;
		this.canvasGroup.blocksRaycasts = false;

	}


}
