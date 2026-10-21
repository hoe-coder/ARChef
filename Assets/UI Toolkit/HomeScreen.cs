using UnityEngine;
using UnityEngine.UIElements;
using Vuforia;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(UIDocument))]
public class HomeScreenVuforiaStarter : MonoBehaviour
{
    [SerializeField] private VuforiaBehaviour vuforiaBehaviour;
    [SerializeField] private Sprite buttonIcon;

    private VisualElement _homeContainer;
    private Button _startButton;

    private void Awake()
    {
        if (vuforiaBehaviour == null)
            vuforiaBehaviour = FindFirstObjectByType<VuforiaBehaviour>(FindObjectsInactive.Include);

        if (vuforiaBehaviour != null)
            vuforiaBehaviour.enabled = false;
    }

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _homeContainer = root.Q<VisualElement>("home-container") ?? root;
        _startButton = root.Q<Button>("start-button");

        if (_startButton != null)
        {
            if (buttonIcon != null)
                _startButton.iconImage = Background.FromSprite(buttonIcon);

            _startButton.clicked += OnStartButtonClicked;
        }
    }

    private void OnDisable()
    {
        if (_startButton != null)
            _startButton.clicked -= OnStartButtonClicked;
    }

    private void OnStartButtonClicked()
    {
#if UNITY_EDITOR
        // Simple native alert popup in Unity Play Mode
        bool allowed = EditorUtility.DisplayDialog(
            "Camera Permission",
            "Allow camera access to start AR?",
            "Allow",
            "Deny"
        );

        if (!allowed) return;
#endif

        if (!VuforiaApplication.Instance.IsInitialized)
            VuforiaApplication.Instance.Initialize();

        if (vuforiaBehaviour != null)
        {
            vuforiaBehaviour.gameObject.SetActive(true);
            vuforiaBehaviour.enabled = true;
        }

        _homeContainer.style.display = DisplayStyle.None;
    }
}