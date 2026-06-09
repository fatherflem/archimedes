using System;

namespace WhisperingCrashDM
{
    public static class AdventureEvents
    {
        public static event Action<AdventureSceneData> CurrentSceneChanged;
        public static event Action<AdventureState> StateChanged;
        public static event Action<string> NoticeRaised;

        public static void RaiseCurrentSceneChanged(AdventureSceneData scene) => CurrentSceneChanged?.Invoke(scene);
        public static void RaiseStateChanged(AdventureState state) => StateChanged?.Invoke(state);
        public static void RaiseNotice(string message) => NoticeRaised?.Invoke(message);
    }
}
