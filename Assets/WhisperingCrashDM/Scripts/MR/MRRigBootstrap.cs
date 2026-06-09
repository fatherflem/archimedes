using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WhisperingCrashDM
{
    public class MRRigBootstrap : MonoBehaviour
    {
        [SerializeField] Transform cameraTransform;
        [SerializeField] Vector2 defaultPanelSize = new Vector2(760, 620);

        void Awake()
        {
            EnsureCamera(); EnsureEventSystem(); BuildCockpit();
        }

        void EnsureCamera()
        {
            if (Camera.main != null) { cameraTransform = Camera.main.transform; return; }
            var cam = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener), typeof(PassthroughSetupHelper));
            cam.tag = "MainCamera"; cam.transform.position = new Vector3(0, 1.55f, 0); cam.transform.rotation = Quaternion.identity;
            cam.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor; cam.GetComponent<Camera>().backgroundColor = new Color(.02f,.015f,.025f); cam.GetComponent<Camera>().nearClipPlane = .05f;
            cameraTransform = cam.transform;
        }

        void EnsureEventSystem()
        {
            if (FindObjectOfType<EventSystem>() != null) return;
            var ev = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            DontDestroyOnLoad(ev);
        }

        void BuildCockpit()
        {
            if (FindObjectOfType<AdventureManager>() != null) return;
            var root = new GameObject("DMRoot");
            var manager = root.AddComponent<AdventureManager>();
            var dice = root.AddComponent<DiceRoller>();
            var cues = root.AddComponent<SoundCueManager>();
            root.AddComponent<EditorWorldLockingService>();
            var visibility = root.AddComponent<PanelVisibilityController>();
            var placement = root.AddComponent<PanelPlacementController>();
            var debug = root.AddComponent<EditorDebugInput>();

            var panels = new[]
            {
                CreatePanel("Castle Systems", typeof(SceneSelectorPanel), p => p.GetComponent<SceneSelectorPanel>().Build(manager)),
                CreatePanel("Current Chamber", typeof(CurrentScenePanel), p => p.GetComponent<CurrentScenePanel>().Build()),
                CreatePanel("Xylos State", typeof(TrackerPanel), p => p.GetComponent<TrackerPanel>().Build()),
                CreatePanel("Fate Dice", typeof(DicePanel), p => p.GetComponent<DicePanel>().Build(dice)),
                CreatePanel("Echoes & Cues", typeof(SoundboardPanel), p => p.GetComponent<SoundboardPanel>().Build(cues)),
                CreatePanel("God Machine Forecast", typeof(FinalBattlePanel), p => p.GetComponent<FinalBattlePanel>().Build())
            };
            visibility.SetPanels(panels); placement.Configure(cameraTransform, panels); debug.Configure(visibility, placement);
        }

        GameObject CreatePanel(string title, System.Type panelComponent, System.Action<GameObject> build)
        {
            var panel = new GameObject(title, typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(Image));
            var rt = panel.GetComponent<RectTransform>(); rt.sizeDelta = defaultPanelSize;
            var canvas = panel.GetComponent<Canvas>(); canvas.renderMode = RenderMode.WorldSpace; canvas.worldCamera = cameraTransform.GetComponent<Camera>(); canvas.sortingOrder = 5;
            panel.transform.localScale = Vector3.one * 0.0017f;
            panel.GetComponent<Image>().color = new Color(.025f,.018f,.04f,.86f);
            var scaler = panel.GetComponent<CanvasScaler>(); scaler.dynamicPixelsPerUnit = 12; scaler.referencePixelsPerUnit = 100;
            var layout = UIFactoryHelpers.Vertical(panel.transform,"Panel Layout",10); UIFactoryHelpers.Stretch(layout.GetComponent<RectTransform>());
            var titleText = UIFactoryHelpers.Text(layout.transform,"Title",title,32,FontStyle.Bold); titleText.color = new Color(.76f,1f,.74f);
            var body = new GameObject("Body", typeof(RectTransform)); body.transform.SetParent(layout.transform,false); var bodyRt = body.GetComponent<RectTransform>(); bodyRt.sizeDelta = new Vector2(0, defaultPanelSize.y - 85); body.AddComponent<LayoutElement>().preferredHeight = defaultPanelSize.y - 85;
            body.AddComponent(panelComponent);
            build(body);
            return panel;
        }
    }
}
