using UnityEngine;

namespace WhisperingCrashDM
{
    public static class DiceExpression
    {
        public static int RollDie(int sides) => Random.Range(1, sides + 1);
    }
}
