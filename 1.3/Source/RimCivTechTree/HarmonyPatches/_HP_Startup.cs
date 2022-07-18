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
        public static Harmony Harm;

        static HarmonyPatches()
        {
            Harm = new Harmony("rimworld.mod.duneref.rimcivtechtree");
            
            if (ModLister.HasActiveModWithName("ResearchPal - Forked"))
            {
                ResearchPalForkedPatches.Patches();
            }
            else if (ModLister.HasActiveModWithName("Dubs Mint Menus"))
            {
                DubsMintMenusPatches.Patches();
            } 
            else
            {
                VanillaPatches.ExclusivePatches();
            }

            VanillaPatches.Patches();
        }
    }
}