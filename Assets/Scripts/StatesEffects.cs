using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatesEffects : MonoBehaviour
{
    public GameObject state_Info_UI;
    TextMeshProUGUI state_text;

    public int Health = 50;
    public int Temperature = 50;
    public List<Effect> effects = new List<Effect>();

    bool burning = false;

    private int nextUpdate = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (state_Info_UI) state_text = state_Info_UI.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdateEffects();
            if (state_text)  state_text.text = $"Health: {Health}\nTemperature: {Temperature}";
        }

    }

    void UpdateEffects()
    {
        List<Effect> toDelete = new List<Effect>();
        foreach (Effect effect in effects)
        {
            if (effect.Name == "heat")
            {
                Temperature += effect.Amount;
                effect.Duration -= 1;
                if (effect.Duration <= 0)
                {
                    toDelete.Add(effect);
                }
            }
        }
        foreach (Effect effect in toDelete)
        {
            effects.Remove(effect);
        }
        if (Temperature < 0) Temperature = 0;
        if (Temperature > 100) Temperature = 100;
        if (Temperature <= 10)
        {
            Health -= 5;
        }
        if (Temperature <= 10)
        {
            Health -= 3;
        }
        if (Temperature >= 90)
        {
            if (!burning)
            {
                burning = true;
                var newChild = Instantiate(SpellSystem.Instance.onFire, transform.position, transform.rotation);
                newChild.transform.parent = transform;
            }
        }
        if (burning)
        {
            Health -= 7;
        }
    }

    public void effect(string aName, int anAmount, int aDuration)
    {
        effects.Add(new Effect(aName, anAmount, aDuration));
    }
}
