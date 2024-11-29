using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName = "???";
    public int ItemScore = 1;
    public bool Pickable = false;
    public bool Creature = false;
    

    public InteractableObject(string itemName, int itemScore, bool pickable, bool creature)
    {
        ItemName = itemName;
        ItemScore = itemScore;
        Pickable = pickable;
        Creature = creature;
    }

    public string Interact()
    {
        if (Pickable)
        {
            Debug.Log($"{GetItemName()} added to inventory");
            if (InventorySystem.Instance.AddToInventory(gameObject)) {
                Destroy(gameObject);
            }
        }
        return GetItemName();
    }
    public string GetItemName()
    {
        return ItemName;
    }
    public string GetItemText()
    {
        if (Pickable) return GetItemName() + " (e)";
        return GetItemName();
    }

    private void Update()
    {
        
    }
}
