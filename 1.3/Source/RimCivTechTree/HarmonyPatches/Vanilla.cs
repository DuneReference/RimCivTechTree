using System;

using Verse;
using HarmonyLib;
using System.Reflection;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace DuneRef_RimCivTechTree
{
    public static class VanillaPatches
    {
        public static readonly Type patchType = typeof(VanillaPatches);
        public static Harmony Harm = HarmonyPatches.Harm;

        public static void Patches()
        {
            Harm.Patch(AccessTools.Method(typeof(ResearchManager), "FinishProject"), postfix: new HarmonyMethod(CommonPatches.patchType, nameof(CommonPatches.FinishProjectOmniFix)));
        }
    }
}