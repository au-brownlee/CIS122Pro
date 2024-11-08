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

    public GameObject rightHandVisual;
    public GameObject leftHandVisual;

    public GameObject Focus;
    public GameObject WorldItems;

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
        isFull = false;

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
        foreach (Transform child in rightHandVisual.transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in leftHandVisual.transform)
        {
            child.gameObject.SetActive(false);
        }
        DragDrop rightHand = null;
        DragDrop leftHand = null;
        bool rightHandEmpty = true;
        bool leftHandEmpty = true;
        if (!SlotIsEmpty(slotList[0]))
        {
            rightHand = slotList[0].transform.GetChild(0).GetComponent<DragDrop>();
            rightHandEmpty = false;
            rightHandVisual.transform.Find(rightHand.ItemName).gameObject.SetActive(true);

        }
        if (!SlotIsEmpty(slotList[1]))
        {
            leftHand = slotList[1].transform.GetChild(0).GetComponent<DragDrop>();
            leftHandEmpty = false;
            leftHandVisual.transform.Find(leftHand.ItemName).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpen)
            {
                Debug.Log("close inv");
                inventoryScreenUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                isOpen = false;
            }
            else
            {
                Debug.Log("open inv");
                inventoryScreenUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                isOpen = true;
            }
        }
        // crafting
        if (Input.GetMouseButtonDown(0) && !isOpen)
        {
            if (!leftHandEmpty && !rightHandEmpty)
            {
                if ((rightHand.ItemName == "stick" && leftHand.ItemName == "Crystall") || 
                    (rightHand.ItemName == "Crystall" && leftHand.ItemName == "stick"))
                {
                    Destroy(slotList[0].transform.GetChild(0).gameObject);
                    Destroy(slotList[1].transform.GetChild(0).gameObject);
                    NewItem("Wand", slotList[0]);
                }
            }
        }

    }

    public bool AddToInventory(string ItemName)
    {
        if (CheckIfFull()) 
        {
            isFull = true;
            Debug.Log("the inventory is full"); 
            return false; 
        }
        whatSlotToEquip = FindNextEmptySlot();
        NewItem(ItemName, whatSlotToEquip);
        return true;
    }

    private void NewItem(string ItemName, GameObject toSlot)
    {
        ItemToAdd = Instantiate(Resources.Load<GameObject>(ItemName),
            toSlot.transform.position,
            toSlot.transform.rotation);
        ItemToAdd.GetComponent<DragDrop>().ItemName = ItemName;
        ItemToAdd.transform.SetParent(toSlot.transform);
        ItemList.Add(ItemName);
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (SlotIsEmpty(slot))
            {
                return slot;
            }
        }
        return new GameObject();
    }

    private bool CheckIfFull()
    {
        foreach (GameObject slot in slotList)
        {
            if (SlotIsEmpty(slot))
            {
                return false;
            }
        }
        return true;
    }

    private bool SlotIsEmpty(GameObject slot)
    {
        return slot.transform.childCount == 0;
    }

    public void DropItem(GameObject ItemToDrop)
    {
        string name = ItemToDrop.GetComponent<DragDrop>().ItemName;
        ItemToAdd = Instantiate(
            Resources.Load<GameObject>(name + "3d"),
            Focus.transform.position,
            Focus.transform.rotation);
        ItemToAdd.GetComponent<InteractableObject>().ItemName = name;
        ItemToAdd.transform.SetParent(WorldItems.transform);
    }
}