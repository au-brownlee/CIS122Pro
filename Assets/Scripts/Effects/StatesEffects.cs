using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatesEffects : MonoBehaviour
{
    public float MaxHealth = 50f;
    public float MaxTemperature = 100f;
    public float MaxHunger = 50f;

    protected bool dead = false;

    public float Health;
    public float Temperature;
    public float Hunger;
    public List<Effect> effects = new List<Effect>();

    public Dictionary<EffectGiver, Effect> areaEffects = new Dictionary<EffectGiver, Effect>();

    public bool burning = false;
    GameObject MyFire;

    public bool freezing = false;
    public bool hungry = false;
    public bool hurting = false;
    public bool healing = false;

    internal virtual void Start()
    {
        Health = MaxHealth;
        Hunger = MaxHunger;
        Temperature = MaxTemperature / 2;
    }

    void Update()
    {
        // reset statuses
        freezing = false;
        hungry = false;
        hurting = false;
        healing = false;

        float HealthWas = Health;
        // make a list of current effects
        if (Time.timeScale == 0) return;
        List<Effect> toDelete = new List<Effect>();
        List<Effect> allEffects = new List<Effect> (effects);
        foreach (EffectGiver EffectGiver in new List<EffectGiver>(areaEffects.Keys))
        {
            if (EffectGiver == null) areaEffects.Remove(EffectGiver);
            else allEffects.Add(areaEffects[EffectGiver]);
        }
        // apply effects
        foreach (Effect effect in allEffects)
        {
            // Debug.Log($"{effect.Name} {effect.Amount}");
            switch (effect.Name) {
                case "heat":
                    {
                        Temperature += effect.Amount * Time.deltaTime;
                        break;
                    }
                case "regen":
                    {
                        Health += effect.Amount * Time.deltaTime;
                        break;
                    }
                case "feed":
                    {
                        Hunger += effect.Amount * Time.deltaTime;
                        break;
                    }
            }
            effect.Duration -= Time.deltaTime;
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
        // Changes: Temperature
        if (Temperature >= MaxTemperature)
        {
            Temperature = MaxTemperature;
            if (!burning)
            {
                burning = true;
                var newChild = Instantiate(SpellSystem.Instance.onFire, transform.position, transform.rotation);
                newChild.transform.parent = transform;
                MyFire = newChild;
            }
        }
        else if (MaxTemperature * 0.4 < Temperature && Temperature  <= MaxTemperature * 0.6 
            && Hunger > MaxHunger * 0.7)
        {
            Health += 1 * Time.deltaTime;
        }
        else if (Temperature <= 0)
        {
            Temperature = 0;
            freezing = true;
            Health -= 5 * Time.deltaTime;
        }
        if (burning)
        {
            Health -= 7 * Time.deltaTime;
            Temperature += 2 * Time.deltaTime;
        }
        // Changes: Hunger
        if (Hunger >= MaxHunger) Hunger = MaxHunger;
        if (Hunger < MaxHunger * 0.25)
        {
            hungry = true;
        }
        if (Hunger <= 0)
        {
            Hunger = 0;
            Health -= 1 * Time.deltaTime;
        }
        // Changes: Health
        if (Health >= MaxHealth) Health = MaxHealth;
        else if (Health <= 0)
        {
            Health = 0;
            if (!dead) Die();
        }
        // Statuses
        if (Health < HealthWas || Health <= 0) hurting = true;
        else if (Health > HealthWas) healing = true;
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

    public virtual void Die()
    {
        dead = true;
        var Ai = gameObject.GetComponent<AI_Movement>();
        if (Ai)
        {
            if (MyFire) Destroy(MyFire);
            Ai.Die();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
