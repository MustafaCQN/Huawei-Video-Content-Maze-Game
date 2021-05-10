using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using DG.Tweening;

/*#if HMS_BUILD
using HmsPlugin;
#endif
using HuaweiMobileServices.Id;
using System;
using HuaweiMobileServices.Utils;
using HuaweiMobileServices.Game;
using HuaweiMobileServices.IAP;*/

public class MainMenu : SimpleMenu<MainMenu>
{

    [SerializeField]
    private GameObject huaweiButton;
  
    [SerializeField]
    private GameObject groupButton;

    [SerializeField]
    private GameObject removeAdsButton;

    private string removeAds = "com.huawei.removeads";//product name

   // List<ProductInfo> productInfoList = new List<ProductInfo>();
    List<string> productPurchasedList = new List<string>();

    void Start()
    {

        Debug.Log("StartStartStart for frame");


        /*  HMSAccountManager.Instance.OnSignInSuccess = OnLoginSuccess;
          HMSAccountManager.Instance.OnSignInFailed = OnSignInFailed;;*/
 
    }
 
        public void Login()
    {
#if GMS_BUILD
        if (gmsaccountManager != null)
        {
            gmsaccountManager.SignInWithGoogle();
            if (gmsaccountManager.IsSignedIn())
            {
                StartCoroutine(OnLoginCO());
            }
        }
        else
        {
            Debug.LogError("Account Manager is null");
        }
#endif
#if HMS_BUILD
        
        HMSAccountManager.Instance.SignIn();
 
#endif
    }
    IEnumerator OnLoginCO()
    {
        Debug.Log("Waiting for frame");
        yield return new WaitForEndOfFrame();
        //yield return new WaitForSeconds(5);
        //yield return new WaitForSecondsRealtime(1);
 
    }
 
    public void OnPlayClick()
    {
        MainMenu.Hide();

        /*    if (Time.timeSinceLevelLoad < Const.GAME_WAIT_TIME) return;
            GameMenu.Show(HMSAccountManager.Instance.HuaweiId.DisplayName);*/
        
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
        #if HMS_BUILD
    
         HMSLeaderboardManager.Instance.SetUserScoreShownOnLeaderboards(1);
         HMSLeaderboardManager.Instance.ShowLeaderboards();
       
        #endif
    }

    public void OnAchievementsClick()
    {
        #if HMS_BUILD

        HMSAchievementsManager.Instance.ShowAchievements();

        #endif
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
