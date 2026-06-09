using System;

namespace WhisperingCrashDM
{
    [Serializable]
    public class DCEntry
    {
        public string label;
        public int dc = 10;
        public string skillOrSave;
        public string successText;
        public string failureText;
    }
}
