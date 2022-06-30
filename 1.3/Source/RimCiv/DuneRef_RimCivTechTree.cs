using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
/*using RimWorld.Planet;*/
using Verse;
/*using HarmonyLib;*/

using VFEC;

namespace DuneRef_RimCivTechTree
{
    [StaticConstructorOnStartup]
    public static class DuneRef_RimCivTechTree
    {
        /*public static Harmony Harm;*/
        static DuneRef_RimCivTechTree()
        {
            // Fires second
            Log.Message("Static DuneRef_RimCivTechTree Class Loaded");

            if (ModLister.HasActiveModWithName("Vanilla Factions Expanded - Classical"))
            {
                Log.Message("VFEC Found, Patching");
                // Change RoadBuilding Requirement from VFEC_RoadBuilding to DuneRef_Currency
                ChangeRoadBuildingTech();
            } else
            {
                Log.Message("VFEC Not Found");
            }

            /*Harm = new Harmony("rimworld.mod.duneref.rimcivtechtree");
            Harm.PatchAll();*/
        }

        private static void ChangeRoadBuildingTech()
        {
            VFEC_DefOf.VFEC_RoadBuilding = DuneRef_RimCivTechTree_DefOf.DuneRef_Currency;
        }
    }

    public class DuneRef_RimCivTechTreeConfig : Mod
    {
        public DuneRef_RimCivTechTreeConfig(ModContentPack content) : base(content)
        {
            // Fires first
            Log.Message("Inherited DuneRef_RimCivTechTree Class Loaded");
        }
    }

    /*[HarmonyPatch]
    public static class Building_Battery_Patch
    {
        [HarmonyPatch(typeof(Caravan), nameof(Caravan.GetGizmos))]
        [HarmonyPrefix]
        public static void PostApplyDamage_Prefix()
        {
            
            Log.Message("WE IN HERE");
        }
    }*/
}
