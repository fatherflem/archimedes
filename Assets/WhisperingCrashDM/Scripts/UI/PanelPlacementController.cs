using UnityEngine;

namespace WhisperingCrashDM
{
    public class PanelPlacementController : MonoBehaviour
    {
        [SerializeField] Transform cameraTransform;
        [SerializeField] GameObject[] panels;
        readonly Vector3[] offsets = { new Vector3(-1.35f, .1f, 2.2f), new Vector3(0f, .15f, 2.05f), new Vector3(1.25f, .45f, 2.2f), new Vector3(1.1f, -.65f, 1.8f), new Vector3(-1.1f, -.65f, 1.8f), new Vector3(2.05f, .1f, 2.6f) };
        public void Configure(Transform cam, GameObject[] panelObjects) { cameraTransform = cam; panels = panelObjects; ResetPanelPositions(); }
        void Start() { if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform; ResetPanelPositions(); }
        public void ResetPanelPositions()
        {
            if (cameraTransform == null || panels == null) return;
            for (int i = 0; i < panels.Length && i < offsets.Length; i++)
            {
                if (panels[i] == null) continue;
                panels[i].transform.position = cameraTransform.TransformPoint(offsets[i]);
                panels[i].transform.rotation = Quaternion.LookRotation(panels[i].transform.position - cameraTransform.position, Vector3.up);
            }
        }
        void LateUpdate()
        {
            if (cameraTransform == null || panels == null) return;
            foreach (var panel in panels) if (panel != null) panel.transform.rotation = Quaternion.LookRotation(panel.transform.position - cameraTransform.position, Vector3.up);
        }
    }
}
