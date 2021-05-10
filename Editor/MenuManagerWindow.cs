using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManagerWindow : EditorWindow
{
    static EditorWindow window;

    private SerializedObject so;

    private Vector2 scrollPos;

    [SerializeField]
    private MenuManagerSettingsVO menuSettings;

    [MenuItem("Window/Menu Manager/Settings")]
    public static void ShowWindow()
    {
        window = GetWindow(typeof(MenuManagerWindow));
        window.titleContent = new GUIContent("Menu Manager");
    }

    private void Awake()
    {
        so = new SerializedObject(this);
    }

    private void OnEnable()
    {
        menuSettings = new MenuManagerSettingsVO();
        LoadSettings();
        so = new SerializedObject(this);
    }

    private void LoadSettings()
    {
        string settingsDir = "Assets/Resources/MenuManager";
        if (AssetDatabase.IsValidFolder(settingsDir))
        {
            var settings = GetMenuManagerSettings();
            if (settings != null)
                menuSettings = MenuManagerSettingsVO.FromSetting(settings);
        }
        Repaint();
    }

    private MenuManagerSettings GetMenuManagerSettings()
    {
        return Resources.Load<MenuManagerSettings>("MenuManager/MenuSettings");
    }

    private void UpdateSettings()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        if (!AssetDatabase.IsValidFolder("Assets/Resources/MenuManager"))
            AssetDatabase.CreateFolder("Assets/Resources", "MenuManager");
        else
        {
            AssetDatabase.DeleteAsset("Assets/Resources/MenuManager/MenuSettings.asset");
        }
        var setting = GetMenuManagerSettings();

        var asset = ScriptableObject.CreateInstance<MenuManagerSettings>();
        asset.Menus = menuSettings.Menus;
        AssetDatabase.CreateAsset(asset, "Assets/Resources/MenuManager/MenuSettings.asset");
    }

    private void OnGUI()
    {
        DrawEditor();
    }

    private void DrawEditor()
    {
        EditorGUILayout.BeginVertical();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        EditorGUILayout.PropertyField(so.FindProperty("menuSettings"));

        if (GUILayout.Button("Update"))
            UpdateSettings();

        if (GUILayout.Button("Create Canvas"))
            CreateCanvasObjects();

        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();

        so.ApplyModifiedProperties();

        if (window != null)
            window.titleContent = new GUIContent("Menu Manager");
    }

    private void CreateCanvasObjects()
    {
        if (GameObject.FindObjectOfType<MenuManager>() == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvasObj.AddComponent<RectTransform>();
            var canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var canvasScaler = canvasObj.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1080, 1920);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0.5f;
            canvasScaler.referencePixelsPerUnit = 100;
            var graphicRaycaster = canvasObj.AddComponent<GraphicRaycaster>();
            graphicRaycaster.ignoreReversedGraphics = true;
            graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            canvasObj.AddComponent<CanvasRenderer>();

            var eventSystemObj = new GameObject("EventSystem");
            var eventSystem = eventSystemObj.AddComponent<EventSystem>();
            var inputModule = eventSystemObj.AddComponent<StandaloneInputModule>();

            GameObject safeAreaObj = new GameObject("SafeArea");
            var rectTransform = safeAreaObj.AddComponent<RectTransform>();
            rectTransform.SetParent(canvasObj.transform);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = Vector3.zero;
            safeAreaObj.AddComponent<SafeArea>();

            safeAreaObj.AddComponent<MenuManager>();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}

[System.Serializable]
public class MenuManagerSettingsVO
{
    public List<Menu> Menus;

    public static MenuManagerSettingsVO FromSetting(MenuManagerSettings settings)
    {
        var p = new MenuManagerSettingsVO();
        p.Menus = settings.Menus;
        return p;
    }
}