using System.Collections.Generic;
using UnityEngine;

namespace WhisperingCrashDM
{
    public static class SampleAdventureFactory
    {
        public static List<AdventureSceneData> CreateScenes()
        {
            return new List<AdventureSceneData>
            {
                Scene("crash_approach", "Crash Approach", SceneCategory.Opening, "The party approaches the crashed, corrupted Castlemania.", "The castle lies half-buried in the world like an idea that fell from another plane. Towers bend through impossible angles, stone whispers in overlapping voices, and every shadow seems to notice you back.", "Introduce unease without naming Xylos. Let the players see familiar castle shapes fused with psychic scar tissue.", "The first whispers are diagnostic probes from Xylos, not ghosts.", cues: new[]{"Castle Groan","Distant Whisper"}),
                Scene("castle_courtyard", "Castle Courtyard", SceneCategory.Exploration, "A warped courtyard fuses Castlemania architecture with organic psychic growth.", "The courtyard stones pulse like a sleeping throat. Old banners twitch in wind that is not there, while thorned nerves web the battlements.", "Give players practical routes, sensory horror, and the first clues that the castle can be healed or dominated.", "Organic growth retracts from sincere mercy and leans toward aggressive commands.", dcs: new[]{ DC("Arcane pulse",14,"Arcana","Identify six bound elemental systems under parasite control.","Mistake the pulse for normal transplanar fallout."), DC("Growth pattern",13,"Investigation","Find safe seams through the living stone.","Trigger a harmless but disturbing twitch of the walls."), DC("Watched angles",15,"Perception","Notice eye-like knots tracking speech.","Xylos hears a private plan."), DC("Wreck ecology",12,"Survival","Read which passages are physically stable.","Choose a route that groans underfoot.")}, cues: new[]{"Organic Pulse","Stone Creak"}),
                Scene("first_echo_contact", "First Echo Contact", SceneCategory.Social, "Echo reaches out as a sympathetic voice claiming to be trapped inside the corruption.", "A voice arrives inside the pause between heartbeats: gentle, frightened, and too intimate. It says it can help you survive the thing wearing the castle.", "Echo should be helpful but manipulative. She gives true tactical information while steering the party away from Majel.", "Echo is partly parasite-facing and partly victim; both readings are useful at the table.", choices: new[]{ Choice("trust_echo_1","Trust Echo slightly",ChoiceType.TrustEcho,"Let Echo guide one immediate step.",0,0,"Echo Trust increases; Echo offers a shortcut."), Choice("resist_echo_1","Resist Echo",ChoiceType.Resist,"Shut the voice out with discipline or magic.",0,0,"Psychic pressure eases, but Echo becomes more careful."), Choice("ask_majel_1","Ask about Majel",ChoiceType.Investigate,"Press Echo for the name behind the static.",0,0,"Echo deflects and psychic pressure rises by curiosity.")}, cues: new[]{"Echo Whisper"}),
                Scene("majel_reveal", "Majel Reveal", SceneCategory.Revelation, "Majel is discovered: a human stretched across time and integrated into corrupted systems.", "A human shape flickers across childhood, age, ash, and birth in the same breath. Majel's eyes focus from several moments at once, and all of them look exhausted.", "Majel can explain purification as release, not conquest. Keep them sympathetic and frightening.", "Majel once helped bind the systems that Xylos now exploits.", choices: new[]{ Choice("trust_majel_1","Trust Majel",ChoiceType.TrustMajel,"Accept Majel's interpretation of purification.",0,0,"Majel Trust increases; final deceptions become easier to parse."), Choice("doubt_majel_1","Doubt Majel",ChoiceType.Resist,"Treat Majel as another corrupted interface.",0,0,"The party keeps distance; pressure drops slightly."), Choice("ask_purification","Ask about purification",ChoiceType.Investigate,"Ask what it costs to cleanse a bound elemental.",0,0,"Majel explains purify versus absorb consequences.")}, cues: new[]{"Time Stutter"}),
                Elemental("fire_chamber","Fire Elemental Chamber",ElementalType.Fire,"rage, hunger, and burning memory","Fire stabilizes into protective warmth; Xylos loses a violent channel.","The party takes violent power into themselves; Xylos learns their anger.","Fire Purified: party gains one-time resistance against psychic flame.","Fire Absorbed: God Machine attacks gain a reckless flame surge." , new[]{"Fire Surge","Xylos Pulse"}),
                Elemental("water_chamber","Water Elemental Chamber",ElementalType.Water,"grief, memory, and drowning empathy","The water clears into healing flow and restores emotional clarity.","The party gains psychic fluidity, but identity boundaries soften.","Water Purified: one memory bleed can be soothed instead of damaging trust.","Water Absorbed: Xylos can blur identities during Phase 2." , new[]{"Drowned Whisper","Water Surge"}),
                Elemental("earth_chamber","Earth Elemental Chamber",ElementalType.Earth,"body horror, stone-flesh fusion, and endurance","The castle regains structural stability and safe footing.","The party gains resilience while the castle grows more parasitic.","Earth Purified: collapsing terrain is delayed for one round.","Earth Absorbed: Xylos armors one guardian in living stone." , new[]{"Stone Heartbeat","Bone Grind"}),
                Elemental("air_chamber","Air Elemental Chamber",ElementalType.Air,"breath, thought, paranoia, and voices in empty space","The air clears; psychic static drops and true voices separate.","Xylos receives easier access to stray thoughts and plans.","Air Purified: whispered lies are obvious for one exchange.","Air Absorbed: initiative begins with intrusive tactical whispers." , new[]{"Air Rush","Whisper Swarm"}),
                Elemental("storm_chamber","Storm Elemental Chamber",ElementalType.Storm,"violent revelation, electrical thought, and wrathful insight","Controlled power returns to the castle's defenses.","The final battle becomes more explosive and unstable.","Storm Purified: purified lightning can interrupt a machine pulse once.","Storm Absorbed: every phase transition triggers psychic lightning." , new[]{"Storm Crack","Psychic Snap"}),
                Elemental("necro_chamber","Necro Elemental Chamber",ElementalType.Necro,"death, undeath, old campaign ghosts, and the cost of preservation","Trapped remnants are released, weakening Xylos's hold on endings.","Necrotic command grows, but moral corruption worsens.","Necro Purified: Xylos loses one remnant shield.","Necro Absorbed: Xylos can raise one fallen castle remnant." , new[]{"Dead Choir","Grave Pulse"}),
                Scene("god_machine_entrance", "God Machine Entrance", SceneCategory.FinalBattle, "The party reaches the central engine of corrupted Castlemania.", "The God Machine turns without gears. Veins of light pump through its ribs while every purified and absorbed choice answers from inside the walls.", "Display active final battle modifiers prominently before initiative.", "The machine reacts to the players' pattern, not just their totals.", cues: new[]{"God Machine Drone"}),
                Scene("god_machine_phase_1", "God Machine Phase 1", SceneCategory.FinalBattle, "Biological defenses and corrupted guardians try to halt the party.", "The floor opens in defensive petals. Guardian shapes step out wearing bits of castle, bone, banner, and prayer.", "Use elemental modifiers from purify and absorb outcomes.", "Guardians target whoever benefited most from absorption first."),
                Scene("god_machine_phase_2", "God Machine Phase 2", SceneCategory.FinalBattle, "Psychic override: Echo and Majel compete to define salvation.", "The machine stops fighting bodies and begins rewriting motives. Echo sounds like rescue. Majel sounds like pain. Xylos sounds like both.", "Use infection, Echo Trust, Majel Trust, and psychic pressure.", "High trust in both NPCs creates a dramatic contradiction, not a failure state."),
                Scene("final_choice", "Final Choice", SceneCategory.Ending, "The party chooses purification, domination, compromise, or collapse.", "The castle waits for the last command. For the first time since the crash, the whispers stop and listen.", "Show infection, purified/absorbed counts, castle integrity, NPC trust, and active final modifiers.", "Any ending can be costly, but none should erase player agency.")
            };
        }

