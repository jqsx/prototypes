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
    private float actionDelay = 0f;

    public static VendorMenuUI instance { private set; get; }

    private Vendor current;

    public void Open(Vendor vendor)
    {
        if ( !isOpen )
        {
            if (actionDelay > Time.time) return;
            actionDelay = Time.time + 0.1f;
            isOpen = true;
            current = vendor;
            listingInfoDisplay.text = "";
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
        else if (isOpen && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Tab))) close();

        if ( isOpen && current)
        {
            if (Vector2.Distance(Camera.main.transform.position, current.transform.position) > 2.5f)
            {
                close();
            }
        }
    }

    public void viewTrade(VendorItemListing listing)
    {
        this.currentlySelected = listing;

        listingInfoDisplay.text = "";

        string color = "white";
        int level = listing.purchase.item.itemStatistics.Level;
        if (level < 10) color = "yellow";
        else if (level < 20) color = "green";
        else if (level < 30) color = "blue";
        else if (level < 40) color = "purple";
        else color = "red";
        listingInfoDisplay.text += "<color=" + color + ">[" + level + "]<b>" + listing.purchase.item.name + "</b></color>\n";
        listingInfoDisplay.text += "<color=#666666><i>" + listing.purchase.item.itemType + "</i></color>";
        listingInfoDisplay.text += listing.purchase.item.itemStatistics.toString();
        listingInfoDisplay.text += "\n";
        listingInfoDisplay.text += "<color=#999999>" + listing.purchase.item.description + "</color>";
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
            if (actionDelay > Time.time) return;
            actionDelay = Time.time + 0.1f;
            currentlySelected = null;
            isOpen = false;
            current = null;

            foreach (VendorItemListing listing in listings)
            {
                Destroy(listing.gameObject);
            }
            listings.Clear();

            menuDisplay.SetActive(false);
        } 
    }
}
