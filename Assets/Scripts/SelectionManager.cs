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

    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
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
                if (Input.GetKeyDown(KeyCode.E))
                {
                    obj.Interact();
                }
            }
            else
            {
                interaction_Info_UI.SetActive(false);
            }

        }
        else
        {
            interaction_Info_UI.SetActive(false);
        }
    }

}