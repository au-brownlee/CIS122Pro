using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName = "???";
    public bool Pickable = false;

    public string Interact()
    {
        if (Pickable)
        {
            Debug.Log($"{GetItemName()} added to inventory");
            Destroy(gameObject);
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
