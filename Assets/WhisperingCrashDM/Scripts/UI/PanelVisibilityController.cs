using UnityEngine;

namespace WhisperingCrashDM
{
    public class PanelVisibilityController : MonoBehaviour
    {
        [SerializeField] GameObject[] panels;
        public GameObject[] Panels => panels;
        public void SetPanels(GameObject[] value) => panels = value;
        public void TogglePanel(int index) { if (panels != null && index >= 0 && index < panels.Length && panels[index] != null) panels[index].SetActive(!panels[index].activeSelf); }
    }
}
