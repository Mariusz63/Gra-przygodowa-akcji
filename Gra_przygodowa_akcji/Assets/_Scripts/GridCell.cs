using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridCell : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool selected;
    public GameObject insertedItem;
    private Image looks;
    private Color _color;

    void Start()
    {
        looks = GetComponent<Image>();
        _color = looks.color;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if ( eventData.button == PointerEventData.InputButton.Left)
        {
            selected = true;
            looks.color = Color.red;
            eventData.Use();
        } else if (eventData.button == PointerEventData.InputButton.Right)
        {
            selected = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!selected) 
        {
            looks.color = Color.green;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selected)
        {
            looks.color = _color;
        }
    }
}
