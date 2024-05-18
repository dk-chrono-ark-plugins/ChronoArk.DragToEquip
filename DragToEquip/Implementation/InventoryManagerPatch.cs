using ChronoArkMod.Helper;
using DragToEquip;
using DragToEquip.Api;
using DragToEquip.Implementation.Components;
using HarmonyLib;
using System.Collections.Generic;

namespace DragToCast.Implementation.Patches;

#nullable enable

internal class InventoryManagerPatch : IPatch
{
    public string Id => "inventory-manager";
    public string Name => Id;
    public string Description => Id;
    public bool Mandatory => true;

    public void Commit()
    {
        var harmony = DragToEquipMod.Instance!._harmony!;
        harmony.Patch(
            original: AccessTools.Method(
                typeof(InventoryManager),
                nameof(InventoryManager.CreateInven),
                [
                    typeof(List<ItemBase>),
                    typeof(bool),
                    typeof(bool),
                ]
            ),
            postfix: new(typeof(InventoryManagerPatch), nameof(OnInventoryInstantiated))
        );
    }

    private static void OnInventoryInstantiated(InventoryManager __instance)
    {
        __instance.Align?.transform
            .GetAllChildren()
            .Do(item => item.gameObject.GetOrAddComponent<ItemSlotBehaviour>());
    }
}
