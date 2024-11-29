using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class ItemSlot : MonoBehaviour, IDropHandler
{

    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");

        
        //if there is not item already then set our item.
        if (Item)
        {
            GameObject myItem = transform.GetChild(0).gameObject;
            myItem.transform.SetParent(DragDrop.itemBeingDragged.transform.GetComponent<DragDrop>().startParent);
            myItem.transform.localPosition = new Vector2(0, 0);

        }
        DragDrop.itemBeingDragged.transform.SetParent(transform);
        DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);


    }




}