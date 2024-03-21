using RimWorld;
using Verse;

namespace AdvancedRaiders;

public class CompReinforcementsSummoner : ThingComp
{
    private int _ageTicks;
    private IncidentParms parms;
    public int reinforcementsPoints = 250;

    public CompProperties_ReinforcementsSummoner Props => props as CompProperties_ReinforcementsSummoner;

    public override void CompTick()
    {
        base.CompTick();
        _ageTicks++; //TODO make age increase less frequent
        if (_ageTicks > Props.reinforcementsDelay)
        {
        }
    }

    private void CallReinforcements()
    {
    }
}