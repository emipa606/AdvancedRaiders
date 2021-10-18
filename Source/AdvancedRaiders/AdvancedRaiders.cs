using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using HarmonyLib;
using UnityStandardAssets;

namespace AdvancedRaiders
{
    [StaticConstructorOnStartup]
    public class AdvancedRaiders : Mod
    {
        public static ARSettings settings;
        public static Harmony harmonyInstance;
        public AdvancedRaiders(ModContentPack content) : base(content)
        {
            GetSettings<ARSettings>().Write();
            harmonyInstance = new Harmony("saloid.AdvancedRaiders");
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            ARSettings.DoSettingsWindowContents(inRect);
            base.DoSettingsWindowContents(inRect);
            
        }
        public override string SettingsCategory() => "Advanced raiders";
    }
}
