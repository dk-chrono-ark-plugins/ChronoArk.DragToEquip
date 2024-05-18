using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DragToEquip.Implementation.Components;

#nullable enable

internal class HoverBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static readonly List<HoverBehaviour> _hoverHierarchy = [];
    internal static HoverBehaviour? CurrentSlot
    {
        get
        {
            _hoverHierarchy.RemoveAll(go => go == null);
            return _hoverHierarchy.LastOrDefault();
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Enter(this);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Exit(this);
    }

    protected static void Enter(HoverBehaviour ho)
    {
        _hoverHierarchy.Add(ho);
    }

    protected static void Exit(HoverBehaviour ho)
    {
        _hoverHierarchy.Remove(ho);
    }
}
