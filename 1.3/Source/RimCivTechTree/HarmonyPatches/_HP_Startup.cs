using System;

using Verse;
using HarmonyLib;
using System.Reflection;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace DuneRef_RimCivTechTree
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        public static readonly Type patchType = typeof(HarmonyPatches);
        public static Harmony Harm;

        static HarmonyPatches()
        {
            Harm = new Harmony("rimworld.mod.duneref.rimcivtechtree");
            
            if (ModLister.HasActiveModWithName("ResearchPal - Forked"))
            {
                ResearchPalForkedPatches.conditionalResearchPalPatches();
            }
            else if (ModLister.HasActiveModWithName("Dubs Mint Menus"))
            {
                DubsMintMenusPatches.conditionalDubsPatches();
            } 
            else
            {
                Harm.Patch(AccessTools.Method(typeof(ResearchManager), "FinishProject"), postfix: new HarmonyMethod(patchType, nameof(FinishProjectOmniFix)));
            }
        }

        public static void FinishProjectOmniFix(ResearchProjectDef proj)
        {
            if (!proj.IsFinished)
                return;

            if (proj.GetModExtension<ResearchUnlocks>() != null)
            {
                foreach (ResearchProjectDef unlock in proj.GetModExtension<ResearchUnlocks>().researchUnlocks)
                {
                    Find.ResearchManager.FinishProject(unlock);
                }
            }
        }
    }
}