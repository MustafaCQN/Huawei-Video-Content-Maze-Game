using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HmsPlugin;
using HuaweiMobileServices.Utils;
using HuaweiMobileServices.Id;
using HuaweiMobileServices.IAP;
using UnityEngine.UI;

public class MazeGameManager : MonoBehaviour
{
    public float Speed = 15f;
    public Board board;
    private List<GameObject> listPlatform = new List<GameObject>();

    public delegate void GameStart();
    public static event GameStart OnGameStarted;
    public delegate void GameEnd();
    public static event GameEnd OnGameEnded;
    public Button MenuButton;
 

    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private TextMeshProUGUI textUserName;
    [SerializeField] private TextMeshProUGUI textTime;

    [SerializeField] private Color[] colors;
 
    [SerializeField] private Material platformMaterial;
    [SerializeField] private Color currentPlatformColor;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private int platformCount = 30;
    public Ball ball;
    private bool isDownKeyPressed, isUpKeyPressed, isLeftKeyPressed, isRightKeyPressed;
    public Tile CurrentTile;
    MaterialPropertyBlock mpb;

    public static event Action<SwipeDirection> Swipe;
    private bool swiping = false;
    private bool eventSent = false;
    private Vector2 lastPosition;
    public const string PREF_SCORE = "Score";
    public const string PREF_BEST_SCORE = "BestScore";

    private  float playTime = 0.0f; 
    public static bool runningGame = false;

    public enum SwipeDirection
    {
        Up,
        Down,
        Right,
        Left
    }
    private static MazeGameManager instance;
    public static MazeGameManager Instance
    {
        get { return instance; }
    }

    public bool isOwned { get;  set; }

