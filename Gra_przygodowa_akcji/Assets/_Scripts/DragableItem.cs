using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image img;
    public Transform m_Transform;

    public void Awake()
    {
        img = GetComponentInChildren<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_Transform = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        img.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(m_Transform);
        img.raycastTarget = true;
    }
}
