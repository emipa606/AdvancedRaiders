using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using RimWorld;
using UnityEngine;

namespace AdvancedRaiders
{
    public class ARSettings : ModSettings
    {

        public static float turretBugChance = 0.5f;

        public static float maxColonistsTauntedCoef = 0.4f;

        public static bool allowPacification = true;
        public static float ptsdPerHit = 0.1f;
        public static float witnessedPacificationPerHitChance = 0.5f;
        public static float witnessedPacificationRadius = 15f;
        public static float ptsdMultipiler = 1f;

        public static float soreThroatMultipiler = 1f;
        public static float inspirationMultipiler = 1f;

        public static bool allowMercMedics = true;
        public static bool allowTechnicians = true;
        public static bool allowPacifiers = true;
        public static bool allowBulldozers = true;
        public static bool allowTribalMedics = true;
        public static bool allowBeastmasters = true;
        public static bool allowCommanderChiefs = true;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref turretBugChance, "turretBugChance", 0.5f);

            Scribe_Values.Look(ref maxColonistsTauntedCoef, "maxColonistsTauntedCoef", 0.4f);

            Scribe_Values.Look(ref allowPacification, "allowPacification", true);
            Scribe_Values.Look(ref ptsdPerHit, "ptsdPerHit", 0.1f);
            Scribe_Values.Look(ref witnessedPacificationPerHitChance, "witnessedPacificationPerHitChance", 0.5f);
            Scribe_Values.Look(ref witnessedPacificationRadius, "witnessedPacificationRadius", 10f);
            Scribe_Values.Look(ref ptsdMultipiler, "ptsdMultipiler", 1f);

            Scribe_Values.Look(ref soreThroatMultipiler, "soreThroatMultipiler", 1f);
            Scribe_Values.Look(ref inspirationMultipiler, "inspirationMultipiler", 1f);

            Scribe_Values.Look(ref allowMercMedics, "allowMercMedics", true);
            Scribe_Values.Look(ref allowTechnicians, "allowPacifiers", true);
            Scribe_Values.Look(ref allowPacifiers, "allowPacifiers", true);
            Scribe_Values.Look(ref allowBulldozers, "allowBulldozers", true);
            Scribe_Values.Look(ref allowTribalMedics, "allowTribalMedics", true);
            Scribe_Values.Look(ref allowBeastmasters, "allowBeastmasters", true);
            Scribe_Values.Look(ref allowCommanderChiefs, "allowCommanderChiefs", true);
        }

        public static void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.Label("Chance for turret to turn against colony instead of breaking down: " + +Math.Round(turretBugChance * 100, 0) + "%");
            turretBugChance = listingStandard.Slider(turretBugChance, 0f, 1f);

            listingStandard.Label("Max % of colonists taunted by bulldozer (last colonist standing cant be taunted): " + Math.Round(maxColonistsTauntedCoef * 100, 0) + "%");
            maxColonistsTauntedCoef = listingStandard.Slider(maxColonistsTauntedCoef, 0f, 1f);

            listingStandard.CheckboxLabeled("Allow pacifiers to beat downed pawns: ", ref allowPacification, "");

            listingStandard.Label("PTSD severety per pacifier hit: " + Math.Round(ptsdPerHit, 2));
            ptsdPerHit = listingStandard.Slider(ptsdPerHit, 0f, 1f);

            listingStandard.Label("Chance for witness to get \"witnessed pacification\" memory per pacifier hit: " + Math.Round(witnessedPacificationPerHitChance * 100, 0) + "%");
            witnessedPacificationPerHitChance = listingStandard.Slider(witnessedPacificationPerHitChance, 0f, 1f);

            listingStandard.Label("Pacification observation radius (pawns with no LOS for victim won't get memories): " + Math.Round(witnessedPacificationRadius));
            witnessedPacificationRadius = listingStandard.Slider(witnessedPacificationRadius, 1f, 100f);

            listingStandard.Label("PTSD mood debuff multipiler: " + Math.Round(ptsdMultipiler * 100, 0) + "%");
            ptsdMultipiler = listingStandard.Slider(ptsdMultipiler, 0f, 2f);

            listingStandard.Label("Inspiration strength multipiler: " + Math.Round(inspirationMultipiler, 2));
            inspirationMultipiler = listingStandard.Slider(inspirationMultipiler, 0f, 5f);

            listingStandard.Label("Sore throat debuff multipiler (more -> less inspiration casts): " + Math.Round(soreThroatMultipiler, 2));
            soreThroatMultipiler = listingStandard.Slider(soreThroatMultipiler, 0f, 5f);

            listingStandard.CheckboxLabeled("Enable mercenary medics spawn", ref allowMercMedics);
            listingStandard.CheckboxLabeled("Enable technicians spawn", ref allowTechnicians);
            listingStandard.CheckboxLabeled("Enable pacifiers spawn", ref allowPacifiers);
            listingStandard.CheckboxLabeled("Enable bulldozers spawn", ref allowBulldozers);
            listingStandard.CheckboxLabeled("Enable tribal medics spawn", ref allowTribalMedics);
            listingStandard.CheckboxLabeled("Enable beastmaster spawn", ref allowBeastmasters);
            listingStandard.CheckboxLabeled("Enable commander chief spawn", ref allowCommanderChiefs);
            listingStandard.End();

            
        }
    }
}
