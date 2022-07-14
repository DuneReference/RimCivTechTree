using System;

using Verse;
using HarmonyLib;
using System.Reflection;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace DuneRef_RimCivTechTree
{
    public static class CommonPatches
    {
        public static readonly Type patchType = typeof(CommonPatches);
        public static Harmony Harm = HarmonyPatches.Harm;

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