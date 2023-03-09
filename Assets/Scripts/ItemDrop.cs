using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    private void Awake()
    {
    }

    public void AssignItem(ItemStack item)
    {
        this.itemStack = item;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.sprite = item.item.icon;

        Vector2 size = GetComponent<SpriteRenderer>().sprite.bounds.size;
        Debug.Log("Sprite bounds: " + size);
        GetComponent<BoxCollider2D>().size = size;
        Debug.Log("ItemDrop spawned at " + transform.position); 
    }

    public ItemStack itemStack;
}
