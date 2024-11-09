using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public GameObject NoteItems;

    public List<GameObject> slotList = new List<GameObject>();

    private GameObject ItemToAdd;

    private GameObject whatSlotToEquip;

    public bool isFull;

    public bool isOpen;

    public GameObject rightHandVisual;
    public GameObject leftHandVisual;

    public GameObject Focus;
    public GameObject WorldItems;

    public GameObject RightHandSlot { 
        get {
            if (slotList.Count > 0)
            {
                return slotList[0];
            }
            return null;
        } }
    public GameObject LeftHandSlot {
        get
        {
            if (slotList.Count > 1)
            {
                return slotList[1];
            }
            return null;
        } }

    public GameObject RightHandItem
    {
        get
        {
            if (!SlotIsEmpty(RightHandSlot))
            {
                return RightHandSlot.transform.GetChild(0).gameObject;
            }
            return null;
        }
    }
    public GameObject LeftHandItem
    {
        get
        {
            if (!SlotIsEmpty(LeftHandSlot))
            {
                return LeftHandSlot.transform.GetChild(0).gameObject;
            }
            return null;
        }
    }
    public DragDrop RightHandItemData
    {
        get
        {
            if (RightHandItem)
            {
                return RightHandItem.GetComponent<DragDrop>();
            }
            return null;
        }
    }
    public DragDrop LeftHandItemData
    {
        get
        {
            if (LeftHandItem)
            {
                return LeftHandItem.GetComponent<DragDrop>();
            }
            return null;
        }
    }

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
        if (RightHandItem) rightHandVisual.transform.Find(RightHandItemData.ItemName).gameObject.SetActive(true);
        if (LeftHandItem) leftHandVisual.transform.Find(LeftHandItemData.ItemName).gameObject.SetActive(true);
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log("close inv");
                inventoryScreenUI.SetActive(false);
                NoteItems.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                isOpen = false;
            }
        }
        else if (!SpellSystem.Instance.isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log("open inv");
                inventoryScreenUI.SetActive(true);
                NoteItems.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                isOpen = true;
            }
            // crafting
            if (Input.GetMouseButtonDown(1))
            {
                if (RightHandItem && LeftHandItem)
                {
                    if ((RightHandItemData.ItemName == "stick" && LeftHandItemData.ItemName == "Crystall") ||
                        (RightHandItemData.ItemName == "Crystall" && LeftHandItemData.ItemName == "stick"))
                    {
                        Destroy(RightHandItem);
                        Destroy(LeftHandItem);
                        CreateNewItem("Wand", 100, slotList[0]);
                    }
                }
            }
            // dropping
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (RightHandItem) 
                { 
                    DropItem(RightHandItem); 
                }
                else if (LeftHandItem)
                {
                    DropItem(LeftHandItem);
                }
            }
            // using
            if (Input.GetMouseButtonDown(0))
            {
                if (RightHandItem)
                {
                    if (RightHandItemData.ItemName == "Wand")
                    {
                        SpellSystem.Instance.StartCast(RightHandItem);
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpellSystem.Instance.EndCast();
            }
            if (Input.GetMouseButtonDown(1))
            {
                SpellSystem.Instance.Discard();
            }
        }

        

    }

    public bool AddToInventory(GameObject Item)
    {
        if (CheckIfFull()) 
        {
            isFull = true;
            Debug.Log("the inventory is full"); 
            return false; 
        }
        whatSlotToEquip = FindNextEmptySlot();
        NewItem(Item, whatSlotToEquip);
        return true;
    }

    private void NewItem(GameObject ItemToAdd, GameObject toSlot)
    {
        string name = ItemToAdd.GetComponent<InteractableObject>().ItemName;
        int score = ItemToAdd.GetComponent<InteractableObject>().ItemScore;
        CreateNewItem(name, score, toSlot);
    }

    public void CreateNewItem(string name, int score, GameObject toSlot)
    {
        ItemToAdd = Instantiate(Resources.Load<GameObject>(name),
            toSlot.transform.position,
            toSlot.transform.rotation);
        ItemToAdd.GetComponent<DragDrop>().ItemName = name;
        ItemToAdd.GetComponent<DragDrop>().ItemScore = score;
        ItemToAdd.transform.SetParent(toSlot.transform);
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
        int score = ItemToDrop.GetComponent<DragDrop>().ItemScore;
        ItemToAdd = Instantiate(
            Resources.Load<GameObject>(name + "3d"),
            Focus.transform.position,
            Focus.transform.rotation);
        ItemToAdd.GetComponent<InteractableObject>().ItemName = name;
        ItemToAdd.GetComponent<InteractableObject>().ItemScore = score;
        ItemToAdd.transform.SetParent(WorldItems.transform);
        Destroy(ItemToDrop.gameObject);
    }
}