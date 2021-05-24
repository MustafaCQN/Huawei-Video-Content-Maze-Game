
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverMenu : SimpleMenu<GameOverMenu>
{
    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    [SerializeField]
    private GameObject continueButton;

    private int continueClickCount;


    private bool rewarded = false;

    private void Start()
    {
        transform.SetAsFirstSibling();
 
    }

  

    private void OnEnable()
    {
        //bestScoreText.text = $"Best Score: {PlayerPrefs.GetInt(Const.PREF_BEST_SCORE)}";
        continueButton.SetActive(continueClickCount < 1);
    }

    public void OnContinueButtonClick()
    {
        continueClickCount++;

#if GMS_BUILD
        GoogleAdMobController.Instance.ShowRewardedAd();
        Hide();
#endif

#if HMS_BUILD
        HMSAdsKitManager.Instance.ShowRewardedAd();
#endif
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("Pause : " + pause);
        if (!pause)
        {
            if (rewarded)
            {
                Debug.Log("Rewarding user");
                //PlayerController.Instance.RestartPlayerFromContinue();
                Hide();
                rewarded = false;
            }
        }
    }

    public void OnTryAgainClick()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
