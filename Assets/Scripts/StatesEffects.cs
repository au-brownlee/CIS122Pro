using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatesEffects : MonoBehaviour
{
    public GameObject state_Info_UI;
    public GameObject deathScreen;
    public GameObject Frost;
    TextMeshProUGUI state_text;


    public int MaxHealth = 50;
    public int MaxTemperature = 100;

    public bool mortal = true;

    public int Health;
    public int Temperature;
    public List<Effect> effects = new List<Effect>();

    bool burning = false;

    private int nextUpdate = 0;

    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
        Temperature = MaxTemperature / 2;
        if (state_Info_UI) state_text = state_Info_UI.GetComponent<TextMeshProUGUI>();
        if (!mortal) effects.Add(new Effect("heat", -1, -1));
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
                if (effect.Duration == 0)
                {
                    toDelete.Add(effect);
                }
            }
        }
        foreach (Effect effect in toDelete)
        {
            effects.Remove(effect);
        }
        if (Frost) Frost.SetActive(false);
        // Changes
        if (Temperature <= 10)
        {
            if (Frost) Frost.SetActive(true);
            Health -= 5;
        }
        if (MaxTemperature / 2 - 10 < Temperature && Temperature  <= MaxTemperature / 2 + 10)
        {
            Health += 1;
        }
        if (Temperature >= MaxTemperature - 10)
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
            Temperature += 2;
        }
        // Fixes
        if (Temperature <= 0) Temperature = 0;
        if (Temperature >= MaxTemperature) Temperature = MaxTemperature;
        if (Health <= 0)
        {
            Health = 0;
            if (mortal)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = new Vector3(0, -10000, 0);
                deathScreen.SetActive(true);
            }
        }
        if (Health >= MaxHealth) Health = MaxHealth;
    }

    public void effect(string aName, int anAmount, int aDuration)
    {
        effects.Add(new Effect(aName, anAmount, aDuration));
    }
}
