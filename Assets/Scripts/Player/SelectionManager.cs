using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{

    public GameObject interaction_Info_UI;
    public int interaction_Distance = 10;
    TextMeshProUGUI interaction_text;

    public GameObject Target = null;

    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (InventorySystem.Instance.isOpen) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interaction_Distance))
        {
            var selectionTransform = hit.transform;

            if (selectionTransform.GetComponent<InteractableObject>())
            {
                InteractableObject obj = selectionTransform.GetComponent<InteractableObject>();
                interaction_text.text = obj.GetItemText();
                interaction_Info_UI.SetActive(true);
                Target = selectionTransform.gameObject;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    obj.Interact();
                }
            }
            else
            {
                Target = null;
                interaction_Info_UI.SetActive(false);
            }

        }
        else
        {
            Target = null;
            interaction_Info_UI.SetActive(false);
        }
    }

}