    private void Awake()
    { 

        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            Init();
        }
        mpb = new MaterialPropertyBlock();
        mpb.SetColor("_Color", Color.red);
    }
    IEnumerator Start()
    {
        HMSGameManager.Instance.SignInSuccess = OnLoginSuccess;
        HMSGameManager.Instance.SignInFailure = OnSignInFailed;

        HMSIAPManager.Instance.OnObtainOwnedPurchasesSuccess = OwnedPurchases;
        HMSIAPManager.Instance.ObtainOwnedPurchases();

        // platformMaterial.color = colors[0];
        Application.targetFrameRate = 60;
        GC.Collect();
     
        MainMenu.Show();
 
        textMeshPro.text = "Level : 1";


        /* StartCoroutine(CoroutSpawnCube());
         StartCoroutine(ChangeMaterialColor());*/
        StartCoroutine(FlickerBomb());
        yield return null;
    }
    void Update()
    {
        if (runningGame)
        {
            playTime += Time.deltaTime;
            int seconds = (int)playTime % 60;
            textTime.text = "Time : " + seconds.ToString();
        } 
        UpdateCollidedTile();

        if (CurrentTile != null)
        {
            ChangeColor();
        }
        if (isUpKeyPressed)
        {
            CurrentTile = ball.collidedTile.GetComponent<Tile>();
            // bı sey
            if (!CurrentTile.is_Up_DirectionAvailable) { isUpKeyPressed = false; ball.transform.position = CurrentTile.transform.position; IsLevelCompleted(); return; }
            ball.gameObject.transform.position += new Vector3(0, Speed, 0);
            return;
        }
        if (isDownKeyPressed)
        {
            CurrentTile = ball.collidedTile.GetComponent<Tile>();
            // bı sey
            if (!CurrentTile.is_Down_DirectionAvailable) { isDownKeyPressed = false; ball.transform.position = CurrentTile.transform.position; IsLevelCompleted(); return; }
            ball.gameObject.transform.position += new Vector3(0, -Speed, 0);
            return;
        }
        if (isLeftKeyPressed)
        {
            CurrentTile = ball.collidedTile.GetComponent<Tile>();
            // bı sey
            if (!CurrentTile.is_Left_DirectionAvailable) { isLeftKeyPressed = false; ball.transform.position = CurrentTile.transform.position; IsLevelCompleted(); return; }
            ball.gameObject.transform.position += new Vector3(-Speed, 0, 0);
            return;
        }
        if (isRightKeyPressed)
        {
            CurrentTile = ball.collidedTile.GetComponent<Tile>();
            // bı sey
            if (!CurrentTile.is_Right_DirectionAvailable) { isRightKeyPressed = false; ball.transform.position = CurrentTile.transform.position; IsLevelCompleted(); return; }
            ball.gameObject.transform.position += new Vector3(Speed, 0, 0);
            return;
        }
   
        if (Input.touchCount == 0)
            return;
        var touch = Input.GetTouch(0);

        if (Input.GetTouch(0).deltaPosition.sqrMagnitude != 0)
        {
            if (swiping == false)
            {
                swiping = true;
                lastPosition = Input.GetTouch(0).position;
                return;
            }
            else
            {
                if (!eventSent)
                {
                    if (Swipe != null)
                    {
                        Vector2 direction = Input.GetTouch(0).position - lastPosition;

                        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                        {
                            if (direction.x > 0)
                                Swipe(SwipeDirection.Right);
                            else
                                Swipe(SwipeDirection.Left);
                        }
                        else
                        {
                            if (direction.y > 0)
                                Swipe(SwipeDirection.Up);
                            else
                                Swipe(SwipeDirection.Down);
                        }

                        eventSent = true;
                    }
                }
            }
        }
        else
        {
            swiping = false;
            eventSent = false;
        }
        Swipe = delegate (SwipeDirection direction)
        {
            if(direction == SwipeDirection.Up)
                isUpKeyPressed = true;
            else if (direction == SwipeDirection.Down)
                isDownKeyPressed = true;
            else if (direction == SwipeDirection.Right)
                isRightKeyPressed = true;
            else if (direction == SwipeDirection.Left)
                isLeftKeyPressed = true;
        };
    }
  

    private void UpdateCollidedTile()
    {
        foreach (Tile tile in board.tileArray)
        {
            if (Vector2.Distance(ball.transform.position, tile.transform.position) <= 15f)
            {
                ball.collidedTile = tile.gameObject;
                break;
            }
        }
    }

    IEnumerator FlickerBomb()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 81; i++)
            {
                if (board.tileArray[i].isBomb)
                {
                    if (board.tileArray[i].GetComponent<Image>().sprite == null)
                    {
                        board.tileArray[i].GetComponent<Image>().sprite = board.BombSprite;
                    }else
                    {
                        board.tileArray[i].GetComponent<Image>().sprite = null;
                    }
                }
            }
        }
    }

    private void ChangeColor()
    {
        CheckGameOver();
        CurrentTile.GetComponent<UnityEngine.UI.Image>().color = (mpb.GetColor("_Color"));
        IsLevelCompleted();
    }

    private void CheckGameOver()
    {
        if (CurrentTile.isBomb && CurrentTile.GetComponent<Image>().sprite != null)
        {
            Debug.Log("Game Over");
            OnGameEnd();
        }
    }

    private int score;
    public int Score
    {
        get
        {
            score = PlayerPrefs.GetInt(PREF_SCORE);
            return score;
        }
        set
        {
            score = value;
            if (GameMenu.Instance != null)
                GameMenu.Instance.UpdateScore(score);
            PlayerPrefs.SetInt(PREF_SCORE, score);
            int bestScore = PlayerPrefs.GetInt(PREF_BEST_SCORE);
            if (score > bestScore)
                PlayerPrefs.SetInt(PREF_BEST_SCORE, score);
        }
    }

    private void Init()
    {
        int count = 0;
        PlayerPrefs.SetInt(PREF_SCORE, 0);
        if (!PlayerPrefs.HasKey(PREF_BEST_SCORE))
            PlayerPrefs.SetInt(PREF_BEST_SCORE, 0);

        InitializeBoard();

    }
    public void OnGameStart()
    {
        runningGame = true;
        playTime = 0;
        OnGameStarted?.Invoke();
    }
    public void OnGameEnd()
    {
        runningGame = false;
        OnGameEnded?.Invoke();
    }
    private int getBallLocation()
    {
        return 0;
    }
    public void ShowMainMenu()
    {
        if (GameObject.Find("MainMenu") == null)
        {
            MainMenu.Show();
        }
    }
    public void InitializeBoard()
    { 
        board.GetComponent<Board>().InitializeTiles(BoardConstant.currentLevel);
    }
    public void ClearTileList()
    {
        board.GetComponent<Board>().ClearTileList();
    }
    private IEnumerator CoroutSpawnCube()
    { 
        yield return null;
       
    }
    IEnumerator ChangeMaterialColor()
    {
        yield return new WaitForSeconds(0.1f);
    }

    // after move call this 
    private bool IsLevelCompleted()
    {
        //Board.tileArray
        foreach (Tile tile in board.GetComponent<Board>().tileArray)
        {

            if(!tile.isWall && tile.GetComponent<UnityEngine.UI.Image>().color == Color.gray)
            {
                return false;
            }            
        }
        BoardConstant.currentLevel++;
        ClearTileList();
        InitializeBoard();
        ball.GetComponent<RectTransform>().localPosition = new Vector3(-365, 365, 0);//board.tileArray[0].transform.position;
     
        textMeshPro.text = "Level : " + (BoardConstant.currentLevel + 1);
        Score += 10;
        HMSLeaderboardManager.Instance.SubmitScore(HMSLeaderboardConstants.Scores, Score);

         
        HMSAchievementsManager.Instance.IncreaseStepAchievement(HMSAchievementConstants.Levels,1);

        if (!isOwned)
        {
            HMSAdsKitManager.Instance.ShowInterstitialAd();
        } 
        return true;
    }

    private void OwnedPurchases(OwnedPurchasesResult obj)
    {
        if (obj == null)
            return;
        isOwned = obj.ItemList.Contains(HMSIAPConstants.AdsRemove);
        
    }

    private bool levelController(int level)
    { 
        return true;
    }
    private void OnSignInFailed(HMSException obj)
    {
        Debug.Log(" OnSignInFailed ");
    }

    private void OnLoginSuccess(AuthAccount obj)
    {
        Debug.Log(" OnLoginSuccess ");
        textUserName.text = obj.DisplayName;

    }
}
