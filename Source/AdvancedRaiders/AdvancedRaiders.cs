using HarmonyLib;
using Mlie;
using UnityEngine;
using Verse;

namespace AdvancedRaiders;

public class AdvancedRaiders : Mod
{
    public static ARSettings Settings;
    public static Harmony HarmonyInstance;
    public static string CurrentVersion;

    public AdvancedRaiders(ModContentPack content) : base(content)
    {
        GetSettings<ARSettings>().Write();
        HarmonyInstance = new Harmony("saloid.AdvancedRaiders");
        CurrentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        ARSettings.DoSettingsWindowContents(inRect);
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Advanced raiders";
    }
}