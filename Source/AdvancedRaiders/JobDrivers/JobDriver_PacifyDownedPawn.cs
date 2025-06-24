using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace AdvancedRaiders;

public class JobDriver_PacifyDownedPawn : JobDriver
{
    private Pawn Victim => TargetThingA as Pawn;

    private bool TargetPacified =>
        !Victim.Downed || Victim.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness) < 0.11;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Map.reservationManager.Reserve(pawn, job, job.targetA);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
        AddFailCondition(() => !GetActor().HasReserved(TargetA) && !GetActor().CanReserve(TargetA));
        AddEndCondition(() => TargetPacified ? JobCondition.Succeeded : JobCondition.Ongoing);

        yield return Toils_Goto.Goto(TargetIndex.A, PathEndMode.InteractionCell);

        yield return Toils_General.Do(terrify);
        yield return Toils_General.Do(() => GetActor().meleeVerbs.TryMeleeAttack(TargetThingA));
    }

    private void terrify()
    {
        if (!Victim.story.traits.HasTrait(AdvancedRaidersDefOf.Masochist))
        {
            var hediff = Victim.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.PacifierPTSD) ??
                         Victim.health.AddHediff(AdvancedRaidersDefOf.PacifierPTSD);

            hediff.Severity += ARSettings.PtsdPerHit;
        }

        var eyewitnesses = from p in Victim.Map.mapPawns.AllPawnsSpawned
            where
                p.RaceProps.Humanlike &&
                p.Position.DistanceTo(Victim.Position) < ARSettings.WitnessedPacificationRadius &&
                (!p.Faction.HostileTo(Victim.Faction) || p.story.traits.HasTrait(TraitDefOf.Bloodlust)) &&
                p != Victim &&
                p.CanSee(TargetThingA)
            select p;

        foreach (var witness in eyewitnesses)
        {
            if (witness.story.traits.HasTrait(TraitDefOf.Bloodlust))
            {
                if (Rand.Value < ARSettings.WitnessedPacificationPerHitChance)
                {
                    witness.needs.mood.thoughts.memories.TryGainMemory(AdvancedRaidersDefOf
                        .WitnessedPacificationBloodlust);
                }
            }

            else if (!witness.story.traits.HasTrait(TraitDefOf.Psychopath))
            {
                if (Rand.Value < ARSettings.WitnessedPacificationPerHitChance)
                {
                    witness.needs.mood.thoughts.memories.TryGainMemory(AdvancedRaidersDefOf.WitnessedPacification);
                }
            }
        }
    }
}