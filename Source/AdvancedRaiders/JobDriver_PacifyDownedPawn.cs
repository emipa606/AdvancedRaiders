using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace AdvancedRaiders
{
    public class JobDriver_PacifyDownedPawn : JobDriver
    {
        protected Pawn Victim => TargetThingA as Pawn;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Map.reservationManager.Reserve(pawn, job, job.targetA); ;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            AddFailCondition(() => !GetActor().HasReserved(TargetA) && !GetActor().CanReserve(TargetA));
            AddEndCondition(() => TargetPacified ? JobCondition.Succeeded : JobCondition.Ongoing);
            
            yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.InteractionCell);

            yield return Toils_General.Do(Terrify);
            yield return Toils_General.Do(() => GetActor().meleeVerbs.TryMeleeAttack(TargetThingA));
        }

        protected bool TargetPacified => !Victim.Downed || Victim.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness) < 0.11;

        protected void Terrify()
        {
            if (!Victim.story.traits.HasTrait(TraitDefOf.Masochist))
            {
                var hediff = Victim.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.PacifierPTSD);
                if (hediff==null)
                {
                    hediff = Victim.health.AddHediff(AdvancedRaidersDefOf.PacifierPTSD);
                }

                hediff.Severity += 0.066f;           //TODO replace 0.066f with var
            }

            var eyewitnesses = from p in Victim.Map.mapPawns.AllPawnsSpawned
                               where
                               p.RaceProps.Humanlike &&
                               p.Position.DistanceTo(Victim.Position) < 15.0f &&                      //TODO replace 15f with var
                               (!p.Faction.HostileTo(Victim.Faction) || p.story.traits.HasTrait(TraitDefOf.Bloodlust)) &&
                               p != Victim
                               select p;

            Random rng = new Random();
            foreach (var pawn in eyewitnesses)
            {
                if (pawn.story.traits.HasTrait(TraitDefOf.Bloodlust))
                {
                    if (Rand.Value < 0.5)
                        pawn.needs.mood.thoughts.memories.TryGainMemory(AdvancedRaidersDefOf.WitnessedPacificationBloodlust);
                }

                else if (!pawn.story.traits.HasTrait(TraitDefOf.Psychopath))
                {
                    if (Rand.Value < 0.5)
                        pawn.needs.mood.thoughts.memories.TryGainMemory(AdvancedRaidersDefOf.WitnessedPacification);
                }
            }
            

        }
    }
}
