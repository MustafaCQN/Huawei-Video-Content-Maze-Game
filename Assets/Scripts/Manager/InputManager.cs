using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void TouchLeft();
    public static event TouchLeft OnTouchLeft;

    public delegate void TouchRight();
    public static event TouchRight OnTouchRight;

    public delegate void TouchScreen();
    public static event TouchScreen OnTouchScreen;

    public bool blockInput = false;

    private static InputManager instance;
    public static InputManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void OnEnable()
    {
        MazeGameManager.OnGameStarted += OnGameStarted;
        MazeGameManager.OnGameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        MazeGameManager.OnGameStarted -= OnGameStarted;
        MazeGameManager.OnGameEnded -= OnGameEnded;
    }

    private void OnGameEnded()
    {
        blockInput = true;
    }

    private void OnGameStarted()
    {
        blockInput = false;
    }

    private void Update()
    {
        if (blockInput) return;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        int touchCount = Input.touchCount;
        if (touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x < Screen.width / 2f)
                    _OnTouchLeft();
                else
                    _OnTouchRight();
            }
        }

#endif

#if (!UNITY_ANDROID && !UNITY_IOS) || UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            _OnTouchLeft();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            _OnTouchRight();
#endif
    }

    private void _OnTouchRight()
    {
        OnTouchScreen?.Invoke();
        OnTouchRight?.Invoke();
    }

    private void _OnTouchLeft()
    {
        OnTouchScreen?.Invoke();
        OnTouchLeft?.Invoke();
    }
}
