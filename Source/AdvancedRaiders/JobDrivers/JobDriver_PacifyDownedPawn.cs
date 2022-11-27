using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace AdvancedRaiders;

public class JobDriver_PacifyDownedPawn : JobDriver
{
    protected Pawn Victim => TargetThingA as Pawn;

    protected bool TargetPacified =>
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

        yield return Toils_General.Do(Terrify);
        yield return Toils_General.Do(() => GetActor().meleeVerbs.TryMeleeAttack(TargetThingA));
    }

    protected void Terrify()
    {
        if (!Victim.story.traits.HasTrait(TraitDefOf.Masochist))
        {
            var hediff = Victim.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.PacifierPTSD);
            if (hediff == null)
            {
                hediff = Victim.health.AddHediff(AdvancedRaidersDefOf.PacifierPTSD);
            }

            hediff.Severity += ARSettings.ptsdPerHit;
        }

        var eyewitnesses = from p in Victim.Map.mapPawns.AllPawnsSpawned
            where
                p.RaceProps.Humanlike &&
                p.Position.DistanceTo(Victim.Position) < ARSettings.witnessedPacificationRadius &&
                (!p.Faction.HostileTo(Victim.Faction) || p.story.traits.HasTrait(TraitDefOf.Bloodlust)) &&
                p != Victim &&
                p.CanSee(TargetThingA)
            select p;

        foreach (var witness in eyewitnesses)
        {
            if (witness.story.traits.HasTrait(TraitDefOf.Bloodlust))
            {
                if (Rand.Value < ARSettings.witnessedPacificationPerHitChance)
                {
                    witness.needs.mood.thoughts.memories.TryGainMemory(AdvancedRaidersDefOf
                        .WitnessedPacificationBloodlust);
                }
            }

            else if (!witness.story.traits.HasTrait(TraitDefOf.Psychopath))
            {
                if (Rand.Value < ARSettings.witnessedPacificationPerHitChance)
                {
                    witness.needs.mood.thoughts.memories.TryGainMemory(AdvancedRaidersDefOf.WitnessedPacification);
                }
            }
        }
    }
}