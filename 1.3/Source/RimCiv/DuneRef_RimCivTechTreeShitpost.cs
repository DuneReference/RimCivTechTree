using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using HarmonyLib;

namespace DuneRef_RimCivTechTree
{
    [StaticConstructorOnStartup]
    public static class DuneRef_RimCivTechTreeShitpost
    {
        static DuneRef_RimCivTechTreeShitpost()
        {
            // Fires second
            Log.Message("Static DuneRef_RimCivTechTree Class SHITPOST Loaded");
        }
    }

    public class DuneRef_RimCivTechTreeShitpostConfig : Mod
    {
        public DuneRef_RimCivTechTreeShitpostConfig(ModContentPack content) : base(content)
        {
            // Fires first
            Log.Message("Inherited DuneRef_RimCivTechTree Class SHITPOST Loaded");
        }
    }

    [HarmonyPatch]
    public static class Building_Battery_Shitpost_Patch
    {
        [HarmonyPatch(typeof(Building_Battery), nameof(Building_Battery.PostApplyDamage))]
        [HarmonyPostfix]
        public static void PostApplyDamage_Postfix()
        {
            // Fire on Building_Battery taking damage.
            Log.Message("Harmony SHITPOST Patch Applied: " + DuneRef_RimCivTechTree_DefOf.DuneRef_Currency.IsFinished);
        }
    }
}
