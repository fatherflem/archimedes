using UnityEngine;

namespace WhisperingCrashDM
{
    public class FinalBattlePanel : MonoBehaviour
    {
        Transform content;
        void OnEnable() { AdventureEvents.StateChanged += RefreshFromState; }
        void OnDisable() { AdventureEvents.StateChanged -= RefreshFromState; }
        public void Build() { content = UIFactoryHelpers.Scroll(transform,"Final Scroll").content; Refresh(false); }
        void RefreshFromState(AdventureState state) { Refresh(false); }
        public void Refresh(bool generate)
        {
            if (content == null || AdventureManager.Instance == null) return; var s = AdventureManager.Instance.State; UIFactoryHelpers.Clear(content);
            UIFactoryHelpers.Text(content,"Stats",$"Xylos Influence: {s.infectionLevel}/6\nPurified vs Absorbed: {s.elementalsPurified}/{s.elementalsAbsorbed}\nCastle Integrity: {s.castleIntegrity}/5",22,FontStyle.Bold);
            UIFactoryHelpers.Text(content,"Mods","Active Final Battle Modifiers",22,FontStyle.Bold);
            if (s.activeFinalBattleModifiers.Count == 0) UIFactoryHelpers.Text(content,"None","No modifiers accumulated yet.",18);
            foreach (var mod in s.activeFinalBattleModifiers) UIFactoryHelpers.Text(content,"Mod","• "+mod,18);
            UIFactoryHelpers.Button(content,"Generate Final Summary",()=>Refresh(true), new Color(.18f,.32f,.26f,.95f));
            if (generate) UIFactoryHelpers.Text(content,"Generated",AdventureManager.Instance.GenerateFinalBattleSummary(),20);
        }
    }
}
