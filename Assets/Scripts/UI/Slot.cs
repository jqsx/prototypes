using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JQUI
{

    public class Slot : Button
    {
        RectTransform rect;
        public int slotNumber;
        public Inventory parent;
        public bool isPointerOnButton = false;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }
        private void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 5f);


            if (isPointerOnButton && Input.GetKeyDown(KeyCode.Q))
            {
                parent.drop(slotNumber);
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            transform.localScale = Vector3.one * Random.Range(1.1f, 1.2f);

            isPointerOnButton = true;

            if (InventoryController.instance.hoverSlot != this)
            {
                InventoryController.instance.hoverSlot = this;
                InventoryController.instance.UpdateHoverSlotDisplay();
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            isPointerOnButton = false;
            if (InventoryController.instance.hoverSlot == this)
            {
                InventoryController.instance.hoverSlot = null;
                InventoryController.instance.UpdateHoverSlotDisplay();
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            InventoryController.instance.slotClicked(this);
            parent.onClick(slotNumber);
        }
    }
}