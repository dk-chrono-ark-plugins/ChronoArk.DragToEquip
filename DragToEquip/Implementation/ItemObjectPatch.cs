using ChronoArkMod.Helper;
using DragToEquip;
using DragToEquip.Api;
using HarmonyLib;
using UnityEngine;

namespace DragToCast.Implementation.Patches;

#nullable enable

internal class ItemObjectPatch : IPatch
{
    private sealed record SharedCanvas(bool IsCharInv);
    private static Canvas? _sharedCanvas;
    private static int _currentCanvasOrder = -1;

    public string Id => "item-object";
    public string Name => Id;
    public string Description => Id;
    public bool Mandatory => true;

    public void Commit()
    {
        var harmony = DragToEquipMod.Instance!._harmony!;
        harmony.Patch(
            original: AccessTools.Method(
                typeof(ItemObject),
                nameof(ItemObject.OnBeginDrag)
            ),
            prefix: new(typeof(ItemObjectPatch), nameof(OnBeginDragPrep)),
            postfix: new(typeof(ItemObjectPatch), nameof(OnBeginDrag))
        );
    }

    private static void OnBeginDragPrep(ItemObject __instance, out SharedCanvas __state)
    {
        __state = new(__instance.MyManager is CharEquipInven);
    }

    private static void OnBeginDrag(ItemObject __instance, SharedCanvas __state)
    {
        if (__state.IsCharInv) {
            _sharedCanvas = Misc.FindCanvas(__instance.transform).GetComponent<Canvas>();
            if (_sharedCanvas is not null) {
                _currentCanvasOrder = _sharedCanvas.sortingOrder;
                _sharedCanvas.sortingOrder = UIManager.inst!.MainCanvas.GetComponent<Canvas>().sortingOrder;
                UIManager.inst!.StartDeferredCoroutine(() => {
                    _sharedCanvas.sortingOrder = _currentCanvasOrder;
                }, () => Input.GetMouseButtonDown(0));
            }
        }
    }
}
