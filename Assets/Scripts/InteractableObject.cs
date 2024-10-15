using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName = "mystery";
    public bool Pickable = false;

    public string Interact()
    {
        if (Pickable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log($"{GetItemName()} added to inventory");
                Destroy(gameObject);
            }
        }
        return GetItemName();
    }
    public string GetItemName()
    {
        return ItemName;
    }

    private void Update()
    {
        
    }
}
