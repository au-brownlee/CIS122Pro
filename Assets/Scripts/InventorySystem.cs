using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();

    public List<String> ItemList = new List<String>();

    private GameObject ItemToAdd;

    private GameObject whatSlotToEquip;

    public bool isFull;

    public bool isOpen;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;

        PopulateSLotList();
    }

    private void PopulateSLotList()
    {
        slotList.Clear();
        slotList.Add(inventoryScreenUI.transform.Find("Right hand").gameObject);
        slotList.Add(inventoryScreenUI.transform.Find("Left hand").gameObject);
        Transform onBodyItemsUI = inventoryScreenUI.transform.Find("onBody");
        foreach (Transform child in onBodyItemsUI)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab) && !isOpen)
        {

            Debug.Log("open inv");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isOpen)
        {
            Debug.Log("close inv");
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }

    public void AddToInventory(string ItemName)
    {
        if (CheckIfFull()) { Debug.Log("the inventory is full"); return; }
        whatSlotToEquip = FindNextEmptySlot();
        ItemToAdd = Instantiate(Resources.Load<GameObject>(ItemName), 
            whatSlotToEquip.transform.position, 
            whatSlotToEquip.transform.rotation);
        ItemToAdd.transform.SetParent(whatSlotToEquip.transform);
        ItemList.Add(ItemName);
    }

    private GameObject FindNextEmptySlot()
    {
        return new GameObject();
    }

    private bool CheckIfFull()
    {
        return false;
    }


}