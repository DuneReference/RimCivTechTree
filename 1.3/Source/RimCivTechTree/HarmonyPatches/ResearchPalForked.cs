using System;

using Verse;
using HarmonyLib;
using System.Reflection;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace DuneRef_RimCivTechTree
{
    public static class ResearchPalForkedPatches
    {
        public static readonly Type patchType = typeof(ResearchPalForkedPatches);
        public static Harmony Harm = HarmonyPatches.Harm;

        public static void Patches()
        {
            // Hide nodes to be hidden
            MethodInfo populateNodesInnerMethod = AccessTools.FindIncludingInnerTypes(
                typeof(ResearchPal.Tree),
                (type) => AccessTools.FirstMethod(
                    type,
                    (method) => method.Name.Contains("<PopulateNodes>") && method.ReturnType == typeof(bool)
                )
            );
            Harm.Patch(populateNodesInnerMethod, postfix: new HarmonyMethod(patchType, nameof(ResearchPalTreePopulateNodesHiddenPostfix)));

            // Unlock techs this higher level tech is associated with
            Harm.Patch(AccessTools.Method(typeof(ResearchPal.HarmonyPatches_Queue.DoCompletionDialog), "Postfix"), prefix: new HarmonyMethod(CommonPatches.patchType, nameof(CommonPatches.FinishProjectOmniFix)));

            // Make icons in nodes show up on higher level techs based on the lower nodes they link to.
            Harm.Patch(AccessTools.Method(typeof(ResearchPal.ResearchProjectDef_Extensions), nameof(ResearchPal.ResearchProjectDef_Extensions.GetPlantsUnlocked)), prefix: new HarmonyMethod(patchType, nameof(GetPlantsUnlockedPrefix)));
            Harm.Patch(AccessTools.Method(typeof(ResearchPal.ResearchProjectDef_Extensions), nameof(ResearchPal.ResearchProjectDef_Extensions.GetRecipesUnlocked)), prefix: new HarmonyMethod(patchType, nameof(GetRecipesUnlockedPrefix)));
            Harm.Patch(AccessTools.Method(typeof(ResearchPal.ResearchProjectDef_Extensions), nameof(ResearchPal.ResearchProjectDef_Extensions.GetTerrainUnlocked)), prefix: new HarmonyMethod(patchType, nameof(GetTerrainUnlockedPrefix)));
            Harm.Patch(AccessTools.Method(typeof(ResearchPal.ResearchProjectDef_Extensions), nameof(ResearchPal.ResearchProjectDef_Extensions.GetThingsUnlocked)), prefix: new HarmonyMethod(patchType, nameof(GetThingsUnlockedPrefix)));
        }

        // Harmony Functions
        public static bool ResearchPalTreePopulateNodesHiddenPostfix(bool __result, ResearchProjectDef p)
        {
            return __result || (p.tab != null && p.tab == RimCivTechTree_DefOf.DuneRef_Hidden);
        }

        public static bool GetPlantsUnlockedPrefix(ref IEnumerable<ThingDef> __result, ResearchProjectDef research)
        {
            __result = DefDatabase<ThingDef>.AllDefs.Where((td) =>
            {
                bool found = td.plant?.sowResearchPrerequisites?.Contains(research) ?? false;

                if (!found && research.GetModExtension<ResearchUnlocks>() != null)
                {
                    foreach (ResearchProjectDef unlock in research.GetModExtension<ResearchUnlocks>().researchUnlocks)
                    {
                        found = td.plant?.sowResearchPrerequisites?.Contains(unlock) ?? false;
                        if (found) break;
                    }
                }

                return found;
            });

            return false;
        }

        public static bool GetRecipesUnlockedPrefix(ref IEnumerable<RecipeDef> __result, ResearchProjectDef research)
        {
            __result = DefDatabase<RecipeDef>.AllDefs.Where((rd) =>
            {
                bool found = rd.researchPrerequisite == research ||
                             (rd.researchPrerequisites != null &&
                              rd.researchPrerequisites.Contains(research));

                if (!found && research.GetModExtension<ResearchUnlocks>() != null)
                {
                    foreach (ResearchProjectDef unlock in research.GetModExtension<ResearchUnlocks>().researchUnlocks)
                    {
                        found = rd.researchPrerequisite == unlock ||
                               (rd.researchPrerequisites != null &&
                                rd.researchPrerequisites.Contains(unlock));
                        if (found) break;
                    }
                }

                return found;
            });

            return false;
        }

        public static bool GetTerrainUnlockedPrefix(ref IEnumerable<TerrainDef> __result, ResearchProjectDef research)
        {
            __result = DefDatabase<TerrainDef>.AllDefs.Where((td) =>
            {
                bool found = td.researchPrerequisites?.Contains(research) ?? false;

                if (!found && research.GetModExtension<ResearchUnlocks>() != null)
                {
                    foreach (ResearchProjectDef unlock in research.GetModExtension<ResearchUnlocks>().researchUnlocks)
                    {
                        found = td.researchPrerequisites?.Contains(unlock) ?? false;
                        if (found) break;
                    }
                }

                return found;
            });

            return false;
        }

        public static bool GetThingsUnlockedPrefix(ref IEnumerable<ThingDef> __result, ResearchProjectDef research)
        {
            __result = DefDatabase<ThingDef>.AllDefs.Where((td) =>
            {
                bool found = td.researchPrerequisites?.Contains(research) ?? false;

                if (!found && research.GetModExtension<ResearchUnlocks>() != null)
                {
                    foreach (ResearchProjectDef unlock in research.GetModExtension<ResearchUnlocks>().researchUnlocks)
                    {
                        found = td.researchPrerequisites?.Contains(unlock) ?? false;
                        if (found) break;
                    }
                }

                return found;
            });

            return false;
        }
    }
}