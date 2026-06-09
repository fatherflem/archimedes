using UnityEngine;

namespace WhisperingCrashDM
{
    public class SoundboardPanel : MonoBehaviour
    {
        Transform content; SoundCueManager manager;
        public void Build(SoundCueManager cueManager)
        {
            manager = cueManager; content = UIFactoryHelpers.Scroll(transform,"Sound Scroll").content;
            foreach (var cue in manager.SoundCues) UIFactoryHelpers.Button(content,cue.displayName,()=>manager.PlayCue(cue.cueId));
            UIFactoryHelpers.Button(content,"Stop Current Cue",()=>manager.StopCurrentCue(), new Color(.18f,.18f,.18f,.95f));
            UIFactoryHelpers.Button(content,"Stop All Cues",()=>manager.StopAllCues(), new Color(.35f,.08f,.08f,.95f));
            UIFactoryHelpers.Text(content,"Hint","Unassigned placeholder cues report a friendly notice instead of throwing errors.",18);
        }
    }
}
