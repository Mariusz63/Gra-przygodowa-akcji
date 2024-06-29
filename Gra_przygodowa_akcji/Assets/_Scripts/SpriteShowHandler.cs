using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteShowHandler : MonoBehaviour
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup group;
    public void Start()
    {
        Image img = GetComponent<Image>();
        if (img == null)
            Debug.Log("NIe ma komponentu");
        else
            Debug.Log("Znaleziono");
        img.sprite = Resources.Load<Sprite>("Prefabs/ItemsShapes/swordIconSprite");
    }

    public void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        group = GetComponent<CanvasGroup>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        group.blocksRaycasts = true;
        // Tu mo¿na dodaæ logikê, ¿eby sprawdziæ, czy przedmiot jest nad komórk¹ ekwipunku
    }

}
