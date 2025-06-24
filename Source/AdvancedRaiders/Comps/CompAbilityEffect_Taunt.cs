using RimWorld;
using Verse;

namespace AdvancedRaiders;

public class CompAbilityEffect_Taunt : CompAbilityEffect
{
    private new CompProperties_AbilityTaunt Props => (CompProperties_AbilityTaunt)props;

    public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
    {
        if (target.Pawn == null)
        {
            return;
        }

        //base.Apply(target, dest);     //TODO make base apply work properly
        if (!tryTauntPawn(target.Pawn))
        {
            Log.Warning($"Failed to taunt {target.Pawn.Name}");
        }
    }


    public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
    {
        var pawn = target.Pawn;
        return pawn != null && pawn.MentalStateDef != AdvancedRaidersDefOf.MurderousRageTaunted;
    }

    public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
    {
        return Valid(target);
    }

    private bool tryTauntPawn(Pawn target)
    {
        if (!target.mindState.mentalStateHandler.TryStartMentalState(AdvancedRaidersDefOf.MurderousRageTaunted))
        {
            return false;
        }

        var mentalState = (MentalState_MurderousRageTaunted)target.MentalState;
        mentalState.target = parent.pawn;
        mentalState.forceRecoverAfterTicks = Props.durationTicks;
        return true;
    }
}