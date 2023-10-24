using JQUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VendorMenuUI : MonoBehaviour
{
    public static bool isOpen = false;
    public Transform listingsParent;
    public GameObject menuDisplay;
    public VendorItemListing prefab;
    public List<VendorItemListing> listings = new List<VendorItemListing>();

    public VendorItemListing currentlySelected = null;

    public TMP_Text listingInfoDisplay;

    public static VendorMenuUI instance { private set; get; }

    public void Open(Vendor vendor)
    {
        if ( !isOpen )
        {
            isOpen = true;

            foreach (Vendor.Purchase v_purchase in vendor.purchases)
            {
                VendorItemListing listing = Instantiate(prefab, listingsParent);
                listing.InitializeButton(v_purchase.purchase, v_purchase.cost, this);
                listings.Add(listing);
            }

            menuDisplay.SetActive(true);
        }
    }

    private void Update()
    {
        if (isOpen && InventoryController.isOpen) close();
        else if (isOpen && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) || Input.GetKeyDown(KeyCode.Tab)) close();
    }

    public void viewTrade(VendorItemListing listing)
    {
        this.currentlySelected = listing;
    }

    public void PurchaseCurrentlySelected()
    {
        if (currentlySelected != null )
        {

        }
    }

    private void Awake()
    {
        instance = this;
        menuDisplay.SetActive(false);
        isOpen = false;
    }

    public void close()
    {
        if ( isOpen )
        {
            currentlySelected = null;
            isOpen = false;

            foreach (VendorItemListing listing in listings)
            {
                Destroy(listing.gameObject);
            }

            menuDisplay.SetActive(false);
        } 
    }
}
