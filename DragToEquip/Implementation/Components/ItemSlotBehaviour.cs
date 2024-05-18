using ChronoArkMod.Helper;
using UnityEngine.EventSystems;

namespace DragToEquip.Implementation.Components;

internal class ItemSlotBehaviour : HoverBehaviour
{
    internal required bool IsAllyTooltip { get; set; }
    private bool IsDragging => ItemObject.itemBeingDragged == null
        || ItemObject.itemBeingDragged.DragObject == null
        || ItemObject.itemBeingDragged.DragItemData == null
        || ItemObject.itemBeingDragged.DragItemData.Item == null;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (BattleSystem.instance == null) {
            SetEquipView(true);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (BattleSystem.instance == null) {
            this.StartDeferredCoroutine(() => {
                if (CurrentSlot == null && IsDragging) {
                    SetEquipView(false);
                }
            }, 0.6f);
        }
    }

    internal static void SetEquipView(bool mode)
    {
        FieldSystem.instance?.PartyWindow
            .ForEach(ally => ally.EquipViewBool = mode);
    }
}
