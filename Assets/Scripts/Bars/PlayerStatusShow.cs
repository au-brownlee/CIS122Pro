using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusShow : MonoBehaviour
{
    public GameObject TemperatureBar;
    public GameObject HungerBar;
    public GameObject HealthBar;

    public GameObject FrostUI;
    public GameObject HungerUI;
    public GameObject DamageUI;
    public GameObject HealingUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StatesEffects stats = GetComponent<StatesEffects>();
        TemperatureBar.GetComponent<StatusBar>().SetSlider(stats.Temperature, stats.MaxTemperature, true);
        HungerBar.GetComponent<StatusBar>().SetSlider(stats.Hunger, stats.MaxHunger, true);
        HealthBar.GetComponent<StatusBar>().SetSlider(stats.Health, stats.MaxHealth, true);
        FrostUI.SetActive(stats.freezing);
        HungerUI.SetActive(stats.hungry);
        DamageUI.SetActive(stats.hurting);
        HealingUI.SetActive(stats.healing);
    }

}
