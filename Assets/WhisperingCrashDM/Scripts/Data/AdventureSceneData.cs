using System.Collections.Generic;
using UnityEngine;

namespace WhisperingCrashDM
{
    [CreateAssetMenu(menuName = "Whispering Crash/Adventure Scene", fileName = "AdventureSceneData")]
    public class AdventureSceneData : ScriptableObject
    {
        public string sceneId;
        public string displayName;
        public SceneCategory category;
        [TextArea(2, 4)] public string shortSummary;
        [TextArea(3, 8)] public string readAloudText;
        [TextArea(3, 8)] public string dmNotes;
        [TextArea(2, 6)] public string secrets;
        public List<DCEntry> dcEntries = new List<DCEntry>();
        public List<MonsterReference> monsterReferences = new List<MonsterReference>();
        public List<SceneChoice> choices = new List<SceneChoice>();
        public List<string> suggestedSoundCueIds = new List<string>();
        public bool isElementalRoom;
        public ElementalType elementalType = ElementalType.None;
        [TextArea(2, 5)] public string purificationOutcomeText;
        [TextArea(2, 5)] public string absorptionOutcomeText;
        public int infectionChangeOnPurify = -1;
        public int infectionChangeOnAbsorb = 1;
        public int castleIntegrityChangeOnPurify = 1;
        public int castleIntegrityChangeOnAbsorb = -1;
        [TextArea(1, 4)] public string finalBattleModifierIfPurified;
        [TextArea(1, 4)] public string finalBattleModifierIfAbsorbed;
    }
}
