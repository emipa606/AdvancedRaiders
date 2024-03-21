using Verse.AI.Group;

namespace AdvancedRaiders;

public static class RaidExtentions
{
    public static bool OwnsAnyInspirers(this Lord lord)
    {
        foreach (var pawn in lord.ownedPawns)
        {
            if (pawn.kindDef == AdvancedRaidersDefOf.Tribal_ChiefCommanderRanged)
            {
                return true;
            }
        }

        return false;
    }
}