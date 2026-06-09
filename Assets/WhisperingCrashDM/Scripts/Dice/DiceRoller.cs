using System;
using System.Collections.Generic;
using UnityEngine;

namespace WhisperingCrashDM
{
    public class DiceRoller : MonoBehaviour
    {
        [SerializeField] int historyLimit = 10;
        public readonly List<RollResult> History = new List<RollResult>();
        public event Action<RollResult> Rolled;

        public RollResult RollDie(int sides, string label = null, int modifier = 0, string interpretation = null)
        {
            int roll = DiceExpression.RollDie(sides);
            return Record(new RollResult { label = string.IsNullOrEmpty(label) ? "d" + sides : label, die = sides, roll = roll, modifier = modifier, total = roll + modifier, interpretation = interpretation });
        }

        public RollResult XylosIntrusion(AdventureState state)
        {
            int roll = DiceExpression.RollDie(20); int total = roll + state.infectionLevel;
            return Record(new RollResult { label = "Xylos Intrusion Check", die = 20, roll = roll, modifier = state.infectionLevel, total = total, interpretation = total >= 20 ? "Parasite advances" : total >= 15 ? "Memory bleed" : total >= 10 ? "Compulsion flicker" : "Whisper only" });
        }

        public RollResult PsychicBacklash(AdventureState state)
        {
            int roll = DiceExpression.RollDie(20); int total = roll + state.psychicPressure;
            return Record(new RollResult { label = "Psychic Backlash", die = 20, roll = roll, modifier = state.psychicPressure, total = total, interpretation = total >= 20 ? "Xylos takes an immediate narrative advantage" : total >= 15 ? "One character receives intrusive vision" : total >= 10 ? "Minor psychic static" : "No backlash" });
        }

        public RollResult CastleIntegrityCheck(AdventureState state)
        {
            int roll = DiceExpression.RollDie(20); int total = roll + state.castleIntegrity;
            return Record(new RollResult { label = "Castle Integrity Check", die = 20, roll = roll, modifier = state.castleIntegrity, total = total, interpretation = total >= 19 ? "Purified systems briefly awaken" : total >= 14 ? "Castle resists corruption" : total >= 9 ? "Unstable but holding" : "Structural rupture" });
        }

        RollResult Record(RollResult result)
        {
            History.Insert(0, result);
            while (History.Count > historyLimit) History.RemoveAt(History.Count - 1);
            Rolled?.Invoke(result);
            return result;
        }
    }
}
