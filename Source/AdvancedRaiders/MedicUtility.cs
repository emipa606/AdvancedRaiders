using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AdvancedRaiders
{
    public static class MedicUtility
    {
        public static ThingDef ActionRelatedThingDef(FirstAidAction action)
        {
            switch (action) 
            {
                case FirstAidAction.OmegaStimulantShot:
                    return AdvancedRaidersDefOf.OmegaStimulant;
                default:
                    return null;
            }

        }

        public static FirstAidAction ThingRelatedFirstAidAction(Thing thing) => ThingDefRelatedFirstAidAction(thing.def);
        public static FirstAidAction ThingDefRelatedFirstAidAction(ThingDef def)
        {
            
            if(def == AdvancedRaidersDefOf.OmegaStimulant)
                return FirstAidAction.OmegaStimulantShot;

            return FirstAidAction.None;
        }
    }
}
