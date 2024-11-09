using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellSystem : MonoBehaviour
{
    public static SpellSystem Instance { get; set; }

    public GameObject FireSpellUi;
    public GameObject NoteItems;

    public GameObject Player;

    public GameObject Manager;

    public GameObject Focus;

    public GameObject FireSparcle;
    public GameObject FireWave;
    public GameObject FireBall;
    public GameObject FirePlace;

    public GameObject onFire;

    public bool isOpen;

    public List<GameObject> symbolList = new List<GameObject>();

    int SymIndex = 0;
    public List<char> SpellText = new List<char>(6) {' ', ' ', ' ', 
                                                ' ', ' ', ' '};

    string Letters = " I,^i*@$";

    List<Sprite> SpellImages = new List<Sprite>() {null };


    List<KeyCode> keys = new List<KeyCode>() { KeyCode.Alpha1,
        KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6};

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
        if (SymIndex >= 6) return;

        for (int i = 0; i < keys.Count; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                GameObject Symbol = null;

                Symbol = symbolList[SymIndex].transform.Find("S" + (i + 1).ToString()).gameObject;
                Symbol.SetActive(true);

                symbolList[SymIndex].transform.Find("glow").gameObject.SetActive(true);
                SpellText[SymIndex] = Letters[i + 1];
                SymIndex++;
                if (SymIndex >= 6)
                {
                    break;
                }
                symbolList[SymIndex].transform.Find("glow").gameObject.SetActive(true);
                break;
            }
        }
        

    }

    public void StartCast()
    {
        FireSpellUi.SetActive(true);
        NoteItems.SetActive(false);
        isOpen = true;
        GameObject Symbol = null;
        SymIndex = 0;
        foreach (GameObject symbol in symbolList)
        {
            for (int j = 0; j < 6; j++)
            {
                Symbol = symbol.transform.Find("S" + (j + 1).ToString()).gameObject;
                Symbol.SetActive(false);
            }
            symbol.transform.Find("glow").gameObject.SetActive(false);
        }
        SpellText = new List<char>(6) {' ', ' ', ' ', 
                                       ' ', ' ', ' '};
        symbolList[0].transform.Find("glow").gameObject.SetActive(true);
    }

    public void EndCast()
    {
        bool hold = false;
        bool power = false;
        int time = 1;
        bool specificTarget = false;
        GameObject target = Manager.GetComponent<SelectionManager>().Target;
        GameObject SpellTarget = null;
        foreach (char sym in SpellText)
        {
            switch (sym)
            {
                case 'I': { hold = true; break; }
                case ',': { power = true; break; }
                case '^': { time = 2; break; }
                case 'i': {
                        specificTarget = true;
                        SpellTarget = Player;
                        break; }
                case '*':
                    {
                        specificTarget = true;
                        if (SpellTarget) break;
                        if (target && target.GetComponent<InteractableObject>().Creature)
                        {
                            SpellTarget = target;
                        }
                        break;
                    }
                case '@':
                    {
                        specificTarget = true;
                        if (SpellTarget) break;
                        if (target && target.GetComponent<InteractableObject>().Pickable)
                        {
                            SpellTarget = target;
                        }
                        break;
                    }
            }
        }
        if (!SpellTarget)
        {
            if (specificTarget) { Discard(); return; }

            GameObject newSpell = null;

            if (hold && power) newSpell = FirePlace;
            else if (hold) newSpell = FireWave;
            else if (power) newSpell = FireBall;
            else newSpell = FireSparcle;

            var newChild = Instantiate(newSpell, Focus.transform.position, Focus.transform.rotation);
            newChild.transform.parent = transform;
        }
        else
        {
            Debug.Log($"Cast on ({SpellTarget})");
            StatesEffects state = SpellTarget.GetComponent<StatesEffects>();
            if (!state) { Discard(); return; }
            if (hold && power) state.effect("heat", 50, 1 * time);
            else if (hold) state.effect("heat", 1, 20 * time);
            else if (power) state.effect("heat", 10, 1 * time);
            else state.effect("heat", 1, 1 * time);
        }

        Discard();
    }

    public void Discard()
    {
        FireSpellUi.SetActive(false);
        NoteItems.SetActive(true);
        isOpen = false;
    }

}
