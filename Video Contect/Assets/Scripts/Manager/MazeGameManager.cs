using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGameManager : MonoBehaviour
{
    private float oneTileMoveDistance = 98.0f;
    public Transform board;
    private List<GameObject> listPlatform = new List<GameObject>();

    public delegate void GameStart();
    public static event GameStart OnGameStarted;
    public delegate void GameEnd();
    public static event GameEnd OnGameEnded;

    [SerializeField] private Color[] colors;
 
    [SerializeField] private Material platformMaterial;
    [SerializeField] private Color currentPlatformColor;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private int platformCount = 30;

/*    private void Start()
    {
        InitializeBoard();
    }*/
    private static MazeGameManager instance;
    public static MazeGameManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            Init();
        }
    }
    IEnumerator Start()
    { 
       // platformMaterial.color = colors[0];
        Application.targetFrameRate = 60;
        GC.Collect();
     
        MainMenu.Show();
 
        /* StartCoroutine(CoroutSpawnCube());
         StartCoroutine(ChangeMaterialColor());*/
        yield return null;
    }
    public void Update()
    {
         

    }

    private void Init()
    {
        int count = 0;
        /* PlayerPrefs.SetInt(Const.PREF_SCORE, 0);
         if (!PlayerPrefs.HasKey(Const.PREF_BEST_SCORE))
             PlayerPrefs.SetInt(Const.PREF_BEST_SCORE, 0);*/

         InitializeBoard();

    }
    public void OnGameStart()
    {
        OnGameStarted?.Invoke();
    }
    public void OnGameEnd()
    {
        OnGameEnded?.Invoke();
    }

        private void Move()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
             
                transform.position = new Vector3(transform.position.x, transform
                    .position.y + oneTileMoveDistance, transform.position.z);
            
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
        }
    }

    private int getBallLocation()
    {
        return 0;
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
        /* while (true)
         {
             Color colorTemp = colors[UnityEngine.Random.Range(0, colors.Length)];
             platformMaterial.DOColor(colorTemp, Const.COLOR_TRANSITION_TIME);
             yield return new WaitForSeconds(Const.COLOR_CHANGE_TIME);
         }*/
    }

    // after move call this 
    private bool IsLevelCompleted()
    {
        //Board.tileArray
        foreach (Tile tile in Board.tileArray)
        {

            if(!tile.isWall && tile.color == Color.white)
            {
                return false;
            }            
        }
        return true;
    }
    private bool levelController(int level)
    { 
        return true;
    }
}
