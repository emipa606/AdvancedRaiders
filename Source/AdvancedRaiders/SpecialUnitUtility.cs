using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Verse.AI.Group;
using Verse.AI;

namespace AdvancedRaiders
{
    public enum PawnClass
    {
        None,
        MercenaryMedic,
        MercenaryTechnician,
        MercenaryPacifier,
        MercenaryBulldozer,
        TribalMedic,
        TribalBeastmaster,
        TribalInspirer
    }

    public static class SpecialUnitUtility
    {
        public static void GenBeastmasterPetsAndRelations(Pawn beastmaster)
        {
            if (beastmaster.relations.GetDirectRelationsCount(PawnRelationDefOf.Bond) > 0)      //avoiding animal spam
                return;

            Random rng = new Random();
            PawnKindDef kindDef;
            int beastNum;
            List<Pawn> beasts = new List<Pawn>();

            //Adding presets from xml-file here would be a good decision, but me dumb me no understand how to.
            switch (rng.Next(0, 5))
            {
                case 0:
                    //A man and a bear.
                    //story by saloid

                    kindDef = DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef def) => def.defName == "Bear_Grizzly").First();
                    if (kindDef == null)
                    {
                        Log.Error("No grizzly def found at GenBeastmasterPetsAndRelations. Weird.");
                        return;
                    }
                    beastNum = 1;
                    break;

                case 1:

