using System.Collections.Generic;
using System.Text;

namespace WhisperingCrashDM
{
    public static class FinalBattleSummaryGenerator
    {
        public static readonly ElementalType[] Elementals = { ElementalType.Fire, ElementalType.Water, ElementalType.Earth, ElementalType.Air, ElementalType.Storm, ElementalType.Necro };

        public static string Generate(AdventureState state)
        {
            state.Clamp();
            var sb = new StringBuilder();
            sb.AppendLine("Xylos Influence: " + InfluenceLabel(state.infectionLevel));
            sb.AppendLine("Castle Condition: " + CastleLabel(state.castleIntegrity));
            sb.AppendLine("Purified Systems: " + ListElementals(state, true));
            sb.AppendLine("Absorbed Systems: " + ListElementals(state, false));
            sb.AppendLine("Final Battle Effects:");
            foreach (var modifier in state.activeFinalBattleModifiers)
                sb.AppendLine("• " + modifier);
            if (state.infectionLevel >= 5) sb.AppendLine("• High Infection: Echo's voice can interrupt initiative once.");
            if (state.castleIntegrity <= 1) sb.AppendLine("• Failing Castle: each phase begins with collapsing terrain or ruptured geometry.");
            if (state.castleIntegrity >= 4) sb.AppendLine("• Strengthened Castle: purified defenses grant the party one safe reposition.");
            if (state.echoTrust >= 4) sb.AppendLine("• Echo Trusted: Echo can offer a shortcut, but Xylos may lace it with a cost.");
            if (state.majelTrust >= 4) sb.AppendLine("• Majel Trusted: Majel clarifies one God Machine deception.");
            if (state.activeFinalBattleModifiers.Count == 0) sb.AppendLine("• No elemental modifiers recorded yet.");
            return sb.ToString();
        }

        static string InfluenceLabel(int value) => value >= 5 ? "Severe" : value >= 3 ? "Growing" : value >= 1 ? "Present" : "Contained";
        static string CastleLabel(int value) => value <= 1 ? "Failing" : value <= 3 ? "Stable" : "Strengthened";

        static string ListElementals(AdventureState state, bool purified)
        {
            var names = new List<string>();
            foreach (var elemental in Elementals)
            {
                if (purified && state.GetPurified(elemental)) names.Add(elemental.ToString());
                if (!purified && state.GetAbsorbed(elemental)) names.Add(elemental.ToString());
            }
            return names.Count == 0 ? "None" : string.Join(", ", names.ToArray());
        }
    }
}
