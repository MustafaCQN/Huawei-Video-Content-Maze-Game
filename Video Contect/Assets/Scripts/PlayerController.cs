using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private float speedRotate = 50f;
    [SerializeField]
    private float speedJump = 50f;
    private float currentDegree = 0;
    [SerializeField]
    private Transform groundCheck;
    private bool isJumping;
    private bool isGameOver;
    private bool isStarted;

    private float animTime = 0.2f;
    [SerializeField]
    public Vector3 originalPos = new Vector3(0, -3, 4.371f);
    [SerializeField]
    private Transform transformParent;
    [SerializeField]
    private Transform sphere;
    [SerializeField]
    private Transform lastGoodCube;
    [SerializeField]
    private GameObject shadow;
    [SerializeField]
    private MeshRenderer sphereRenderer;

    private void Awake()
    {
        if (instance != null && instance != this)
            ;//Destroy(gameObject);
        else
        {
            instance = this;
            Init();
        }
    }

    private void Init()
    {
        //OnGameEnded();
        /*groundCheck = transform.Find("GroundCheck");
        currentDegree = 0;
        isJumping = false;
        isGameOver = false;
        isStarted = false;*/
    }

    private void OnEnable()
    {
        InputManager.OnTouchLeft += MoveLeft;
        InputManager.OnTouchRight += MoveRight;
        MazeGameManager.OnGameStarted += OnGameStarted;
        MazeGameManager.OnGameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        InputManager.OnTouchLeft -= MoveLeft;
        InputManager.OnTouchRight -= MoveRight;
        MazeGameManager.OnGameStarted -= OnGameStarted;
        MazeGameManager.OnGameEnded -= OnGameEnded;
    }

    private void OnGameEnded()
    {
        isJumping = false;
        isGameOver = true;
        StopAllCoroutines();
        GameOverMenu.Show();
    }

    private void OnGameStarted()
    {
        isStarted = true;
    }

    public void RestartPlayerFromContinue()
    {
        isJumping = false;
        isGameOver = false;
        isStarted = false;

        transform.localPosition = originalPos;

        var pos = transformParent.position;
        pos.z = lastGoodCube.position.z - 4.69f - 0.3f;

        transformParent.position = pos;

        transformParent.rotation = lastGoodCube.rotation;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        sphere.localPosition = new Vector3(0, -0.24f, 0);
        StartCoroutine(RestartPlayerCO());
    }

    IEnumerator RestartPlayerCO()
    {
        yield return new WaitForSeconds(2);
        MazeGameManager.Instance.OnGameStart();
    }

    private void Update()
    {
        transform.localPosition = originalPos;
        if (!isStarted) return;
        if (!isGrounded() && !isJumping && !isGameOver)
        {
            isGameOver = true;
            StartCoroutine(DoAnimGameOver());
            return;
        }
    }

    private bool PlayerCanJump()
    {
        if (!isStarted) return false;
        if (!isGrounded() && !isJumping && !isGameOver) return false;
        if (isJumping || isGameOver) return false;
        return true;
    }

    private void MoveRight()
    {
        if (PlayerCanJump())
            DoMove(1);
    }

    private void MoveLeft()
    {
        if (PlayerCanJump())
            DoMove(-1);
    }

    private void DoMove(int v)
    {
        isJumping = true;
        currentDegree += v * 45f;
        StartCoroutine(RotateTo(v));
        StartCoroutine(DoJump());
    }

    private IEnumerator DoJump()
    {
        isJumping = true;
        shadow.SetActive(false);
        float startPosY = -0.24f;
        float finalPosY = -0.24f + 1f;
        float timer = 0;
        float timeJump = animTime / 2f;
        while (timer <= timeJump)
        {
            timer += Time.deltaTime;
            float yPosTemp = Mathf.Lerp(startPosY, finalPosY, timer / timeJump);
            var s = sphere.localPosition;
            s.y = yPosTemp;
            sphere.localPosition = s;

            yield return null;
        }
        timer = 0;
        while (timer <= timeJump)
        {
            timer += Time.deltaTime;
            float yPosTemp = Mathf.Lerp(finalPosY, startPosY, timer / timeJump);
            var s = sphere.localPosition;
            s.y = yPosTemp;
            sphere.localPosition = s;

            yield return null;
        }
    }

    private IEnumerator RotateTo(float direction)
    {
        isJumping = true;
        shadow.SetActive(false);

        float originalRotation = transformParent.eulerAngles.z;

        float finalRotation = originalRotation + direction * 45f;

        float timer = 0;

        while (timer <= animTime)
        {
            timer += Time.deltaTime;
            float rotTemp = Mathf.Lerp(originalRotation, finalRotation, timer / animTime);
            transformParent.eulerAngles = Vector3.forward * rotTemp;
            yield return null;
        }

        if (isGrounded(true))
        {
           
        }
        isJumping = false;
        yield return null;
        shadow.SetActive(true);
    }

    private IEnumerator DoAnimGameOver()
    {
        isJumping = false;

        isGameOver = true;

        float startPosY = -0.24f;
        float finalPosY = -0.24f - 5;
        float timer = 0;
        float timeJump = animTime;

        while (timer <= timeJump)
        {
            timer += Time.deltaTime;
            float yPosTemp = Mathf.Lerp(startPosY, finalPosY, timer / timeJump);
            var s = sphere.localPosition;
            s.y = yPosTemp;
            sphere.localPosition = s;

            yield return null;
        }
        MazeGameManager.Instance.OnGameEnd();
    }

    private bool isGrounded()
    {
        return isGrounded(true);
    }

    RaycastHit hit;
    private bool isGrounded(bool savePos)
    {
        if (isJumping && !savePos)
            return true;

        Vector3 down = transform.TransformDirection(Vector3.down);

        if (Physics.Raycast(groundCheck.position, down, out hit, 10))
        {
            if (savePos)
            {
                if (!isJumping)
                {
                    var t = hit.transform;

                    var cube = t.GetComponentInParent<Cube>();
                    if (cube != null)
                        lastGoodCube = cube.cube;
                }
                return true;
            }
        }
        return false;
    }

    public void ChangeSpeed(float newSpeed)
    {
        GetComponentInParent<MoveForward>().sensitivity = newSpeed;
    }
}
