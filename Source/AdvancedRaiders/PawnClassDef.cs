using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Verse.AI;

namespace AdvancedRaiders
{

    public class PawnClassDef : Def
    {
        public List<PawnKindDef> possiblePawnKinds;
        public List<JobDef> jobs;

    }
}
