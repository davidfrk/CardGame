using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {

	public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                {
                    Debug.Log("Ad Finished");
                    break;
                }
            case ShowResult.Skipped:
                {
                    Debug.Log("Ad Skipped");
                    break;
                }
            case ShowResult.Failed:
                {
                    Debug.Log("Ad Failed");
                    break;
                }
        }
    }
}
