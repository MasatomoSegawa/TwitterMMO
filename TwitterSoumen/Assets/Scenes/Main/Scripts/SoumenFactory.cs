using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoumenFactory : MonoBehaviour {

    // そうめん.
    public GameObject soumenPrefab;

    public Transform CreatePosition;

    public List<GameObject> CreateSoumen(TweetObject[] tweetSoumens)
    {

        // そうめんを優しく受け入れるざる(リスト)を生成.
        List<GameObject> zaru = new List<GameObject>();

        foreach (TweetObject tweetSoumen in tweetSoumens)
        {
            // そうめんの生成.
            GameObject soumen = Instantiate(soumenPrefab) as GameObject;

            // そうめんのプロパティを代入.
            TweetObject soumenScript = soumen.GetComponent<TweetObject>();
            
            soumenScript.tweetText = tweetSoumen.tweetText;
            soumenScript.screenName = tweetSoumen.screenName;

            zaru.Add(soumen);
        }

        return zaru;
    }

}
