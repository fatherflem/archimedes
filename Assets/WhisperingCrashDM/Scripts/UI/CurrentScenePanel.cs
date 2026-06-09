using UnityEngine;

namespace WhisperingCrashDM
{
    public class CurrentScenePanel : MonoBehaviour
    {
        Transform content;
        void OnEnable() { AdventureEvents.CurrentSceneChanged += Refresh; AdventureEvents.StateChanged += RefreshFromState; }
        void OnDisable() { AdventureEvents.CurrentSceneChanged -= Refresh; AdventureEvents.StateChanged -= RefreshFromState; }
        public void Build() { content = UIFactoryHelpers.Scroll(transform, "Scene Scroll").content; Refresh(AdventureManager.Instance?.CurrentScene); }
        void RefreshFromState(AdventureState state) { Refresh(AdventureManager.Instance?.CurrentScene); }
        void Refresh(AdventureSceneData scene)
        {
            if (content == null) return; UIFactoryHelpers.Clear(content);
            if (scene == null) { UIFactoryHelpers.Text(content,"Empty","No chamber selected."); return; }
            UIFactoryHelpers.Text(content,"Name",scene.displayName,30,FontStyle.Bold).color = new Color(1f,.86f,.55f);
            AddSection("Summary", scene.shortSummary); AddSection("Read Aloud", scene.readAloudText); AddSection("DM Notes", scene.dmNotes); AddSection("Secrets", scene.secrets);
            if (scene.dcEntries.Count > 0) { UIFactoryHelpers.Text(content,"DCs","DCs",24,FontStyle.Bold); foreach (var dc in scene.dcEntries) AddLine($"{dc.label}: DC {dc.dc} {dc.skillOrSave}\nSuccess: {dc.successText}\nFailure: {dc.failureText}"); }
            if (scene.monsterReferences.Count > 0) { UIFactoryHelpers.Text(content,"Monsters","Monster / Combat Notes",24,FontStyle.Bold); foreach (var m in scene.monsterReferences) AddLine($"{m.monsterName} ({m.role})\n{m.quickStats}\nTactics: {m.tactics}\nNotes: {m.specialNotes}"); }
            if (scene.choices.Count > 0) { UIFactoryHelpers.Text(content,"Choices","Scene Choices",24,FontStyle.Bold); foreach (var choice in scene.choices) UIFactoryHelpers.Button(content, choice.label, () => AdventureManager.Instance.ApplyChoice(choice)); }
            if (scene.isElementalRoom)
            {
                UIFactoryHelpers.Text(content,"Elemental","Elemental Resolution",24,FontStyle.Bold);
                UIFactoryHelpers.Button(content,"PURIFY",()=>AdventureManager.Instance.PurifyElemental(scene.elementalType), new Color(.72f,.55f,.18f,.95f));
                UIFactoryHelpers.Button(content,"ABSORB",()=>AdventureManager.Instance.AbsorbElemental(scene.elementalType), new Color(.42f,.04f,.08f,.95f));
                AddLine("Current status: " + AdventureManager.Instance.State.GetElementalStatus(scene.elementalType));
            }
            if (scene.suggestedSoundCueIds.Count > 0) AddSection("Suggested Cues", string.Join(", ", scene.suggestedSoundCueIds.ToArray()));
        }
        void AddSection(string title, string body) { if (string.IsNullOrWhiteSpace(body)) return; UIFactoryHelpers.Text(content,title,title,24,FontStyle.Bold); AddLine(body); }
        void AddLine(string body) { var t = UIFactoryHelpers.Text(content,"Text",body,20); t.alignment = TextAnchor.UpperLeft; }
    }
}
