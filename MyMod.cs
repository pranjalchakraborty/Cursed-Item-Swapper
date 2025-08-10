// Copyright (C)
// See LICENSE file for extended copyright information.

using ModShardLauncher;
using ModShardLauncher.Mods;

namespace CursedItemSwapper;
public class CursedItemSwapper : Mod
{
    public override string Author => "Aracor";
    public override string Name => "Cursed Item Swapper";
    public override string Description => "Allows cursed items to be swapped with items in containers and on the ground.";
    public override string Version => "1.0.0";
    public override string TargetVersion => "0.8.2.10";

    public override void PatchMod()
    {
        // Add a simple menu option to toggle the feature on or off.
        Msl.AddMenu("Cursed Item Swapper", new UIComponent[] {
            new(name: "Allow Cursed Swapping?", associatedGlobal: "CIS_AllowCursedSwap", UIComponentType.Slider, (0, 1), 1) // 0=No, 1=Yes
        });

        // --- Patch to Allow Cursed Item Swapping ---
        // This finds the exact line that prevents swapping and adds our global variable as an exception.
        string originalSwapLogic = "if (can_pick && !other.wound_block && !ds_map_find_value(data, \"is_cursed\"))";
        string newSwapLogic = "if (can_pick && !other.wound_block && (!ds_map_find_value(data, \"is_cursed\") || global.CIS_AllowCursedSwap == 1))";

        Msl.GetStringGMLFromFile("gml_Object_o_inv_weapon_slot_Mouse_5").Peek();

        Msl.LoadGML("gml_Object_o_inv_weapon_slot_Mouse_5")
           .MatchFrom(originalSwapLogic)
           .ReplaceBy(newSwapLogic)
           .Save();
    }
}