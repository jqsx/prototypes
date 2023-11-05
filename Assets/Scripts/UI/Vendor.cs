using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JQUI;

public class Vendor : MonoBehaviour
{
    public Item Currency;

    [Header("Add the items you want available for purchase here")]
    public Item[] Purchaseables;

    public Purchase[] purchases = new Purchase[0];

    public string text_display = "Click to open...";

    public TMP_Text displayText;

    public bool isRandom = false;

    private int purchasedItems = 0;

    void Awake()
    {
        displayText.text = "<color=orange>[E]</color> " + text_display;
        if (isRandom) GenerateRandomPurchases();
    }


    void Update()
    {
        if (!InventoryController.isOpen && !VendorMenuUI.isOpen && Vector2.Distance(transform.position, Camera.main.transform.position) < 2)
        {
            displayText.transform.localScale = Vector3.Lerp(displayText.transform.localScale, Vector3.one, Time.deltaTime * 10f);
            if (Input.GetKeyDown(KeyCode.E))
            {
                VendorMenuUI.instance.Open(this);
            }
        }
        else
        {
            displayText.transform.localScale = Vector3.Lerp(displayText.transform.localScale, Vector3.zero, Time.deltaTime * 10f);
        }
    }

    public void GenerateRandomPurchases()
    {

    }

    public void ItemHasBeenPurchased()
    {
        purchasedItems++;
    }

    [System.Serializable]
    public class Purchase
    {
        public ItemStack purchase;
        public ItemStack cost;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 2);
    }
}
