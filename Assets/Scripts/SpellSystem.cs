using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellSystem : MonoBehaviour
{
    public static SpellSystem Instance { get; set; }

    public GameObject FireSpellUi;

    public bool isOpen;

    public List<GameObject> symbolList = new List<GameObject>();

    int SymIndex = 0;
    List<char> SpellText = new List<char>(6);

    string Letters = " I,^i*@$";

    List<Sprite> SpellImages = new List<Sprite>() {null };

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

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;

        foreach (Transform child in FireSpellUi.transform.Find("symbols"))
        {
            if (child.CompareTag("Symbol"))
            {
                symbolList.Add(child.gameObject);
            }
        }
        int i = 1;
        while (i < 7)
        {
            SpellImages.Add(Resources.Load<Sprite>("Spells/SP" + i));
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen) return;

        List<KeyCode> keys = new List<KeyCode>() { KeyCode.Alpha1,
        KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6};

        for (int i = 0; i < keys.Count; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                GameObject Symbol = symbolList[SymIndex].transform.Find("Symbol").gameObject;
                Symbol.GetComponent<Image>().sprite = SpellImages[i];
                //Debug.Log($"{SpellText}[{SymIndex}], {Letters}[{i}]");
                //Debug.Log("ST: " + SpellText[SymIndex]);
                //Debug.Log("L:" + Letters[i]);
                SpellText[SymIndex] = Letters[i];
            }
        }
        

    }

    public void StartCast()
    {
        FireSpellUi.SetActive(true);
        isOpen = true;
        foreach (GameObject symbol in symbolList)
        {
            symbol.transform.Find("Symbol").gameObject.SetActive(false);
            symbol.transform.Find("glow").gameObject.SetActive(false);
        }
        SymIndex = 0;
        SpellText = new List<char>(6) {' ', ' ', ' ', 
                                       ' ', ' ', ' '};
        symbolList[0].transform.Find("glow").gameObject.SetActive(true);
    }

    public void EndCast()
    {
        FireSpellUi.SetActive(false);
        isOpen = false;
    }

    public void Discard()
    {
        FireSpellUi.SetActive(false);
        isOpen = false;
    }

}
