using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMenu : Menu<GameMenu>
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI nameText;

    private void Start()
    {
        MazeGameManager.Instance.OnGameStart();
    }

    public static void Show(string name)
    {
        Open();
        Instance.nameText.text = name;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
