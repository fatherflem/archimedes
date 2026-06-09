using UnityEngine;

namespace WhisperingCrashDM
{
    public class DicePanel : MonoBehaviour
    {
        Transform history;
        DiceRoller roller;
        public void Build(DiceRoller diceRoller)
        {
            roller = diceRoller;
            var root = UIFactoryHelpers.Vertical(transform,"Dice Content").transform;
            int[] dice = {20,12,10,8,6,4,100};
            foreach (int sides in dice) UIFactoryHelpers.Button(root,"d"+sides,()=>roller.RollDie(sides));
            UIFactoryHelpers.Button(root,"Xylos Intrusion Check",()=>roller.XylosIntrusion(AdventureManager.Instance.State));
            UIFactoryHelpers.Button(root,"Psychic Backlash",()=>roller.PsychicBacklash(AdventureManager.Instance.State));
            UIFactoryHelpers.Button(root,"Castle Integrity Check",()=>roller.CastleIntegrityCheck(AdventureManager.Instance.State));
            UIFactoryHelpers.Button(root,"Secret DM Roll",()=>roller.RollDie(20,"Secret DM Roll"));
            UIFactoryHelpers.Text(root,"Recent","Recent Rolls",22,FontStyle.Bold);
            history = UIFactoryHelpers.Vertical(root,"Roll History",4).transform;
            roller.Rolled += _ => Refresh(); Refresh();
        }
        void Refresh()
        {
            if (history == null || roller == null) return; UIFactoryHelpers.Clear(history);
            foreach (var roll in roller.History) UIFactoryHelpers.Text(history,"Roll",roll.ToString(),18);
        }
    }
}
