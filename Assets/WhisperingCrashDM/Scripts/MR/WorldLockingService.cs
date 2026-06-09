using UnityEngine;

namespace WhisperingCrashDM
{
    public class WorldLockingService : MonoBehaviour
    {
        public virtual void SavePanelLayout() => AdventureEvents.RaiseNotice("Panel layout save is stubbed for MVP; add Meta spatial anchors later.");
        public virtual void LoadPanelLayout() => AdventureEvents.RaiseNotice("Panel layout load is stubbed for MVP; add Meta spatial anchors later.");
        public virtual void ResetPanelLayout() { FindObjectOfType<PanelPlacementController>()?.ResetPanelPositions(); }
    }
}
