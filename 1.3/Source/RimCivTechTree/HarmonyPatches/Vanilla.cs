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

            Harm.Patch(AccessTools.PropertyGetter(typeof(ResearchProjectDef), nameof(ResearchProjectDef.UnlockedDefs)), prefix: new HarmonyMethod(typeof(VanillaPatches), nameof(UnlockedDefsPrefix)));
        }

        // Until I find a reason not to, I've removed all the standard researches from being added to unlockedDefs.
        // This makes my researches not have info in the leftRect showing that it's co-depedent on the old techs.
        public static bool UnlockedDefsPrefix(ref List<Def> __result, ResearchProjectDef __instance, ref List<Def> ___cachedUnlockedDefs)
        {
            if (___cachedUnlockedDefs == null)
            {
                ___cachedUnlockedDefs = DefDatabase<RecipeDef>.AllDefs.Where(x =>
                {
                    bool found = false;

                    if (!found && __instance.GetModExtension<ResearchUnlocks>() != null)
                    {
                        foreach (ResearchProjectDef unlock in __instance.GetModExtension<ResearchUnlocks>().researchUnlocks)
                        {
                            found = x.researchPrerequisite == unlock ||
                                   (x.researchPrerequisites != null &&
                                    x.researchPrerequisites.Contains(unlock));
                            if (found) break;
                        }
                    }

                    return found;
                }).SelectMany(x => x.products.Select(y => (Def)y.thingDef))
                   .OrderBy(x => x.label)
                   .Concat(
                        DefDatabase<ThingDef>.AllDefs
                            .Where(x =>
                            {
                                bool found = false;

                                if (!found && __instance.GetModExtension<ResearchUnlocks>() != null)
                                {
                                    foreach (ResearchProjectDef unlock in __instance.GetModExtension<ResearchUnlocks>().researchUnlocks)
                                    {
                                        found = x.researchPrerequisites != null &&
                                                x.researchPrerequisites.Contains(unlock);
                                        if (found) break;
                                    }
                                }

                                return found;
                            })
                            .OrderBy(x => x.label))
                   .Concat(
                        DefDatabase<ThingDef>.AllDefs
                            .Where(x =>
                            {
                                bool found = false;

                                if (!found && __instance.GetModExtension<ResearchUnlocks>() != null)
                                {
                                    foreach (ResearchProjectDef unlock in __instance.GetModExtension<ResearchUnlocks>().researchUnlocks)
                                    {
                                        found = x.plant != null &&
                                                x.plant.sowResearchPrerequisites != null &&
                                                x.plant.sowResearchPrerequisites.Contains(__instance);
                                        if (found) break;
                                    }
                                }

                                return found;
                            })
                            .OrderBy(x => x.label))
                   .Concat(
                        DefDatabase<TerrainDef>.AllDefs
                            .Where(x =>
                            {
                                bool found = false;

                                if (!found && __instance.GetModExtension<ResearchUnlocks>() != null)
                                {
                                    foreach (ResearchProjectDef unlock in __instance.GetModExtension<ResearchUnlocks>().researchUnlocks)
                                    {
                                        found = x.researchPrerequisites != null &&
                                                x.researchPrerequisites.Contains(unlock);
                                        if (found) break;
                                    }
                                }

                                return found;
                            })
                            .OrderBy(x => x.label))
                   .Distinct()
                   .ToList();
            }

            __result = ___cachedUnlockedDefs;

            return false;
        }
    }
}