using UnityEngine;

namespace WhisperingCrashDM
{
    public static class WhisperingCrashAutoBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void BootstrapAfterSceneLoad()
        {
            if (Object.FindObjectOfType<AdventureManager>() != null || Object.FindObjectOfType<MRRigBootstrap>() != null) return;
            new GameObject("WhisperingCrash_DM_Cockpit_Bootstrap", typeof(MRRigBootstrap));
        }
    }
}
