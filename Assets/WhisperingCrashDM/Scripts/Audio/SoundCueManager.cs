using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WhisperingCrashDM
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundCueManager : MonoBehaviour
    {
        [SerializeField] List<SoundCueData> soundCues = new List<SoundCueData>();
        AudioSource oneShotSource;
        readonly List<AudioSource> loopingSources = new List<AudioSource>();
        public IReadOnlyList<SoundCueData> SoundCues => soundCues;

        void Awake()
        {
            oneShotSource = GetComponent<AudioSource>();
            var resources = Resources.LoadAll<SoundCueData>("WhisperingCrashDM/SoundCues");
            foreach (var cue in resources) if (cue != null && !soundCues.Contains(cue)) soundCues.Add(cue);
            if (soundCues.Count == 0) soundCues.AddRange(SampleAdventureFactory.CreateSoundCues());
        }

        public void PlayCue(string cueId)
        {
            var cue = soundCues.FirstOrDefault(c => c.cueId == cueId || c.displayName == cueId);
            if (cue == null) { AdventureEvents.RaiseNotice("Sound cue not found: " + cueId); return; }
            if (cue.clip == null) { AdventureEvents.RaiseNotice("No clip assigned for " + cue.displayName + "."); return; }
            if (cue.loop)
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.clip = cue.clip; source.volume = cue.volume; source.loop = true; source.Play(); loopingSources.Add(source);
            }
            else oneShotSource.PlayOneShot(cue.clip, cue.volume);
            AdventureEvents.RaiseNotice("Playing cue: " + cue.displayName);
        }

        public void StopCurrentCue()
        {
            if (loopingSources.Count > 0)
            {
                var source = loopingSources[loopingSources.Count - 1];
                loopingSources.RemoveAt(loopingSources.Count - 1);
                if (source != null) Destroy(source);
            }
            oneShotSource.Stop();
        }

        public void StopAllCues()
        {
            foreach (var source in loopingSources) if (source != null) Destroy(source);
            loopingSources.Clear(); oneShotSource.Stop();
            AdventureEvents.RaiseNotice("Stopped all sound cues.");
        }
    }
}
