
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
    public const string PREF_BEST_SCORE = "BestScore";


    private bool rewarded = false;

    private void Start()
    {
       // transform.SetAsFirstSibling(); 
    }
     
    private void OnEnable()
    {
        bestScoreText.text = $"Best Score: {PlayerPrefs.GetInt(PREF_BEST_SCORE)}";
        MazeGameManager.runningGame = false;
        continueButton.SetActive(continueClickCount < 1);
    }

    public void OnContinueButtonClick()
    {
        continueClickCount++;
         
        HMSAdsKitManager.Instance.ShowRewardedAd();
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
