using HmsPlugin;
using HuaweiMobileServices.IAP;
using HuaweiMobileServices.Id;
using HuaweiMobileServices.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : SimpleMenu<MainMenu>
{

    [SerializeField]
    private GameObject huaweiButton;
  
    [SerializeField]
    private GameObject groupButton;

    [SerializeField]
    private GameObject removeAdsButton;


    void Start()
    { 
        Debug.Log("Start for frame");
    }
  
    IEnumerator OnLoginCO()
    {
        Debug.Log("Waiting for frame");
        yield return new WaitForEndOfFrame();
        //yield return new WaitForSeconds(5);
        //yield return new WaitForSecondsRealtime(1);
 
    }
    public void OnNoAdsClick()
    {
        HMSIAPManager.Instance.OnBuyProductSuccess += OnBuyProductSuccess;

        HMSIAPManager.Instance.BuyProduct(HMSIAPConstants.AdsRemove);

        MainMenu.Hide(); 

    }

    private void OnBuyProductSuccess(PurchaseResultInfo result)
    {
        if (result.InAppPurchaseData.ProductId == HMSIAPConstants.AdsRemove)
        {
            // Write your remove ads logic here.
            Debug.Log(" OnBuyProductSuccess ");
            MazeGameManager.Instance.isOwned = true;
            HMSAdsKitManager.Instance.HideBannerAd();
        }
    }

    public void OnPlayClick()
    {
        MainMenu.Hide();
    }
    public void QuitGame()
    {
       Debug.Log("QuitGame");
       // Application.Quit();
       BoardConstant.currentLevel++;
       MazeGameManager.Instance.ClearTileList();
       MazeGameManager.Instance.InitializeBoard();
    }

    public void OnLeaderboardsClick()
    { 
         HMSLeaderboardManager.Instance.SetUserScoreShownOnLeaderboards(1);
         HMSLeaderboardManager.Instance.ShowLeaderboards();
    }

    public void OnAchievementsClick()
    { 
        HMSAchievementsManager.Instance.ShowAchievements();
    }
 

    private void ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
