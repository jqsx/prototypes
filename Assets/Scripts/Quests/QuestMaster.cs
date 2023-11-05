using JQUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestMaster : MonoBehaviour
{
    public TMP_Text displayText;
    void Start()
    {
        
    }

    void Update()
    {
        if (!InventoryController.isOpen && !VendorMenuUI.isOpen && Vector2.Distance(transform.position, Camera.main.transform.position) < 2)
        {
            displayText.transform.localScale = Vector3.Lerp(displayText.transform.localScale, Vector3.one, Time.deltaTime * 10f);
            if (Input.GetKeyDown(KeyCode.E))
            {
                QuestMenuUI.instance.open(this);
            }
        }
        else
        {
            displayText.transform.localScale = Vector3.Lerp(displayText.transform.localScale, Vector3.zero, Time.deltaTime * 10f);
        }
    }
}
