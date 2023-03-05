using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    private void Awake()
    {
        Rect re = GetComponent<SpriteRenderer>().sprite.rect;
        Vector2 size = re.max - re.min;
        Debug.Log("Sprite bounds: " + size);
        GetComponent<BoxCollider2D>().size = size;
        Debug.Log("ItemDrop spawned at " + transform.position);
    }

    public void AssignItem(ItemStack item)
    {
        this.itemStack = item;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.sprite = item.item.icon;
    }

    public ItemStack itemStack;
}
