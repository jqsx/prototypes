using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JQUI
{

    public class InventoryController : MonoBehaviour
    {
        public static InventoryController instance;
        public int size = 15;
        public static Inventory inventory;
        Animator animator;
        bool isOpen = false;
        public Transform visualSlotParent;
        public int currentlySelected = -1;
        public GameObject prefab_slot;
        public ItemStack cursor = null;
        public Sprite noitem;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            animator.Play("Close");
            instance = this;
            if (inventory == null)
            {
                inventory = new Inventory(size);
            }
            generateInventory();
        }

        void generateInventory()
        {
            for (int i = 0; i < visualSlotParent.childCount; i++)
            {
                Destroy(visualSlotParent.GetChild(i).gameObject);
            }

            for(int i = 0; i < size; i++)
            {
                Slot slot = generateNewSlotObject();
                slot.slotNumber = i;
                slot.parent = inventory;
            }
            updateInventoryDisplay();
        }

        Slot generateNewSlotObject()
        {
            GameObject slt = Instantiate(prefab_slot);
            slt.transform.parent = visualSlotParent;
            return slt.GetComponent<Slot>();
        }

        // Update is called once per frame
        void Update()
        {
            if (instance == null) instance = this;
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (isOpen) animator.Play("Close");
                else animator.Play("Open");
                isOpen = !isOpen;
                if (!isOpen)
                {
                    
                }
            }
        }

        public void updateInventoryDisplay()
        {
            for (int i = 0; i < visualSlotParent.childCount; i++)
            {
                ItemStack slot = inventory.slots[i];
                Sprite icon = noitem;
                if (slot != null) icon = slot.item.icon;
                visualSlotParent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = icon;
            }
        }

        public static void updateDisplay()
        {
            instance.updateInventoryDisplay();
        }

        public void slotClicked(Slot self)
        {
            if (currentlySelected == self.slotNumber) currentlySelected = -1;
            else if (currentlySelected != -1)
            {
                inventory.swapSlots(currentlySelected, self.parent, self.slotNumber);
                currentlySelected = -1;
                updateInventoryDisplay();
            }
            else
            {
                currentlySelected = self.slotNumber;
            }
        }
    }
}

[System.Serializable]
public class Inventory
{
    private static List<Inventory> openInventories = new List<Inventory>();
    public ItemStack[] slots;

    public Inventory(int size)
    {
        openInventories.Add(this);
        slots = new ItemStack[size];
    }

    public void swapSlots(int local, Inventory other, int otherslot)
    {
        ItemStack lcl = slots[local];
        ItemStack othr = other.slots[otherslot];

        slots[local] = othr;
        other.slots[otherslot] = lcl;
    }

    public void drop(int index)
    {
        onDrop(index);
        if (slots[index] == null) return;
        PlayerController p = PlayerController.instance;
        GAMEINITIALIZER.SpawnItem(slots[index], (Vector2)p.transform.position - ((Vector2)p.transform.position - PlayerController.mouseWorldPosition).normalized);
        slots[index] = null;

        JQUI.InventoryController.updateDisplay();
    }

    public virtual void onDrop(int index)
    {

    }

    public ItemStack addItemToInventory(ItemStack item)
    {
        foreach(ItemStack slot in slots)
        {
            if (item == null) return null;
            if (slot == null) continue;
            item = slot.addItem(item);
        }
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;
                return null;
            }
        }
        return item;
    }

    public ItemStack addItemToSlot(int index, ItemStack item)
    {
        return slots[index].addItem(item);
    }

    /// <summary>
    /// Occurs when the player closes the main inventory,
    /// this event includes the main inventory and other containers
    /// </summary>
    public virtual void onClose()
    {

    }

    /// <summary>
    /// Occurs when clicking a slot in the inventory <br></br>
    /// Includes the index of the clicked slot.
    /// </summary>
    /// <param name="index"></param>
    public virtual void onClick(int index)
    {

    }

    /// <summary>
    /// This just launches an event to close all<br></br> inventories that have been opened when the <br></br>player closes the main inventory
    /// </summary>
    public static void closeOpenInventories()
    {
        foreach(Inventory inv in openInventories)
        {
            inv.onClose();
        }
        openInventories.Clear();
    }
}