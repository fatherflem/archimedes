using UnityEngine;

namespace WhisperingCrashDM
{
    public class TrackerPanel : MonoBehaviour
    {
        Transform content;
        void OnEnable() { AdventureEvents.StateChanged += Refresh; }
        void OnDisable() { AdventureEvents.StateChanged -= Refresh; }
        public void Build() { content = UIFactoryHelpers.Vertical(transform,"Tracker Content").transform; Refresh(AdventureManager.Instance?.State); }
        void Refresh(AdventureState s)
        {
            if (content == null || s == null) return; UIFactoryHelpers.Clear(content);
            UIFactoryHelpers.Text(content,"Stats",$"Xylos Influence: {s.infectionLevel}/6\nCastle Integrity: {s.castleIntegrity}/5\nEcho Trust: {s.echoTrust}/5\nMajel Trust: {s.majelTrust}/5\nPsychic Pressure: {s.psychicPressure}/6\nPurified: {s.elementalsPurified}/6\nAbsorbed: {s.elementalsAbsorbed}/6",22,FontStyle.Bold);
            foreach (var e in FinalBattleSummaryGenerator.Elementals) UIFactoryHelpers.Text(content,e.ToString(),$"{e}: {s.GetElementalStatus(e)}",20);
            UIFactoryHelpers.Button(content,"Save State",()=>AdventureManager.Instance.SaveState());
            UIFactoryHelpers.Button(content,"Load State",()=>AdventureManager.Instance.LoadState());
            UIFactoryHelpers.Button(content,"Reset State",()=>AdventureManager.Instance.ResetAdventureState(), new Color(.35f,.08f,.08f,.95f));
        }
    }
}
