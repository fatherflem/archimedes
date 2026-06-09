using System;

namespace WhisperingCrashDM
{
    [Serializable]
    public class RollResult
    {
        public string label;
        public int die;
        public int roll;
        public int modifier;
        public int total;
        public string interpretation;
        public override string ToString() => string.IsNullOrEmpty(interpretation) ? $"{label}: d{die}({roll}) {(modifier >= 0 ? "+" : "")}{modifier} = {total}" : $"{label}: {total} — {interpretation}";
    }
}
