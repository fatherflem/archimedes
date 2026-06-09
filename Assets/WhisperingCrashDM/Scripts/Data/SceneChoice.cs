using System;

namespace WhisperingCrashDM
{
    [Serializable]
    public class SceneChoice
    {
        public string choiceId;
        public string label;
        public ChoiceType choiceType = ChoiceType.Custom;
        public string description;
        public int infectionDelta;
        public int castleIntegrityDelta;
        public string consequenceText;
    }
}
