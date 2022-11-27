using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace AdvancedRaiders;

public class CompAbilityEffect_InspireAllies : CompAbilityEffect
{
    public new CompProperties_AbilityInspireAllies Props => props as CompProperties_AbilityInspireAllies;

    public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
    {
        InspireAlliesInRange(parent.pawn.Position);
        GiveSoreThroatHediff(parent.pawn);
    }

    public override void Apply(GlobalTargetInfo target)
    {
        Apply(null, null);
        //rewrite later
    }

    private void InspireAlliesInRange(LocalTargetInfo center)
    {
        var alliesInRange =
            from p in parent.pawn.Map.mapPawns.FreeHumanlikesSpawnedOfFaction(parent.pawn.Faction)
            where center.Cell.DistanceTo(p.Position) <= Props.radius
            select p;

        foreach (var pawn in alliesInRange)
        {
            InspirePawn(pawn);
        }
    }

    public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
    {
        var hediff = parent.pawn.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.SoreThroat);
        return hediff is not { Visible: true };
    }

    public override bool CanApplyOn(GlobalTargetInfo target)
    {
        return CanApplyOn(null, null);
    }

    private void InspirePawn(Pawn pawn)
    {
        var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.InspirationHediff);
        if (hediff == null)
        {
            hediff = pawn.health.AddHediff(AdvancedRaidersDefOf.InspirationHediff);
        }

        hediff.Severity += Props.inspirationStrength * InspirationMultipiler(pawn) * ARSettings.inspirationMultipiler;
    }

    private float InspirationMultipiler(Pawn pawn)
    {
        var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.InspirationHediff);

        if (hediff == null)
        {
            return 1f;
        }

        return hediff.Severity > 1 ? 0 : 1 - hediff.Severity;
    }

    private void GiveSoreThroatHediff(Pawn pawn)
    {
        var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.SoreThroat);
        if (hediff == null)
        {
            hediff = pawn.health.AddHediff(AdvancedRaidersDefOf.SoreThroat);
        }

        hediff.Severity += 0.1f * Props.soreThroatEffectFactor * ARSettings.soreThroatMultipiler;
    }
}