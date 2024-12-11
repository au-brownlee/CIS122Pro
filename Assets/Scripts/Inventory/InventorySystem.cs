using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject Player;
    public GameObject inventoryScreenUI;
    public GameObject PauseScreenUI;
    public GameObject NoteItems;
    public GameObject ManaBar;
    public GameObject deathScreenUI;

    public List<GameObject> slotList = new List<GameObject>();

    private GameObject ItemToAdd;

    private GameObject whatSlotToEquip;

    public bool isFull;

    public bool isOpen;

    public GameObject rightHandVisual;
    public GameObject leftHandVisual;

    public GameObject Focus;
    public GameObject WorldItems;

    internal PauseScreen pauseComponent;
    internal GameOver gameOverComponent;

    // gets and sets

    public bool canInteract { get { return !isOpen && !pauseComponent.paused && !gameOverComponent.isOver; } }
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

        pauseComponent = PauseScreenUI.GetComponent<PauseScreen>();
        gameOverComponent = deathScreenUI.GetComponent<GameOver>();

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

        if (RightHandItem && RightHandItemData.ItemMaxScore > 1)
        {
            ManaBar.SetActive(true);
            Slider slider = ManaBar.GetComponent<Slider>();
            slider.maxValue = RightHandItemData.ItemMaxScore;
            slider.value = RightHandItemData.ItemScore;
        }
        else
        {
            ManaBar.SetActive(false);
        }

        if (gameOverComponent.isOver) 
        {
            if (Input.GetKeyDown(KeyCode.Escape)) gameOverComponent.RestartButton();
        }
        else if (pauseComponent.paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) pauseComponent.UnPause();
        }
        else if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                inventoryScreenUI.SetActive(false);
                NoteItems.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                isOpen = false;
            }
        }
        else if (SpellSystem.Instance.isOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpellSystem.Instance.Press();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                SpellSystem.Instance.UnPress();
            }
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                SpellSystem.Instance.Discard();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                inventoryScreenUI.SetActive(true);
                NoteItems.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                isOpen = true;
            }
            if (Input.GetKeyDown(KeyCode.Escape)) pauseComponent.Pause();
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
                        CreateNewItem("Wand", 100, 100, slotList[0]);
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
                    if (RightHandItemData.ItemName == "meat")
                    {
                        Player.GetComponent<StatesEffects>().effect("feed", 15, 1);
                        Destroy(RightHandItem);
                    }
                }
            }
            // changing hands
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (RightHandItem && LeftHandItem)
                {
                    RightHandItem.transform.SetParent(LeftHandSlot.transform);
                    LeftHandItem.transform.SetParent(RightHandSlot.transform);
                }
                else if (RightHandItem)
                {
                    RightHandItem.transform.SetParent(LeftHandSlot.transform);
                }
                else if (LeftHandItem)
                {
                    LeftHandItem.transform.SetParent(RightHandSlot.transform);
                }
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
        float score = ItemToAdd.GetComponent<InteractableObject>().ItemScore;
        float maxScore = ItemToAdd.GetComponent<InteractableObject>().ItemMaxScore;
        CreateNewItem(name, score, maxScore, toSlot);
    }

    public void CreateNewItem(string name, float score, float maxScore, GameObject toSlot)
    {
        ItemToAdd = Instantiate(Resources.Load<GameObject>(name),
            toSlot.transform.position,
            toSlot.transform.rotation);
        ItemToAdd.GetComponent<DragDrop>().ItemName = name;
        ItemToAdd.GetComponent<DragDrop>().ItemScore = score;
        ItemToAdd.GetComponent<DragDrop>().ItemMaxScore = maxScore;
        ItemToAdd.transform.SetParent(toSlot.transform);
        RectTransform rect = ItemToAdd.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector2.one;
    }

    public void SpawnNewObject(string name, float score, float maxScore, GameObject Reference, Vector3 offset)
    {
        ItemToAdd = Instantiate(Resources.Load<GameObject>(name + "3d"),
            Reference.transform.position + offset,
            Reference.transform.rotation);
        ItemToAdd.GetComponent<InteractableObject>().ItemName = name;
        ItemToAdd.GetComponent<InteractableObject>().ItemScore = score;
        ItemToAdd.GetComponent<InteractableObject>().ItemMaxScore = maxScore;
        ItemToAdd.transform.SetParent(WorldItems.transform);
    }
    public void SpawnNewObject(string name, float score, float maxScore, GameObject Reference)
    {
        SpawnNewObject(name, score, maxScore, Reference, new Vector3(0, 0, 0));
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
        float score = ItemToDrop.GetComponent<DragDrop>().ItemScore;
        float maxScore = ItemToDrop.GetComponent<DragDrop>().ItemMaxScore;

        SpawnNewObject(name, score, maxScore, Focus);
        Destroy(ItemToDrop.gameObject);
    }
}