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
        inspireAlliesInRange(parent.pawn.Position);
        giveSoreThroatHediff(parent.pawn);
    }

    public override void Apply(GlobalTargetInfo target)
    {
        Apply(null, null);
        //rewrite later
    }

    private void inspireAlliesInRange(LocalTargetInfo center)
    {
        var alliesInRange =
            from p in parent.pawn.Map.mapPawns.FreeHumanlikesSpawnedOfFaction(parent.pawn.Faction)
            where center.Cell.DistanceTo(p.Position) <= Props.radius
            select p;

        foreach (var pawn in alliesInRange)
        {
            inspirePawn(pawn);
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

    private void inspirePawn(Pawn pawn)
    {
        var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.InspirationHediff) ??
                     pawn.health.AddHediff(AdvancedRaidersDefOf.InspirationHediff);

        hediff.Severity += Props.inspirationStrength * inspirationMultiplier(pawn) * ARSettings.InspirationMultiplier;
    }

    private static float inspirationMultiplier(Pawn pawn)
    {
        var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.InspirationHediff);

        if (hediff == null)
        {
            return 1f;
        }

        return hediff.Severity > 1 ? 0 : 1 - hediff.Severity;
    }

    private void giveSoreThroatHediff(Pawn pawn)
    {
        var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(AdvancedRaidersDefOf.SoreThroat) ??
                     pawn.health.AddHediff(AdvancedRaidersDefOf.SoreThroat);

        hediff.Severity += 0.1f * Props.soreThroatEffectFactor * ARSettings.SoreThroatMultiplier;
    }
}