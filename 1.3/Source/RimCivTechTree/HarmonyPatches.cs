using System;

using Verse;
using HarmonyLib;
using System.Reflection;
using RimWorld;

namespace DuneRef_RimCivTechTree
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);
        public static Harmony Harm;

        static HarmonyPatches()
        {
            Harm = new Harmony("rimworld.mod.duneref.rimcivtechtree");
            
            if (ModLister.HasActiveModWithName("ResearchPal - Forked"))
            {
                conditionalResearchPalPatch();
            }
            else if (ModLister.HasActiveModWithName("Dubs Mint Menus"))
            {
                conditionalDubsPatch();
            } 
            else
            {
                Harm.Patch(AccessTools.Method(typeof(ResearchManager), "FinishProject"), postfix: new HarmonyMethod(patchType, nameof(FinishProjectOmniFix)));
            }
        }

        public static void conditionalResearchPalPatch()
        {
            MethodInfo populateNodesInnerMethod = AccessTools.FindIncludingInnerTypes(
                typeof(ResearchPal.Tree),
                (type) => AccessTools.FirstMethod(
                    type,
                    (method) => method.Name.Contains("<PopulateNodes>") && method.ReturnType == typeof(bool)
                )
            );

            Harm.Patch(populateNodesInnerMethod, postfix: new HarmonyMethod(patchType, nameof(ResearchPalTreePopulateNodesHiddenPostfix)));

            Harm.Patch(AccessTools.Method(typeof(ResearchPal.HarmonyPatches_Queue.DoCompletionDialog), "Postfix"), prefix: new HarmonyMethod(patchType, nameof(FinishProjectOmniFix)));
        }

        public static void conditionalDubsPatch()
        {
            Harm.Patch(AccessTools.Method(typeof(DubsMintMenus.HarmonyPatches.Patch_FinishProject), "Postfix"), prefix: new HarmonyMethod(patchType, nameof(FinishProjectOmniFix)));
        }

        public static bool ResearchPalTreePopulateNodesHiddenPostfix(bool __result, ResearchProjectDef p)
        {
            return __result || (p.tab != null && p.tab == RimCivTechTree_DefOf.DuneRef_Hidden); 
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