using UnityEngine;

namespace WhisperingCrashDM
{
    [CreateAssetMenu(menuName = "Whispering Crash/Sound Cue", fileName = "SoundCueData")]
    public class SoundCueData : ScriptableObject
    {
        public string cueId;
        public string displayName;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 0.8f;
        public bool loop;
        public string category = "General";
    }
}
