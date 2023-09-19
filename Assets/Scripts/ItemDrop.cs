using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    private void Awake()
    {
    }

    public ItemDrop AssignItem(ItemStack item)
    {
        this.itemStack = item;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.sprite = item.item.icon;

        Vector2 size = GetComponent<SpriteRenderer>().sprite.bounds.size;
        Debug.Log("Sprite bounds: " + size);
        GetComponent<BoxCollider2D>().size = size;
        Debug.Log("ItemDrop spawned at " + transform.position);
        return this;
    }

    public ItemStack itemStack;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if (player)
        {
            itemStack = JQUI.InventoryController.inventory.addItemToInventory(getStoredDrop());
            JQUI.InventoryController.updateDisplay();
            if (itemStack == null) Destroy(gameObject);
        }
    }

    public virtual ItemStack getStoredDrop()
    {
        return itemStack;
    }
}
