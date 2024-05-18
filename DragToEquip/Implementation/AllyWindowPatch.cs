using ChronoArkMod.Helper;
using DragToEquip;
using DragToEquip.Api;
using DragToEquip.Implementation.Components;
using HarmonyLib;

namespace DragToCast.Implementation.Patches;

#nullable enable

internal class AllyWindowPatch : IPatch
{
    public string Id => "ally-window";
    public string Name => Id;
    public string Description => Id;
    public bool Mandatory => true;

    public void Commit()
    {
        var harmony = DragToEquipMod.Instance!._harmony!;
        harmony.Patch(
            original: AccessTools.Method(
                typeof(AllyWindow),
                "Start"
            ),
            postfix: new(typeof(AllyWindowPatch), nameof(OnStart))
        );
    }

    private static void OnStart(AllyWindow __instance)
    {
        __instance.stooltip.gameObject.GetOrAddComponent<ItemSlotBehaviour>().IsAllyTooltip = true;
    }
}
