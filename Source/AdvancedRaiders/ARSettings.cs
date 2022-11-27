using System;
using UnityEngine;
using Verse;

namespace AdvancedRaiders;

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
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);

        listingStandard.Label("AdRa.turretBugChance".Translate(turretBugChance.ToStringPercent()));
        turretBugChance = listingStandard.Slider(turretBugChance, 0f, 1f);

        listingStandard.Label("AdRa.maxColonistsTauntedCoef".Translate(maxColonistsTauntedCoef.ToStringPercent()));
        maxColonistsTauntedCoef = listingStandard.Slider(maxColonistsTauntedCoef, 0f, 1f);

        listingStandard.Label("AdRa.ptsdPerHit".Translate(Math.Round(ptsdPerHit, 2)));
        ptsdPerHit = listingStandard.Slider(ptsdPerHit, 0f, 1f);

        listingStandard.Label(
            "AdRa.witnessedPacificationPerHitChance".Translate(witnessedPacificationPerHitChance.ToStringPercent()));
        witnessedPacificationPerHitChance = listingStandard.Slider(witnessedPacificationPerHitChance, 0f, 1f);

        listingStandard.Label(
            "AdRa.witnessedPacificationPerHitChance".Translate(Math.Round(witnessedPacificationRadius)));
        witnessedPacificationRadius = listingStandard.Slider(witnessedPacificationRadius, 1f, 100f);

        listingStandard.Label("AdRa.ptsdMultipiler".Translate(ptsdMultipiler.ToStringPercent()));
        ptsdMultipiler = listingStandard.Slider(ptsdMultipiler, 0f, 2f);

        listingStandard.Label("AdRa.inspirationMultipiler".Translate(inspirationMultipiler.ToStringPercent()));
        inspirationMultipiler = listingStandard.Slider(inspirationMultipiler, 0f, 5f);

        listingStandard.Label("AdRa.soreThroatMultipiler".Translate(Math.Round(soreThroatMultipiler, 2)));
        soreThroatMultipiler = listingStandard.Slider(soreThroatMultipiler, 0f, 5f);

        listingStandard.CheckboxLabeled("AdRa.allowPacification".Translate(), ref allowPacification);
        listingStandard.CheckboxLabeled("AdRa.allowMercMedics".Translate(), ref allowMercMedics);
        listingStandard.CheckboxLabeled("AdRa.allowTechnicians".Translate(), ref allowTechnicians);
        listingStandard.CheckboxLabeled("AdRa.allowPacifiers".Translate(), ref allowPacifiers);
        listingStandard.CheckboxLabeled("AdRa.allowBulldozers".Translate(), ref allowBulldozers);
        listingStandard.CheckboxLabeled("AdRa.allowTribalMedics".Translate(), ref allowTribalMedics);
        listingStandard.CheckboxLabeled("AdRa.allowBeastmasters".Translate(), ref allowBeastmasters);
        listingStandard.CheckboxLabeled("AdRa.allowCommanderChiefs".Translate(), ref allowCommanderChiefs);
        if (AdvancedRaiders.currentVersion != null)
        {
            GUI.contentColor = Color.gray;
            listingStandard.Label("AdRa.modVersion".Translate(AdvancedRaiders.currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }
}