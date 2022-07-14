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
        public static void conditionalDubsPatches()
        {
            HarmonyPatches.Harm.Patch(AccessTools.Method(typeof(DubsMintMenus.HarmonyPatches.Patch_FinishProject), "Postfix"), prefix: new HarmonyMethod(HarmonyPatches.patchType, nameof(HarmonyPatches.FinishProjectOmniFix)));
        }
    }
}