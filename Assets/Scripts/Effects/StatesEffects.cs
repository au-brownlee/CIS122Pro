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


    public float MaxHealth = 50f;
    public float MaxTemperature = 100f;

    public bool mortal = true;

    public float Health;
    public float Temperature;
    public List<Effect> effects = new List<Effect>();

    public Dictionary<EffectGiver, Effect> areaEffects = new Dictionary<EffectGiver, Effect>();

    bool burning = false;

    private float nextUpdate = 0;
    private float deltaTime = 0.05f;

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
            nextUpdate = Time.time + deltaTime;
            UpdateEffects();
            if (state_text)  state_text.text = $"Health: {Health}\nTemperature: {Temperature}";
        }

    }

    void UpdateEffects()
    {
        List<Effect> toDelete = new List<Effect>();
        List<Effect> allEffects = new List<Effect> (effects);
        foreach (EffectGiver EffectGiver in new List<EffectGiver>(areaEffects.Keys))
        {
            if (EffectGiver == null) areaEffects.Remove(EffectGiver);
            else allEffects.Add(areaEffects[EffectGiver]);
        }
        foreach (Effect effect in allEffects)
        {
            Debug.Log($"{effect.Name} {effect.Amount}");
            switch (effect.Name) {
                case "heat":
                    {
                        Temperature += effect.Amount * deltaTime;
                        break;
                    }
                case "regen":
                    {
                        Health += effect.Amount * deltaTime;
                        break;
                    }
            }
            effect.Duration -= deltaTime;
            if (-1 < effect.Duration && effect.Duration <= 0)
            {
                toDelete.Add(effect);
            }
        }
        foreach (Effect effect in toDelete)
        {
            if (effects.Contains(effect))
            {
                effects.Remove(effect);
            }
        }
        if (Frost) Frost.SetActive(false);
        // Changes
        if (Temperature <= MaxTemperature * 0.1)
        {
            if (Frost) Frost.SetActive(true);
            Health -= 5 * deltaTime;
        }
        if (MaxTemperature * 0.4 < Temperature && Temperature  <= MaxTemperature * 0.6)
        {
            Health += 1 * deltaTime;
        }
        if (Temperature >= MaxTemperature * 0.9)
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
            Health -= 7 * deltaTime;
            Temperature += 2 * deltaTime;
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
                deathScreen.GetComponent<GameOver>().Setup();
            }
        }
        if (Health >= MaxHealth) Health = MaxHealth;
    }

    public void effect(string aName, float anAmount, float aDuration)
    {
        effects.Add(new Effect(aName, anAmount, aDuration));
    }

    private void OnTriggerEnter(Collider other)
    {
        EffectGiver source = other.gameObject.GetComponent<EffectGiver>();
        if (source)
        {
            areaEffects[source] = new Effect(source.EffectName, source.EffectAmount, 2);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        EffectGiver source = other.gameObject.GetComponent<EffectGiver>();
        if (source && areaEffects.ContainsKey(source))
        {
            areaEffects.Remove(source);
        }
    }
}