                    kindDef = DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef def) => def.defName == "Wolf_Timber").First();
                    if (kindDef == null)
                    {
                        Log.Error("No Wolf_Timber def found at GenBeastmasterPetsAndRelations.");
                        return;
                    }
                    beastNum = 2;
                    break;

                case 2:

                    kindDef = DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef def) => def.defName == "Warg").First();
                    if (kindDef == null)
                    {
                        Log.Error("No warg def found at GenBeastmasterPetsAndRelations.");
                        return;
                    }
                    beastNum = 1;
                    break;

                case 3:

                    kindDef = DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef def) => def.defName == "Cougar").First();
                    if (kindDef == null)
                    {
                        Log.Error("No cougar def found at GenBeastmasterPetsAndRelations.");
                        return;
                    }
                    beastNum = 2;
                    break;

                case 4:

                    kindDef = DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef def) => def.defName == "Boomrat").First();
                    if (kindDef == null)
                    {
                        Log.Error("No boomrat def found at GenBeastmasterPetsAndRelations.");
                        return;
                    }
                    beastNum = 4;
                    break;

                default:
                    //this sholdnt happen
                    return;
            }


            for (int i = 0; i < beastNum; i++)
            {
                var newBeast = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kindDef, beastmaster.Faction));
                beasts.Add(newBeast);
                GenSpawn.Spawn(newBeast, CellFinder.RandomSpawnCellForPawnNear(beastmaster.Position, beastmaster.Map), beastmaster.Map, WipeMode.VanishOrMoveAside);
                beastmaster.relations.AddDirectRelation(PawnRelationDefOf.Bond, newBeast);

                newBeast.training.Train(TrainableDefOf.Tameness, beastmaster, true);
                newBeast.training.Train(TrainableDefOf.Obedience, beastmaster, true);
                newBeast.training.Train(TrainableDefOf.Release, beastmaster, true);
            }
        }

        public static void AddPetsToBeastmasterLord(Pawn beastmaster)
        {
            Lord lord = beastmaster.GetLord();
            if (lord == null)
                return;

            foreach (var relation in beastmaster.relations.DirectRelations)
            {
                if (relation.def == PawnRelationDefOf.Bond)
                {
                    Pawn beast = relation.otherPawn;
                    Lord beastLord = beast.GetLord();

                    if (beastLord == beastmaster.GetLord())
                        return;

                    if (beastLord != null)
                        beastLord.RemovePawn(beast);

                    lord.AddPawn(beast);
                }
            }
        }

        public static void MakeUkuphilaZombie(Pawn pawn)
        {
            if (!pawn.Dead)
            {
                Log.Error("Attempted to make ukuphila zombie from live pawn");
                return;
            }

            if (!pawn.RaceProps.Humanlike)
            {
                Log.Error("Attempt to make ukuphila zombie from non-humanlike");
                return;
            }

            ResurrectionUtility.Resurrect(pawn);
            pawn.health.AddHediff(AdvancedRaidersDefOf.UkuphilaResurrection);

            if (pawn.Dead)
                Log.Error("Looks like even forbidden herbs couldn't revive " + pawn.Name + ". Press F.");

            //avoiding lord spam
            pawn.GetLord().RemovePawn(pawn);
        }

        public static bool TooManyColonistsTaunted(Map map)
        {
            int taunted = 0;
            int total = 0;
            foreach (var colonist in map.mapPawns.FreeColonistsSpawned.Where((Pawn p) => p.health.State == PawnHealthState.Mobile))
            {
                total++;
                if (colonist.MentalStateDef == AdvancedRaidersDefOf.MurderousRageTaunted)
                    taunted++;
            }
            if (total < 2)
                return true;

            return ((double)taunted / total) > ARSettings.maxColonistsTauntedCoef;
        }

        public static bool AtLeastNAlliesInInspireRadius(int nPawns, Pawn caster)
        {
            Ability ability = caster.abilities.GetAbility(AdvancedRaidersDefOf.InspireAlliesAbility);
            if (ability == null)
                return false;

            float radius = ((CompAbilityEffect_InspireAllies)ability.comps.Find((AbilityComp c) => c is CompAbilityEffect_InspireAllies)).Props.radius;
            var pawnsInRadius =
                from p in caster.Map.mapPawns.FreeColonistsSpawned
                where p.Position.DistanceTo(caster.Position) < radius &&
                p.health.State == PawnHealthState.Mobile &&
                p.Faction == caster.Faction
                select p;

            return pawnsInRadius.Count() - 1 >= nPawns;       //-1 for caster themself
        }

        public static bool IsAllowedAdvancedRaiderClass(PawnClass unitClass)  
        {
            switch(unitClass)
            {
                case PawnClass.None:
                    return false;
                case PawnClass.MercenaryMedic:
                    return ARSettings.allowMercMedics;
                case PawnClass.MercenaryTechnician:
                    return ARSettings.allowTechnicians;
                case PawnClass.MercenaryPacifier:
                    return ARSettings.allowPacifiers;
                case PawnClass.MercenaryBulldozer:
                    return ARSettings.allowBulldozers;
                case PawnClass.TribalMedic:
                    return ARSettings.allowTribalMedics;
                case PawnClass.TribalBeastmaster:
                    return ARSettings.allowBeastmasters;
                case PawnClass.TribalInspirer:
                    return ARSettings.allowCommanderChiefs;
                default:
                    return false;
            }
        }

        public static PawnClass AdvancedRaiderClass(Pawn p) => AdvancedRaiderKindDefClass(p.kindDef);
        public static PawnClass AdvancedRaiderKindDefClass(PawnKindDef def)
        {

            if (def == AdvancedRaidersDefOf.Mercenary_MedicRanged || def == AdvancedRaidersDefOf.Mercenary_MedicMelee)
                return PawnClass.MercenaryMedic;

            if (def == AdvancedRaidersDefOf.Mercenary_Technician)
                return PawnClass.MercenaryTechnician;

            if (def == AdvancedRaidersDefOf.Mercenary_Bulldozer)
                return PawnClass.MercenaryBulldozer;

            if (def == AdvancedRaidersDefOf.MercenaryPacifier_Bloodlust || def == AdvancedRaidersDefOf.MercenaryPacifier_Psychopath)
                return PawnClass.MercenaryPacifier;

            if (def == AdvancedRaidersDefOf.Tribal_MedicRanged || def == AdvancedRaidersDefOf.Tribal_MedicMelee)
                return PawnClass.TribalMedic;

            if (def == AdvancedRaidersDefOf.Tribal_Beastmaster)
                return PawnClass.TribalBeastmaster;

            if (def == AdvancedRaidersDefOf.Tribal_ChiefCommanderRanged || def == AdvancedRaidersDefOf.Tribal_ChiefCommanderMelee)
                return PawnClass.TribalInspirer;

            return PawnClass.None;
        }

        public static bool IsARAndMeleeFighter(PawnKindDef def)
        {
            if (def == AdvancedRaidersDefOf.MercenaryPacifier_Bloodlust ||
                def == AdvancedRaidersDefOf.MercenaryPacifier_Psychopath ||
                def == AdvancedRaidersDefOf.Mercenary_MedicMelee ||
                def == AdvancedRaidersDefOf.Tribal_MedicMelee ||
                def == AdvancedRaidersDefOf.Tribal_ChiefCommanderMelee)
            {
                return true;
            }

            return false;
        }
    }
}
