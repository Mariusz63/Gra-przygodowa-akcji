using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteShowHandler : MonoBehaviour
{
    public void Start()
    {
        Image img = GetComponent<Image>();
        if (img == null)
            Debug.Log("NIe ma komponentu");
        else
            Debug.Log("Znaleziono");
        img.sprite = Resources.Load<Sprite>("Prefabs/ItemsShapes/swordIconSprite");
    }

}
