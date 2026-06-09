using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace WhisperingCrashDM
{
    public class AdventureManager : MonoBehaviour
    {
        public static AdventureManager Instance { get; private set; }

        [SerializeField] List<AdventureSceneData> sceneAssets = new List<AdventureSceneData>();
        [SerializeField] bool loadResourcesScenes = true;
        [SerializeField] string saveFileName = "whispering_crash_state.json";

        public AdventureState State { get; private set; } = new AdventureState();
        public AdventureSceneData CurrentScene { get; private set; }
        public IReadOnlyList<AdventureSceneData> Scenes => sceneAssets;
        string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            LoadScenes();
            State.Clamp();
        }

        void Start()
        {
            if (CurrentScene == null && sceneAssets.Count > 0) SetCurrentScene(sceneAssets[0].sceneId);
            AdventureEvents.RaiseStateChanged(State);
        }

        void LoadScenes()
        {
            if (loadResourcesScenes)
            {
                var resources = Resources.LoadAll<AdventureSceneData>("WhisperingCrashDM/Scenes");
                foreach (var scene in resources)
                    if (scene != null && !sceneAssets.Contains(scene)) sceneAssets.Add(scene);
            }
            if (sceneAssets.Count == 0) sceneAssets.AddRange(SampleAdventureFactory.CreateScenes());
            sceneAssets = sceneAssets.Where(s => s != null && !string.IsNullOrWhiteSpace(s.sceneId))
                                     .GroupBy(s => s.sceneId).Select(g => g.First())
                                     .OrderBy(s => s.category).ThenBy(s => s.displayName).ToList();
        }

        public void SetCurrentScene(string sceneId)
        {
            var next = sceneAssets.FirstOrDefault(s => s.sceneId == sceneId);
            if (next == null) { AdventureEvents.RaiseNotice("Scene not found: " + sceneId); return; }
            CurrentScene = next;
            if (!State.completedSceneIds.Contains(sceneId)) State.completedSceneIds.Add(sceneId);
            State.adventureLog.Add("Entered chamber: " + next.displayName);
            AdventureEvents.RaiseCurrentSceneChanged(CurrentScene);
            AdventureEvents.RaiseStateChanged(State);
        }

        public void ApplyChoice(SceneChoice choice)
        {
            if (choice == null) return;
            IncreaseInfection(choice.infectionDelta);
            AdjustCastleIntegrity(choice.castleIntegrityDelta);
            if (choice.choiceType == ChoiceType.TrustEcho) State.echoTrust++;
            if (choice.choiceType == ChoiceType.TrustMajel) State.majelTrust++;
            if (choice.choiceType == ChoiceType.Resist) State.psychicPressure = Mathf.Max(0, State.psychicPressure - 1);
            if (choice.choiceType == ChoiceType.Investigate || choice.choiceType == ChoiceType.Custom) State.psychicPressure++;
            State.Clamp();
            State.adventureLog.Add("Choice: " + choice.label + " — " + choice.consequenceText);
            AdventureEvents.RaiseNotice(choice.consequenceText);
            AdventureEvents.RaiseStateChanged(State);
        }

        public void PurifyElemental(ElementalType elemental)
        {
            var scene = FindElementalScene(elemental);
            if (!CanResolveElemental(elemental, scene)) return;
            State.SetPurified(elemental);
            IncreaseInfection(scene.infectionChangeOnPurify);
            AdjustCastleIntegrity(scene.castleIntegrityChangeOnPurify);
            AddModifier(scene.finalBattleModifierIfPurified);
            State.adventureLog.Add(elemental + " purified: " + scene.purificationOutcomeText);
            AdventureEvents.RaiseNotice(scene.purificationOutcomeText);
            AdventureEvents.RaiseStateChanged(State);
        }

        public void AbsorbElemental(ElementalType elemental)
        {
            var scene = FindElementalScene(elemental);
            if (!CanResolveElemental(elemental, scene)) return;
            State.SetAbsorbed(elemental);
            IncreaseInfection(scene.infectionChangeOnAbsorb);
            AdjustCastleIntegrity(scene.castleIntegrityChangeOnAbsorb);
            AddModifier(scene.finalBattleModifierIfAbsorbed);
            State.adventureLog.Add(elemental + " absorbed: " + scene.absorptionOutcomeText);
            AdventureEvents.RaiseNotice(scene.absorptionOutcomeText);
            AdventureEvents.RaiseStateChanged(State);
        }

        AdventureSceneData FindElementalScene(ElementalType elemental) => sceneAssets.FirstOrDefault(s => s.isElementalRoom && s.elementalType == elemental) ?? CurrentScene;

        bool CanResolveElemental(ElementalType elemental, AdventureSceneData scene)
        {
            if (elemental == ElementalType.None || scene == null) { AdventureEvents.RaiseNotice("No elemental chamber is active."); return false; }
            if (State.IsResolved(elemental)) { AdventureEvents.RaiseNotice(elemental + " has already been resolved."); return false; }
            return true;
        }

        void AddModifier(string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && !State.activeFinalBattleModifiers.Contains(text)) State.activeFinalBattleModifiers.Add(text);
        }

        public void IncreaseInfection(int amount) { State.infectionLevel += amount; State.Clamp(); }
        public void DecreaseInfection(int amount) { State.infectionLevel -= amount; State.Clamp(); }
        public void AdjustCastleIntegrity(int amount) { State.castleIntegrity += amount; State.Clamp(); }
        public string GenerateFinalBattleSummary() => FinalBattleSummaryGenerator.Generate(State);

        public void ResetAdventureState()
        {
            State = new AdventureState();
            State.adventureLog.Add("Adventure state reset.");
            AdventureEvents.RaiseStateChanged(State);
        }

        public void SaveState()
        {
            File.WriteAllText(SavePath, JsonUtility.ToJson(State, true));
            AdventureEvents.RaiseNotice("Saved DM state to " + SavePath);
        }

        public void LoadState()
        {
            if (!File.Exists(SavePath)) { AdventureEvents.RaiseNotice("No saved state found yet."); return; }
            State = JsonUtility.FromJson<AdventureState>(File.ReadAllText(SavePath)) ?? new AdventureState();
            State.Clamp();
            State.adventureLog.Add("Adventure state loaded.");
            AdventureEvents.RaiseStateChanged(State);
        }
    }
}
