using UnityEngine;

namespace WhisperingCrashDM
{
    public class EditorDebugInput : MonoBehaviour
    {
        [SerializeField] PanelVisibilityController visibility;
        [SerializeField] PanelPlacementController placement;
        public void Configure(PanelVisibilityController v, PanelPlacementController p) { visibility = v; placement = p; }
        void Update()
        {
            for (int i = 0; i < 6; i++) if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i))) visibility?.TogglePanel(i);
            if (Input.GetKeyDown(KeyCode.R)) placement?.ResetPanelPositions();
        }
    }
}
