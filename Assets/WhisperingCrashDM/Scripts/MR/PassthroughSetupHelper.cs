using System;
using UnityEngine;

namespace WhisperingCrashDM
{
    public class PassthroughSetupHelper : MonoBehaviour
    {
        [SerializeField] bool tryMetaPassthrough = true;
        void Awake()
        {
            if (!tryMetaPassthrough) return;
            var overlayType = Type.GetType("OVRPassthroughLayer, Assembly-CSharp") ?? Type.GetType("OVRPassthroughLayer, Oculus.VR");
            if (overlayType == null)
            {
                Debug.LogWarning("Meta passthrough classes were not found. The DM cockpit will run with a normal camera background; see Docs/SetupNotes.md.");
                return;
            }
            if (GetComponent(overlayType) == null) gameObject.AddComponent(overlayType);
        }
    }
}
