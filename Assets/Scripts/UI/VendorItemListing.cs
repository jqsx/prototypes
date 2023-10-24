using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VendorItemListing : MonoBehaviour
{
    public TMP_Text itemInformationText;
    public Image itemIconDisplay;

    public ItemStack purchase;
    public ItemStack cost;

    public VendorMenuUI menu;
    
    public void InitializeButton(ItemStack purchase, ItemStack cost, VendorMenuUI menu)
    {
        this.purchase = purchase;
        this.cost = cost;
        this.menu = menu;

        itemIconDisplay.sprite = purchase.item.icon;

        string color = "white";
        int level = purchase.item.itemStatistics.Level;
        if (level < 10) color = "yellow";
        else if (level < 20) color = "green";
        else if (level < 30) color = "blue";
        else if (level < 40) color = "purple";
        else color = "red";

        itemInformationText.text = "<color=" + color + ">" + purchase.item.name + "</color> x" + purchase.amount + "\nPrice: " + cost.item.name + " x" + cost.amount;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(viewTrade);
    }

    public void viewTrade()
    {
        menu.viewTrade(this);
    }
}
