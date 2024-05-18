using ChronoArkMod.Plugin;
using DragToCast.Implementation.Patches;
using DragToEquip.Api;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace DragToEquip;

#nullable enable

public class DragToEquipMod : ChronoArkPlugin
{
    private static DragToEquipMod? _instance;
    private readonly List<IPatch> _patches = [];

    public static DragToEquipMod? Instance => _instance;
    internal Harmony? _harmony;

    public override void Dispose()
    {
        _instance = null;
    }

    public override void Initialize()
    {
        _instance = this;
        _harmony = new(GetGuid());

        _patches.Add(new InventoryManagerPatch());
        _patches.Add(new AllyWindowPatch());
        _patches.Add(new ItemObjectPatch());

        foreach (var patch in _patches) {
            if (patch.Mandatory) {
                Debug.Log($"patching {patch.Name}");
                patch.Commit();
                Debug.Log("success!");
            }
        }
    }
}