        public static List<SoundCueData> CreateSoundCues()
        {
            string[] names = { "Echo Whisper", "Xylos Pulse", "God Machine Drone", "Castle Groan", "Time Stutter", "Fire Surge", "Water Surge", "Stone Heartbeat", "Air Rush", "Storm Crack", "Dead Choir", "Psychic Snap", "Distant Whisper", "Organic Pulse", "Stone Creak", "Drowned Whisper", "Bone Grind", "Whisper Swarm", "Grave Pulse" };
            var list = new List<SoundCueData>();
            foreach (var name in names)
            {
                var cue = ScriptableObject.CreateInstance<SoundCueData>();
                cue.cueId = name;
                cue.displayName = name;
                cue.loop = name.Contains("Drone") || name.Contains("Pulse");
                cue.category = cue.loop ? "Ambient" : "Sting";
                list.Add(cue);
            }
            return list;
        }

        static AdventureSceneData Elemental(string id, string name, ElementalType type, string theme, string purify, string absorb, string modPurify, string modAbsorb, string[] cues)
        {
            var s = Scene(id, name, SceneCategory.ElementalTrial, "Theme: " + theme + ".", "The " + type + " chamber breathes with " + theme + ". Its bound elemental is not evil, but Xylos has taught it pain as a language.", "Purification requires controlled release and empathy. Absorption offers immediate power with parasite consequences.", "The elemental remembers Castlemania before the crash.", cues: cues);
            s.isElementalRoom = true; s.elementalType = type;
            s.purificationOutcomeText = purify; s.absorptionOutcomeText = absorb;
            s.finalBattleModifierIfPurified = modPurify; s.finalBattleModifierIfAbsorbed = modAbsorb;
            s.infectionChangeOnPurify = -1; s.infectionChangeOnAbsorb = 1; s.castleIntegrityChangeOnPurify = 1; s.castleIntegrityChangeOnAbsorb = -1;
            s.dcEntries.Add(DC("Read the corruption", 14, "Arcana or Religion", "Learn whether purify or absorb is safer here.", "Xylos frames domination as mercy."));
            s.monsterReferences.Add(new MonsterReference { monsterName = "Corrupted " + type + " Elemental", role = "Trial / hazard", quickStats = "Use a level-appropriate elemental or hazard stat block.", tactics = "Punish greed; respond to empathy.", specialNotes = "Does not need to die if purified." });
            return s;
        }

        static AdventureSceneData Scene(string id, string name, SceneCategory category, string summary, string read, string notes, string secrets, DCEntry[] dcs = null, SceneChoice[] choices = null, string[] cues = null)
        {
            var s = ScriptableObject.CreateInstance<AdventureSceneData>();
            s.sceneId = id; s.displayName = name; s.category = category; s.shortSummary = summary; s.readAloudText = read; s.dmNotes = notes; s.secrets = secrets;
            if (dcs != null) s.dcEntries.AddRange(dcs);
            if (choices != null) s.choices.AddRange(choices);
            if (cues != null) s.suggestedSoundCueIds.AddRange(cues);
            return s;
        }
        static DCEntry DC(string label, int dc, string skill, string success, string failure) => new DCEntry { label = label, dc = dc, skillOrSave = skill, successText = success, failureText = failure };
        static SceneChoice Choice(string id, string label, ChoiceType type, string description, int inf, int castle, string consequence) => new SceneChoice { choiceId = id, label = label, choiceType = type, description = description, infectionDelta = inf, castleIntegrityDelta = castle, consequenceText = consequence };
    }
}
