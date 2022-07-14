using System;

using Verse;
using HarmonyLib;
using System.Reflection;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace DuneRef_RimCivTechTree
{
    public static class DubsMintMenusPatches
    {
        public static readonly Type patchType = typeof(DubsMintMenusPatches);
        public static Harmony Harm = HarmonyPatches.Harm;

        public static void Patches()
        {
            Harm.Patch(AccessTools.Method(typeof(DubsMintMenus.HarmonyPatches.Patch_FinishProject), "Postfix"), prefix: new HarmonyMethod(CommonPatches.patchType, nameof(CommonPatches.FinishProjectOmniFix)));
        }
    }
}