using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleLogic : MonoBehaviour {

    // PinCode入力用のInputField.
    public InputField pinCodeInputField;

    void Start()
    {
        TwitterPluginManager.Instance.OnRegisterAccessTokenEndEvent += OnAccessTokenEnd;
    }

    /// <summary>
    /// RegisterTwitterButtonが押された時に呼び出される.
    /// </summary>
    public void OnRegisterTwitterButton()
    {
        TwitterPluginManager.Instance.OnGetRequestToken();
    }

    /// <summary>
    /// PINが入力完了した際に呼び出される.
    /// </summary>
    public void OnPinInputButton()
    {

        string pinCode = pinCodeInputField.text;
        TwitterPluginManager.Instance.OnPinInput(pinCode,OnAccessTokenEnd);

    }

    public void OnAccessTokenEnd(bool success)
    {

        FadeManager.Instance.LoadLevel("Main", 1.0f);
        Debug.Log("EndAccessToken");
    }

}
