using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodVendor : Vendor
{
    private void Awake()
    {
        displayText.text = "<color=orange>[E]</color> " + text_display;
        StartCoroutine(load());
    }

    IEnumerator load()
    {
        yield return new WaitUntil(() => GAMEINITIALIZER.instance);
        ConsumableItem[] consumableItems = GAMEINITIALIZER.instance.itemRegistry.registeredConsumableItems.ToArray();

        Purchase[] purchases = new Purchase[Mathf.Clamp(consumableItems.Length, 0, 15)];

        List<int> alreadyPicked = new List<int>();
        int i = 0;
        while (alreadyPicked.Count < purchases.Length)
        {
            if (!alreadyPicked.Contains(i))
            {
                if (Random.Range(0f, 1f) < 0.5f)
                {
                    Item consumable = consumableItems[i];
                    Purchase purchase = new Purchase();
                    int count = Random.Range(1, consumable.maxStackSize);
                    purchase.purchase = new ItemStack(consumable, count);
                    int level = purchase.purchase.item.itemStatistics.Level;
                    ItemStack cost = new ItemStack(Currency, (int)Mathf.Round(level * 1.4f * count));
                    purchase.cost = cost;
                    purchases[alreadyPicked.Count] = purchase;
                    alreadyPicked.Add(i);
                }
            }
            i++;
            if (i == consumableItems.Length) i = 0;
        }

        this.purchases = purchases;
    }
}
