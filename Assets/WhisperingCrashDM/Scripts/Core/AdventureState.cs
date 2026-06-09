using System;
using System.Collections.Generic;
using UnityEngine;

namespace WhisperingCrashDM
{
    [Serializable]
    public class AdventureState
    {
        public int infectionLevel;
        public int castleIntegrity = 3;
        public int echoTrust;
        public int majelTrust;
        public int psychicPressure;
        public int elementalsPurified;
        public int elementalsAbsorbed;
        public bool firePurified, waterPurified, earthPurified, airPurified, stormPurified, necroPurified;
        public bool fireAbsorbed, waterAbsorbed, earthAbsorbed, airAbsorbed, stormAbsorbed, necroAbsorbed;
        public List<string> completedSceneIds = new List<string>();
        public List<string> activeFinalBattleModifiers = new List<string>();
        public List<string> adventureLog = new List<string>();

        public void Clamp()
        {
            infectionLevel = Mathf.Clamp(infectionLevel, 0, 6);
            castleIntegrity = Mathf.Clamp(castleIntegrity, 0, 5);
            echoTrust = Mathf.Clamp(echoTrust, 0, 5);
            majelTrust = Mathf.Clamp(majelTrust, 0, 5);
            psychicPressure = Mathf.Clamp(psychicPressure, 0, 6);
            elementalsPurified = CountPurified();
            elementalsAbsorbed = CountAbsorbed();
        }

        public bool IsResolved(ElementalType elemental) => GetPurified(elemental) || GetAbsorbed(elemental);

        public bool GetPurified(ElementalType elemental)
        {
            switch (elemental)
            {
                case ElementalType.Fire: return firePurified;
                case ElementalType.Water: return waterPurified;
                case ElementalType.Earth: return earthPurified;
                case ElementalType.Air: return airPurified;
                case ElementalType.Storm: return stormPurified;
                case ElementalType.Necro: return necroPurified;
                default: return false;
            }
        }

        public bool GetAbsorbed(ElementalType elemental)
        {
            switch (elemental)
            {
                case ElementalType.Fire: return fireAbsorbed;
                case ElementalType.Water: return waterAbsorbed;
                case ElementalType.Earth: return earthAbsorbed;
                case ElementalType.Air: return airAbsorbed;
                case ElementalType.Storm: return stormAbsorbed;
                case ElementalType.Necro: return necroAbsorbed;
                default: return false;
            }
        }

        public void SetPurified(ElementalType elemental)
        {
            switch (elemental)
            {
                case ElementalType.Fire: firePurified = true; break;
                case ElementalType.Water: waterPurified = true; break;
                case ElementalType.Earth: earthPurified = true; break;
                case ElementalType.Air: airPurified = true; break;
                case ElementalType.Storm: stormPurified = true; break;
                case ElementalType.Necro: necroPurified = true; break;
            }
            Clamp();
        }

        public void SetAbsorbed(ElementalType elemental)
        {
            switch (elemental)
            {
                case ElementalType.Fire: fireAbsorbed = true; break;
                case ElementalType.Water: waterAbsorbed = true; break;
                case ElementalType.Earth: earthAbsorbed = true; break;
                case ElementalType.Air: airAbsorbed = true; break;
                case ElementalType.Storm: stormAbsorbed = true; break;
                case ElementalType.Necro: necroAbsorbed = true; break;
            }
            Clamp();
        }

        public string GetElementalStatus(ElementalType elemental)
        {
            if (GetPurified(elemental)) return "Purified";
            if (GetAbsorbed(elemental)) return "Absorbed";
            return "Unresolved";
        }

        int CountPurified() { int count = 0; foreach (ElementalType e in FinalBattleSummaryGenerator.Elementals) if (GetPurified(e)) count++; return count; }
        int CountAbsorbed() { int count = 0; foreach (ElementalType e in FinalBattleSummaryGenerator.Elementals) if (GetAbsorbed(e)) count++; return count; }
    }
}
